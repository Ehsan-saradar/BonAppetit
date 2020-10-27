using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace BonAppetit.Model.Entities
{
    public class RestaurantUser : IdentityUser
    {
        public string RestaurantName { get; set; }
        public string Address { set; get; }
        public string Desc { get; set; }
        public string Mobile { get; set; }
        public DateTime LastLoginDateTime { get; set; }
        public bool IsOpen { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}
