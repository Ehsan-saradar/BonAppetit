using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonAppetit.Web.Models
{
    public class LoginRequest
    {
        public string Mobile { get; set; }
        public string Password { get; set; }
    }
}
