using DataLayer.DataServices;
using DataLayer.Entities.Chess;
using DataLayer.Entities.Chess.Piece;
using DataLayer.Models.Chess;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using DataLayer.HelperMethods;

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
            (int id, Piece[][] game) = await db.CreateGameAsync(model.player1, model.player2);


            if (game == null)
            {
                return NotFound();
            }
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // Optional: Makes the JSON output more readable
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // Include properties even if they are null
            };

            var serializedOutput = JsonSerializer.Serialize(new { game, id }, options);
            return Ok(serializedOutput);
        }

        [HttpPut]
        [Route("{id}/move")]
        public async Task<IActionResult> Move(int id, [FromBody] MoveModel moveModel)
        {
            ChessGame? game = await db.GetGameAsync(id);
            // check if user is part of the game here if (game.player1 || game.player2 == moveModel.id???) return BadRequest("user not part of game");
            if (game == null) return BadRequest("CannotFindGame");

            // replay game to get to current state
            var chessBoard = ChessMethods.CreateGameBoard(); 
            game.Moves.ForEach(m => ChessMethods.MakeMove(chessBoard, m.MoveString));

            // validate move

            // make the move
            chessBoard = ChessMethods.MakeMove(chessBoard, moveModel.Move);
            var moveMade = await db.MoveAsync(id, moveModel.Move);

            if (!moveMade) return BadRequest("Cannot make move"); 
            return Ok(JsonSerializer.Serialize(new { chessBoard, game.Id }));
        }

        private object? CreateChessModel(ChessGame game)
        {
            throw new NotImplementedException();
        }




    }
}
