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

    // chess stuff
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
    public ChessGame GetGame(int chessId)
    {
        var game = db.ChessGames.Include(x => x.Players)
                                .Where(x => x.chessId == chessId)
                                .Select(x => x)
                                .FirstOrDefault();

        return game;
    }
    public IList<UserChessGames> GetGames(int userId)
    {
        return db.Users.Where(x => x.UserId == userId).Select(x => x.ChessGames).First();
    }

    public IList<ChessGame> GetGames()
    {
        return db.ChessGames
                        .Include(x => x.Players)
                        .Include(x => x.moves)
                        .Select(x => x).ToList();
    }

    public IList<ChessMove> GetMoves(int chessId)
    {
        throw new NotImplementedException();
    }

    // users
    public User GetUser(int userId)
    {
        User user = db.Users.Include(x => x.UserCustomization)
                            .Include(x => x.ChessGames)
                            .Where(x => x.UserId == userId)
                            .First();
        return user;
    }

    public IList<User> GetUsers()
    {
        IList<User> users = db.Users.Include(x => x.UserCustomization)
                                    .Include(x => x.ChessGames)
                                    .AsSplitQuery()
                                    .ToList();

        return users;
    }

    // authentication
    public bool LogIn(string username, string password)
    {
        bool loggedIn = false;

        loggedIn = db.Database
                  .SqlQueryRaw<bool>("select * from check_login_credentials({0}, {1})", username, password)
                  .AsEnumerable()
                  .FirstOrDefault();
        
        

        if (loggedIn)
        {
            db.Database.ExecuteSqlRaw("call login({0}, {1})", username, password);
            return true;
        }

        return false;
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
