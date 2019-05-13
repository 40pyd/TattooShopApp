using System.ComponentModel.DataAnnotations;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class CountryViewModel
    {
        public int? CountryId { get; set; }

        [Required(ErrorMessage = "Country name is required")]
        public string Name { get; set; }
    }
}
