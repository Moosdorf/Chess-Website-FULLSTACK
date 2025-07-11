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
    public async Task<User> GetUser(string username)
    {
        return await _db.Users.FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task<User> GetUser(int id)
    {
        return await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
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
            Password = hashedPassword,
            Salt = salt
        };
        _db.Add<User>(user);
        _db.SaveChanges();


        return user;
    }

    // authentication
    public bool SignInUser(string username, string password)
    {
        User? user = _db.Users.Where(user => user.Username == username).FirstOrDefault(); // default refers to the types default value, User is null. int is 0....

        if (user == null)
        {
            return false;
        }
        var hasher = new Hashing();

        return hasher.Verify(password, user.Password, user.Salt);
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
