using System.Collections.Generic;

namespace BonAppetit.Web.Models
{
    public class Order
    {
        public string ResturantId { get; set; }
        public List<OrderItems> OrderItems { get; set; }
    }
    public class OrderItems
    {
        public int Num { get; set; }
        public long Id { get; set; }

    }
}