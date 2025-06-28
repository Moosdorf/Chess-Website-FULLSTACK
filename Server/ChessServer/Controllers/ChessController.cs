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
            (int id, ChessInfo game) = await db.CreateGameAsync(model.player1, model.player2);


            if (game == null)
            {
                return NotFound();
            }
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // Optional: Makes the JSON output more readable
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // Include properties even if they are null
            };

            var serializedOutput = JsonSerializer.Serialize(new ChessModel 
                { 
                    Chessboard = game.GameBoard, 
                    Id = id, 
                    IsWhite = true, 
                    Check = false
                }, 
                options);

            return Ok(serializedOutput);
        }

        [HttpPut]
        [Route("{id}/move")]
        public async Task<IActionResult> Move(int id, [FromBody] MoveModel moveModel)
        {
            ChessGame? game = await db.GetGameAsync(id);
            // check if user is part of the game here if (game.player1 || game.player2 == moveModel.id???) return BadRequest("user not part of game");
            if (game == null) return BadRequest("CannotFindGame");

            // create chess state from moves
            var chessState = new ChessInfo(game.Moves);

            // validate if the move can be made
            var canMove = chessState.Move(moveModel.Move);
            if (!canMove) return BadRequest("Cannot make move - dataservice");

            // change in the database
            var moveMade = await db.MoveAsync(id, moveModel.Move);
            if (!moveMade) return BadRequest("Cannot make move - database");
            Console.WriteLine(chessState);
            return Ok(JsonSerializer.Serialize(db.CreateChessModel(chessState, game)));
        }
    }
}
