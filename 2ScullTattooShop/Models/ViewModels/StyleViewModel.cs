using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class StyleViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Style name is required")]
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
