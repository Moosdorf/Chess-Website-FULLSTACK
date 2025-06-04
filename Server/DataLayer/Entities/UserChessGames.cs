using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class UserChessGames {
        public int UserId { get; set; }
        public User User { get; set; }

        public int ChessId { get; set; }
        public ChessGame Game { get; set; }
    }
}
