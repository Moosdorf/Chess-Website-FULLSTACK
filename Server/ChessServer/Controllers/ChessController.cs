using DataLayer.DataServices;
using DataLayer.Entities.Chess;
using DataLayer.Entities.Chess.Piece;
using DataLayer.Models.Chess;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

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
        public IActionResult CreateGame(CreateChessModel model)
        {
            if (model == null)
            {
                return NotFound();
            }
            Piece[][] game = db.CreateGame(model.player1, model.player2);

            if (game == null)
            {
                return NotFound();
            }
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // Optional: Makes the JSON output more readable
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // Include properties even if they are null
            };

            var serializedOutput = JsonSerializer.Serialize(game, options);
            return Ok(serializedOutput);
        }

        [HttpPut]
        [Route("{id}/move")]
        public IActionResult Move([FromBody] MoveModel moveModel)
        {
            var chessState = JsonSerializer.Deserialize<Piece[][]>(moveModel.ChessState);

            string pattern = @"\D"; // pattern for removing all non ints
            string moves = Regex.Replace(moveModel.Move, pattern, "");

            int attackerRow = Int32.Parse(moves.Substring(0, 1)); // parse ints attackrow, attackcol, victimrow, victimcol)
            int attackerCol = Int32.Parse(moves.Substring(1, 1));
            int victimRow = Int32.Parse(moves.Substring(2, 1));
            int victimCol = Int32.Parse(moves.Substring(3, 1));

            // chessState = db.Move(chessState, (attackerRow, attackerCol), (victimRow, victimCol));

            if (chessState == null) return BadRequest("Cannot move");

            

            return Ok(JsonSerializer.Serialize(chessState));
        }

        private object? CreateChessModel(ChessGame game)
        {
            throw new NotImplementedException();
        }




    }
}
