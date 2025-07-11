using DataLayer.Models.Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.IDataServices
{
    public interface IStockFishService
    {
        MoveModel MoveFrom(string fEN);
        public void StartNewStockFishGame();

    }
}
