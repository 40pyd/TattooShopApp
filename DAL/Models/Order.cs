using System;

namespace DAL.Models
{
    public class Order
    {
        public Order()
        {
            CreatedOn = DateTimeOffset.Now;
        }

        public int OrderId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string CustomerId { get; set; }
        public int? AddressId { get; set; }
        public int TotalValue { get; set; }
        
        public virtual Address Address { get; set; }
    }
}
