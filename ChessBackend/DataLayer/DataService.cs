using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities;

public class DataService : IDataService
{
    private ChessContext db;

    public DataService()
    {
        db = new ChessContext();
    }

    public ChessGame CreateGame(int userId1, int userId2)
    {
        int newId = db.ChessGames.Max(x => x.chessId) + 1;
        var chessGame =  db.ChessGames
                        .FromSqlRaw("call create_chess_game({0}, {1})", userId1, userId2)
                        .AsEnumerable()
                        .FirstOrDefault();


        if (chessGame == null)
        {
            return null;
        }
        return chessGame;

    }

    public ChessMove CreateMove(string move, int chessId)
    {
        throw new NotImplementedException();
    }

    public ChessGame EndGame(int chessId)
    {
        throw new NotImplementedException();
    }
    public ChessGame? GetGame(int chessId)
    {
        var game = db.ChessGames.Include(x => x.players)
                                .Where(x => x.chessId == chessId)
                                .Select(x => x)
                                .FirstOrDefault();

        if (game == null) return null;
        return game;
    }
    public IList<ChessGame> GetGames(int userId)
    {
        return db.Users.Where(x => x.userId == userId).Select(x => x.chessGames).First();
    }

    public IList<ChessGame> GetGames()
    {
        throw new NotImplementedException();
    }

    public IList<ChessMove> GetMoves(int chessId)
    {
        throw new NotImplementedException();
    }

    public User GetUser(int userId)
    {
        throw new NotImplementedException();
    }

    public IList<User> GetUsers()
    {
        throw new NotImplementedException();
    }

    public bool LogIn(string username, string password)
    {
        throw new NotImplementedException();
    }

    public bool LogOut(string username)
    {
        throw new NotImplementedException();
    }

    public bool RemoveLastMove(int chessId)
    {
        throw new NotImplementedException();
    }

    public bool StartSession()
    {
        throw new NotImplementedException();
    }

    public bool StopSession()
    {
        throw new NotImplementedException();
    }

    public bool UpdateCustomization()
    {
        throw new NotImplementedException();
    }

    public bool UpdatePassword(string newPassword)
    {
        throw new NotImplementedException();
    }
}
