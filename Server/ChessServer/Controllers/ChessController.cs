using DataLayer.DataServices;
using DataLayer.Entities.Chess;
using DataLayer.HelperMethods;
using DataLayer.Models.Chess;
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
        readonly LinkGenerator _linkGenerator;

        public ChessController(IChessDataService chessDataService, LinkGenerator linkGenerator)
        {
            db = chessDataService;
            _linkGenerator = linkGenerator;
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
            (ChessGame game, ChessInfo chessState) = await db.CreateGameAsync(model.player1, model.player2);


            if (game == null)
            {
                return NotFound();
            }
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // Optional: Makes the JSON output more readable
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // Include properties even if they are null
            };

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
                Console.WriteLine("last FEN: " + game.Moves.Last().FEN);
                chessState = new ChessInfo(game.Moves.Last().FEN); // find last moves FEN to create state from
            }
            else
                chessState = new ChessInfo();

            // validate if the move can be made
            var canMove = chessState.Move(moveModel.Move);
            if (!canMove) return BadRequest("Cannot make move - dataservice");

            var FEN = ChessMethods.GenerateFEN(chessState);

            // change in the database
            var moveMade = await db.MoveAsync(id, moveModel.Move, FEN);
            if (!moveMade) return BadRequest("Cannot make move - database");
            return Ok(JsonSerializer.Serialize(db.CreateChessModel(chessState, game)));
        }
    }
}
