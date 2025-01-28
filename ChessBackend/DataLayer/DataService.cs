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

   

    // users
    public User GetUser(int userId)
    {
        User user = db.Users.Include(x => x.ChessGames)
                            .Where(x => x.UserId == userId)
                            .First();
        return user;
    }

    public IList<User> GetUsers()
    {
        IList<User> users = db.Users.Include(x => x.ChessGames)
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
