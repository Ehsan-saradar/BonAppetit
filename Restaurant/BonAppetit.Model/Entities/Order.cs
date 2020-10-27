using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BonAppetit.Model.Entities
{
    public class Order
    {
        public long Id { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        [JsonIgnore]
        [ForeignKey("CustomerUser")]
        public string CustomerUserId { get; set; }
        [JsonIgnore]

        public virtual CustomerUser CustomerUser { get; set; }
        public string Status { get; set; }
        public bool? approved { get; set; }
    }
}
