using DataLayer.Entities.Users;
using DataLayer.HelperMethods;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbUpdateException = Microsoft.EntityFrameworkCore.DbUpdateException;

namespace DataLayer.DataServices;

public class DataService : IDataService
{
    private ChessContext _db;

    public DataService(ChessContext context)
    {
        _db = context;
    }


    // users
    public User GetUser(int userId)
    {
        User user = _db.Users.Where(x => x.Id == userId)
                            .First();
        return user;
    }

    public IList<User> GetUsers()
    {
        IList<User> users = _db.Users.ToList();

        return users;
    }

    public User? CreateUser(string username, string password)
    {
        User user = null;

        if (_db.Users.Any(user => user.Username == username))
        {
            return null;
        }
        var hasher = new Hashing();
        var (hashedPassword, salt) = hasher.Hash(password);
        user = new User
        {
            Username = username,
            Password = hashedPassword
        };
        _db.Add<User>(user);
        _db.SaveChanges();


        return user;
    }

    // authentication
    public bool LogIn(string username, string password)
    {
        bool loggedIn = false;

        loggedIn = _db.Database
                  .SqlQueryRaw<bool>("select * from check_login_credentials({0}, {1})", username, password)
                  .AsEnumerable()
                  .FirstOrDefault();
        
        

        if (loggedIn)
        {
            _db.Database.ExecuteSqlRaw("call login({0}, {1})", username, password);
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
