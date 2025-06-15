using DataLayer.Entities.Chess.Piece;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Chess
{
    public class Move
    {
        public int Id { get; set; }

        [Required]
        public string MoveString { get; set; } = ""; // for example: "e4,e5"

        [Required]
        public required ChessGame ChessGame { get; set; }
    }
}
