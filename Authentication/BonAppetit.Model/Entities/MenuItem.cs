using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BonAppetit.Model.Entities
{
    public class MenuItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Price { get; set; }
        public long Num { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        [ForeignKey("RestaurantUser")]
        public string RestaurantUserId { get; set; }
        [JsonIgnore]

        public virtual RestaurantUser RestaurantUser { get; set; }
    }
}
