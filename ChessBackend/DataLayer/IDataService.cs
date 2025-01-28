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
