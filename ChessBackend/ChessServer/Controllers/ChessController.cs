using ChessServer.Models;
using DataLayer;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ChessServer.Controllers
{
    [ApiController]
    [Route("api/chess")]
    public class ChessController : ControllerBase
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
            PieceModel[][] game = db.CreateGame(model.player1, model.player2);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(JsonSerializer.Serialize(game));
        }

        [HttpPut]
        [Route("{id}/move")]
        public IActionResult Move([FromBody] MoveModel moveModel)
        {
            var chessState = JsonSerializer.Deserialize<PieceModel[][]>(moveModel.ChessState);

            Console.WriteLine(chessState);
            string pattern = @"\D"; 
            string moves = Regex.Replace(moveModel.Move, pattern, "");

            int attackerRow = Int32.Parse(moves.Substring(0, 1));
            int attackerCol = Int32.Parse(moves.Substring(1, 1)); 
            int victimRow = Int32.Parse(moves.Substring(2, 1));
            int victimCol = Int32.Parse(moves.Substring(3, 1));

            chessState[victimRow][victimCol] = chessState[attackerRow][attackerCol];
            chessState[attackerRow][attackerCol] = new PieceModel { piece = "blank", color = "blank" };

            Console.WriteLine(moves);

            return Ok(JsonSerializer.Serialize(chessState));
        }

        private object? CreateChessModel(ChessGame game)
        {
            throw new NotImplementedException();
        }



    }
}
