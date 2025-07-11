using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities.Users;

namespace DataLayer.DataServices
{
    public interface IDataService
    {
        // user stuff
        IList<User> GetUsers();
        Task<User> GetUser(int id);
        Task<User> GetUser(string username);
        User? CreateUser(string username, string password);
        bool UpdateCustomization();


        // authentication stuff
        bool StartSession();

        bool SignInUser(string username, string password);

        bool LogOut(string username);

        bool StopSession();

        bool UpdatePassword(string newPassword);
    }
}
