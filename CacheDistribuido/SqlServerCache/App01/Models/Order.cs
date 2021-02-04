using System.Collections.Generic;

namespace App01.Models
{
    public class Order
    {
        public Order(string custommer)
        {
            Custommer = custommer;
        }
        public Order() { }

        public int Id { get; set; }
        public string Custommer { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
    }
}