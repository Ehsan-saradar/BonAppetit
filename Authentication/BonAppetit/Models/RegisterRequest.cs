using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonAppetit.Web.Models
{
    /// <summary>
    /// Request to register new user
    /// </summary>
    public class RegisterRequest
    {
        public string NationalCode { get; set; }
        public string Mobile { get; set; }

        public  string Email { get; set; }
        public  string Password { get; set; }
        public string Address { get; set; }
    }
}
