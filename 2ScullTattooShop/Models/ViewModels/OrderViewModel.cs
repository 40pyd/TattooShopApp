using System;
using System.ComponentModel.DataAnnotations;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class OrderViewModel
    {
        public int? OrderId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int? AddressId { get; set; }
        public int? CountryId { get; set; }

        [Required(ErrorMessage ="Enter the address")]
        public string AddressLine { get; set; }
        public string CountryName { get; set; }
        public int TotalValue { get; set; }
    }
}
