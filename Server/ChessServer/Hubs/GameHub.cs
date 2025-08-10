using DataLayer.DataServices;
using DataLayer.Entities.Chess;
using DataLayer.Entities.Users;
using DataLayer.HelperMethods;
using DataLayer.HubServices;
using DataLayer.IDataServices;
using DataLayer.Models.Chess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;

namespace ChessServer.Hubs;

[Authorize]
public class GameHub : Hub<IGameHub>
{
    private readonly IGameManager _gameManager;
    private readonly IChessDataService _chessDataService;
    IStockFishService stockFish;

    public GameHub(IGameManager gameManager, IChessDataService chessDataService, IStockFishService stockFishService)
    {
        _gameManager = gameManager;
        _chessDataService = chessDataService;
        stockFish = stockFishService;
    }

    public override async Task OnDisconnectedAsync(Exception? ex)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

        Console.WriteLine("userid: " + userId);  
        Console.WriteLine("username: " + username);  

        if (username != null) _gameManager.RemoveUserFromSession(username);

        Console.WriteLine("disconnecting signal r");
        // do the logging here
        Trace.WriteLine(Context.ConnectionId + " - disconnected");


        await base.OnDisconnectedAsync(ex);
    }

    public async Task SendMessageToGroup(string message, string sessionId)
    {
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        if (username == null) return;
        await Clients.Group(sessionId).ReceiveMessage(username, message);
    }

    public async Task LeaveGame(string sessionId)
    {
        Console.WriteLine("leave game: ");

        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        if (username == null) return;
        Console.WriteLine("user name: " + username);


        var session = _gameManager.GetSession(username);
        if (session == null) return;
        Console.WriteLine("session " + session.Id);
            

        var result = (session.WhitePlayer == username) ? GameResult.BlackWin : GameResult.WhiteWin;
        _gameManager.RemoveUserFromSession(username);

        await _chessDataService.EndGame(session.GameId, result);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId);
        await Clients.Group(sessionId).ReceiveMessage("System", "Opponent has left");
    }

    public async Task StopQueue()
    {
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        _gameManager.RemoveUserFromSession(username);
        await Clients.Caller.QueueStopped("Queue stopped");
    }

    public async Task JoinGame(string user)
    {
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        Console.WriteLine("trying to join a game");
        // find an available session, if no session found then create one and wait
        var session = _gameManager.FindWaitingGame();

        if (username == null || _gameManager.GetSessionId(username) != null) return;


        // null indicates no session found
        if (session == null)
        {
            Console.WriteLine("session null!");
            // create the session, this wont add to database
            session = _gameManager.CreateGame(user);
            // add the caller to a group based on the session id and send info that opponent is missing
            await Groups.AddToGroupAsync(Context.ConnectionId, session.Id);
            await Clients.Caller.WaitingForOpponent();
        }
        else
        {   
            Console.WriteLine("session not null!");
            // session found, add this user to the session and initialize the session (will set values like who is black).

            _gameManager.JoinGameSession(session.Id, user);
            (ChessGame game, ChessInfo chessState) = await _chessDataService.CreateGameAsync(session.WhitePlayer, session.BlackPlayer);
            session.GameId = game.Id;
            if (game == null) return;

            
            
            // add player 2 to the existing group
            await Groups.AddToGroupAsync(Context.ConnectionId, session.Id);

            // create the game (adding to database) and send the state to the users.
            await Clients.Group(session.Id).GameReady(_chessDataService.CreateChessModel(chessState, game, session.Id));
        }
    }
    public async Task JoinBotGame(string user, bool isWhite)
    {
        Console.WriteLine("trying to join a bot game");
        var session = _gameManager.CreateGame(user);


        (ChessGame game, ChessInfo chessState) = await _chessDataService.CreateBotGameAsync(user, isWhite);


        _gameManager.JoinBotGame(session.Id, game, "stockfish");
        await Groups.AddToGroupAsync(Context.ConnectionId, session.Id);

        // create the game (adding to database) and send the state to the users.
        await Clients.Caller.GameReady(_chessDataService.CreateChessModel(chessState, game, session.Id));

        if (!isWhite) await MakeBotMove(game.Id, session.Id);
    }


    public async Task MakeBotMove(int gameId, string sessionId)
    {
        ChessGame? game = await _chessDataService.GetGameAsync(gameId);
        if (game == null)
        {
            await Clients.Caller.BadMove("Game null");
            return;
        }

        ChessInfo chessState;
        // create chess state from moves
        if (game.Moves.Count > 0)
        {
            chessState = new ChessInfo(game.Moves.Last().FEN); // find last moves FEN to create state from
        }
        else
        {
            chessState = new ChessInfo();
        }


        var lastFEN = ChessMethods.GenerateFEN(chessState);


        Console.WriteLine(lastFEN);
        var stockFishMove = stockFish.MoveFrom(lastFEN);

        // validate if the move can be made
        var canMove = chessState.Move(stockFishMove);
        if (!canMove) await Clients.Caller.BadMove("Cannot make move - dataservice");

        var FEN = ChessMethods.GenerateFEN(chessState);

        // change in the database
        var moveMade = await _chessDataService.MoveAsync(gameId, stockFishMove.Move, FEN);
        if (!moveMade) await Clients.Caller.BadMove("Cannot make move - database");
        await Clients.Caller.ReceiveMove(_chessDataService.CreateChessModel(chessState, game, sessionId));
    }

    public async Task ForfeitGame(string sessionId)
    {
        Console.WriteLine("forfeit game: ");

        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        if (username == null) return;
        Console.WriteLine("user name: " + username);


        var session = _gameManager.GetSession(username);
        if (session == null) return;
        Console.WriteLine("session " + session.Id);

        var result = (session.WhitePlayer == username) ? GameResult.BlackWin : GameResult.WhiteWin;
        await _chessDataService.EndGame(session.GameId, result);

        ChessInfo chessState;
        ChessGame? game = await _chessDataService.GetGameAsync(session.GameId);

        if (game != null && game.Moves.Count > 0)
        {
            chessState = new ChessInfo(game.Moves.Last().FEN); // find last moves FEN to create state from
        }
        else
            chessState = new ChessInfo();

        Console.WriteLine(game.Id);


        await Clients.Group(sessionId).EndGame(_chessDataService.CreateChessModel(chessState, game, sessionId));
        await Clients.Group(sessionId).ReceiveMessage("System", $"{username} has forfeit the game.");
        Console.WriteLine("sent messages to user");
    }

    public async Task SendDrawResponse(string sessionId, bool response)
    {
        Console.WriteLine("draw response: ");

        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        if (username == null) return;
        Console.WriteLine("user name: " + username);

        var session = _gameManager.GetSession(username);
        if (session == null) return;
        Console.WriteLine("session " + session.Id);

        ChessInfo chessState;
        ChessGame? game = await _chessDataService.GetGameAsync(session.GameId);

        if (game != null && game.Moves.Count > 0)
        {
            chessState = new ChessInfo(game.Moves.Last().FEN); // find last moves FEN to create state from
        }
        else
            chessState = new ChessInfo();


        if (!response) // if declined
        {
            await Clients.Group(sessionId).ReceiveMessage("System", $"{username} has declined the draw request.");
        } else
        {
            await _chessDataService.EndGame(session.GameId, GameResult.Draw);
            await Clients.Group(sessionId).EndGame(_chessDataService.CreateChessModel(chessState, game, sessionId));
            await Clients.Group(sessionId).ReceiveMessage("System", $"{username} has accepted the draw request.");
        }
        await Clients.Group(sessionId).ReceiveDrawResponse(response);

        Console.WriteLine("sent messages to user");
    }

    public async Task RequestDraw(string sessionId)
    {
        Console.WriteLine("requesting draw: ");

        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        if (username == null) return;
        Console.WriteLine("user name: " + username);


        var session = _gameManager.GetSession(username);
        if (session == null) return;
        Console.WriteLine("session " + session.Id);

        ChessInfo chessState;
        ChessGame? game = await _chessDataService.GetGameAsync(session.GameId);

        if (game != null && game.Moves.Count > 0)
        {
            chessState = new ChessInfo(game.Moves.Last().FEN); // find last moves FEN to create state from
        }
        else
            chessState = new ChessInfo();

        Console.WriteLine(game.Id);


        await Clients.Group(sessionId).ReceiveDrawRequest(username);
        await Clients.Group(sessionId).ReceiveMessage("System", $"{username} has requested a draw.");
        Console.WriteLine("sent messages to user");
    }

    public async Task MakeMove(int gameId, string sessionId, MoveModel move)
    {
        try
        {
            ChessGame? game = await _chessDataService.GetGameAsync(gameId);
            // check if user is part of the game here if (game.player1 || game.player2 == moveModel.id???) return BadRequest("user not part of game");
            if (game == null)
            {
                await Clients.Caller.BadMove("Game null");
                return;
            }
            if (game.Result != GameResult.Ongoing)
            {
                await Clients.Caller.BadMove("Game is already done");
                return;
            }

            ChessInfo chessState;

            if (game.Moves.Count > 0)
            {
                chessState = new ChessInfo(game.Moves.Last().FEN); // find last moves FEN to create state from
            }
            else
                chessState = new ChessInfo();

            // validate if the move can be made
            var canMove = chessState.Move(move);
            if (!canMove) await Clients.Caller.BadMove("Dataservice fail");

            var FEN = ChessMethods.GenerateFEN(chessState);

            // change in the database
            var moveMade = await _chessDataService.MoveAsync(gameId, move.Move, FEN);
            if (!moveMade) await Clients.Caller.BadMove("Database fail");


            if (game.BlackPlayer.Username == "stockfish" || game.WhitePlayer.Username == "stockfish")
            {
                await Clients.Caller.ReceiveMove(_chessDataService.CreateChessModel(chessState, game, sessionId));
                await MakeBotMove(game.Id, sessionId);
            }
            else await Clients.Group(sessionId).ReceiveMove(_chessDataService.CreateChessModel(chessState, game, sessionId));
        } catch (Exception ex)
        {
            Console.WriteLine($"MakeMove Exception: {ex}");
        }
        
    }

}
