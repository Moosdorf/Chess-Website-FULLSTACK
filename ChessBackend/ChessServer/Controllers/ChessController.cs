using ChessServer.Models;
using DataLayer;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChessServer.Controllers
{
    [ApiController]
    [Route("api/chess")]
    public class ChessController : ControllerBase
    {
        IDataService db;
        readonly LinkGenerator _linkGenerator;

        public ChessController(IDataService dataService, LinkGenerator linkGenerator)
        {
            db = dataService;
            _linkGenerator = linkGenerator;
        }

        // create game
        [HttpPost]
        public IActionResult CreateGame(CreateChessModel model)
        {
            if (model == null)
            {
                return NotFound();
            }
            ChessGame game = db.CreateGame(model.player1, model.player2);

            if (game == null)
            {
                return NotFound();
            }

            return Created("GetGame", CreateChessModel(game));

        }

        private object? CreateChessModel(ChessGame game)
        {
            throw new NotImplementedException();
        }

        // get gamesate
        // get game history based on users
        // make move
        // resign game

        // join game ?? not sure


    }


}
