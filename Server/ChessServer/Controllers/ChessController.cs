﻿using DataLayer.DataServices;
using DataLayer.Entities.Chess;
using DataLayer.HelperMethods;
using DataLayer.IDataServices;
using DataLayer.Models.Chess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChessServer.Controllers
{
    [ApiController]
    [Route("api/chess")]
    public class ChessController : BaseController
    {
        IChessDataService db;
        IStockFishService stockFish;
        readonly LinkGenerator _linkGenerator;

        public ChessController(IChessDataService chessDataService, IStockFishService stockFishService, LinkGenerator linkGenerator)
        {
            db = chessDataService;
            _linkGenerator = linkGenerator;
            stockFish = stockFishService;
        }

        // create game
        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> CreateGame(CreateChessModel model)
        {
            if (model == null)
            {
                return NotFound();
            }

            (ChessGame game, ChessInfo chessState) = await db.CreateGameAsync(model.Player1, model.Player2);


            if (game == null)
            {
                return NotFound();
            }


            return Ok(JsonSerializer.Serialize(db.CreateChessModel(chessState, game)));
        }

        // create bot game
        [Authorize]
        [HttpPost]
        [Route("newbotgame")]
        public async Task<IActionResult> CreateBotGame(CreateBotChessModel model)
        {
            if (model == null)
            {
                Console.WriteLine("model null");
                return NotFound();
            }

            (ChessGame game, ChessInfo chessState) = await db.CreateBotGameAsync(model.Player1, model.PickedWhite);


            if (game == null)
            {
                Console.WriteLine("game null");
                return NotFound();
            }


            return Ok(JsonSerializer.Serialize(db.CreateChessModel(chessState, game)));
        }

        [HttpPut]
        [Route("{id}/move")]
        public async Task<IActionResult> Move(int id, [FromBody] MoveModel moveModel)
        {
            ChessGame? game = await db.GetGameAsync(id);
            // check if user is part of the game here if (game.player1 || game.player2 == moveModel.id???) return BadRequest("user not part of game");
            if (game == null) return BadRequest("CannotFindGame");

            ChessInfo chessState;
            // create chess state from moves
            if (game.Moves.Count > 0)
            {
                chessState = new ChessInfo(game.Moves.Last().FEN); // find last moves FEN to create state from
            }
            else
                chessState = new ChessInfo();

            // validate if the move can be made
            var canMove = chessState.Move(moveModel);
            if (!canMove) return BadRequest("Cannot make move - dataservice");

            var FEN = ChessMethods.GenerateFEN(chessState);

            // change in the database
            var moveMade = await db.MoveAsync(id, moveModel.Move, FEN);
            if (!moveMade) return BadRequest("Cannot make move - database");
            return Ok(JsonSerializer.Serialize(db.CreateChessModel(chessState, game)));
        }


        [HttpPut]
        [Route("{id}/moveBot")]
        public async Task<IActionResult> MoveBot(int id)
        {
            ChessGame? game = await db.GetGameAsync(id);
            // check if user is part of the game here if (game.player1 || game.player2 == moveModel.id???) return BadRequest("user not part of game");
            if (game == null) return BadRequest("CannotFindGame");

            ChessInfo chessState;
            // create chess state from moves
            if (game.Moves.Count > 0)
            {
                Console.WriteLine("atleast 1 move");
                chessState = new ChessInfo(game.Moves.Last().FEN); // find last moves FEN to create state from
            }
            else
            {
                Console.WriteLine("no moves made");
                chessState = new ChessInfo();
            }


            var lastFEN = ChessMethods.GenerateFEN(chessState);


            Console.WriteLine(lastFEN);
            var stockFishMove = stockFish.MoveFrom(lastFEN);

            // validate if the move can be made
            var canMove = chessState.Move(stockFishMove);
            if (!canMove) return BadRequest("Cannot make move - dataservice");

            var FEN = ChessMethods.GenerateFEN(chessState);
            Console.WriteLine("new fen: " + FEN);

            // change in the database
            var moveMade = await db.MoveAsync(id, stockFishMove.Move, FEN);
            if (!moveMade) return BadRequest("Cannot make move - database");
            return Ok(JsonSerializer.Serialize(db.CreateChessModel(chessState, game)));
        }

        [HttpGet]
        [Route("stockfish")]
        public async Task<IActionResult> StartStocky()
        {
            stockFish.StartNewStockFishGame();
            return Ok();
        }
    }
}
