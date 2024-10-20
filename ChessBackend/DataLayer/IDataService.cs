using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using Microsoft.OpenApi.Models;

namespace DataLayer
{
    public interface IDataService
    {
        // chess stuff
        ChessGame CreateGame(int userId1, int userId2);

        ChessMove CreateMove(string move, int chessId);

        bool RemoveLastMove(int chessId);

        ChessGame EndGame(int chessId);

        IList<ChessGame> GetGames(int userId);

        IList<ChessGame> GetGames();

        IList<ChessMove> GetMoves(int chessId);
        ChessGame? GetGame(int chessId);


        // user stuff
        IList<User> GetUsers();
        User GetUser(int userId);

        bool UpdateCustomization();


        // authentication stuff
        bool StartSession();

        bool LogIn(string username, string password);

        bool LogOut(string username);

        bool StopSession();

        bool UpdatePassword(string newPassword);

        
    }
}
