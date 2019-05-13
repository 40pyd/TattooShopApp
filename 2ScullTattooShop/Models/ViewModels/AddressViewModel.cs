using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class AddressViewModel
    {
        public int? AddressId { get; set; }
        [Required]
        [DisplayName("Ship to")]
        public string AddressLine { get; set; }
        [Required]
        public string ContactName { get; set; }
        [Required]
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        [Required]
        public string City { get; set; }
    }
}
