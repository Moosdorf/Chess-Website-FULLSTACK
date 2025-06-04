using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class UserCustomization {
        public int UserId { get; set; }
        public User User { get; set; }

        public int CustomizationId { get; set; }
        public Customization Customization { get; set; }
    }
}
