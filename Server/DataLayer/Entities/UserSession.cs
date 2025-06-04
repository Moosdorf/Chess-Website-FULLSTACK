using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class UserSession
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int SessionId { get; set; }
        public Session Session { get; set; }
    }
}
