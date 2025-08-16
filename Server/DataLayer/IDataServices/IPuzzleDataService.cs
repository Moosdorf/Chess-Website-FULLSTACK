using DataLayer.Entities.Chess;
using DataLayer.Entities.Users;
using DataLayer.Models.Puzzle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.IDataServices
{
    public interface IPuzzleDataService
    {
        PuzzleDTO GetRandomPuzzle();
        PuzzleDTO GetPuzzle(int rating, string theme);

    }
}
