using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace BonAppetit.Model.Entities
{
    public class CustomerUser:IdentityUser
    {
        public string NationalCode { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        
        public  DateTime LastLoginDateTime { get; set; }
    }
}
