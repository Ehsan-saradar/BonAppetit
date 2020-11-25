using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonAppetit.Web.Models
{
    public class EditRestaurantRequest
    {
        public string RestaurantName { get; set; }
        public string Address { set; get; }
        public string Desc { get; set; }
        public string Password { get; set; }
    }
}
