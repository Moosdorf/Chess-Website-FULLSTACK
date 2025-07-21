using DataLayer.DataServices;
using DataLayer.Entities.Chess;
using DataLayer.HelperMethods;
using DataLayer.HubServices;
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


    public async Task SendMessageToGroup(string user, string message, string sessionId)
    {
        await Clients.Group(sessionId).ReceiveMessage(user, message);
    }

    public GameHub(IGameManager gameManager, IChessDataService chessDataService)
    {
        _gameManager = gameManager;
        _chessDataService = chessDataService;
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

            _gameManager.JoinGame(session.Id, user);
            
            // add player 2 to the existing group
            await Groups.AddToGroupAsync(Context.ConnectionId, session.Id);

            // create the game (adding to database) and send the state to the users.
            (ChessGame game, ChessInfo chessState) = await _chessDataService.CreateGameAsync(session.WhitePlayer, session.BlackPlayer);
            await Clients.Group(session.Id).GameReady(_chessDataService.CreateChessModel(chessState, game, session.Id));
        }
    }

    public async Task MakeMove(int gameId, string sessionId, MoveModel move)
    {
        try
        {
            Console.WriteLine("making move");
            ChessGame? game = await _chessDataService.GetGameAsync(gameId);
            // check if user is part of the game here if (game.player1 || game.player2 == moveModel.id???) return BadRequest("user not part of game");
            if (game == null) await Clients.Caller.BadMove("Game null");

            ChessInfo chessState;
            // create chess state from moves
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
            await Clients.Group(sessionId).ReceiveMove(_chessDataService.CreateChessModel(chessState, game, sessionId));
        } catch (Exception ex)
        {
            Console.WriteLine($"MakeMove Exception: {ex}");
        }
        
    }

}
