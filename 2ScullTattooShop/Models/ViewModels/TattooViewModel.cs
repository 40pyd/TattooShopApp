using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class TattooViewModel
{
        public int? TattooId { get; set; }
        [Required(ErrorMessage = "Tattoo name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter the style of tattoo")]
        public int? StyleId { get; set; }
        public string StyleName { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Enter the author of the tattoo")]
        public int? AuthorId { get; set; }
        public string AuthorName { get; set; }
        public IFormFile Image { get; set; }
    }
}
