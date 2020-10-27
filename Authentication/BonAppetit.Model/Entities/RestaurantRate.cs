using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BonAppetit.Model.Entities
{
    public class RestaurantRate
    {
        public long Id { get; set; }
        [JsonIgnore]
        [ForeignKey("RestaurantUser")]
        public string RestaurantUserId { get; set; }
        [JsonIgnore]

        public virtual RestaurantUser RestaurantUser { get; set; }

        [JsonIgnore]
        [ForeignKey("Order")]
        public long OrderId { get; set; }
        [JsonIgnore]

        public virtual Order Order { get; set; }
    }
}
