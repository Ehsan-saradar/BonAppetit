using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BonAppetit.Model.Entities
{
    public class OrderItem
    {
        public long Id { get; set; }
        [JsonIgnore]
        [ForeignKey("Order")]
        public long OrderId { get; set; }
        [JsonIgnore]

        public virtual Order Order { get; set; }

        [JsonIgnore]
        [ForeignKey("MenuItem")]
        public long MenuItemId { get; set; }
        [JsonIgnore]

        public virtual MenuItem MenuItem { get; set; }
        public long Count { get; set; }
    }
}
