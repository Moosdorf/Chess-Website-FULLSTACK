using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.Chess
{
    public class CreateBotChessModel
    {
        public string Player1 { get; set; } = null!;
        public bool PickedWhite { get; set; }
    }
}
