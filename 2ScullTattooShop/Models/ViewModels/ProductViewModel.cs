using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class ProductViewModel
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Category of product is required")]
        public int? CategoryId { get; set; }
        [Required]
        public int? BrandId { get; set; }
        [DisplayName("Category")]
        public string CategoryName { get; set; }
        [DisplayName("Brand")]
        public string BrandName { get; set; }
        public IFormFile Image { get; set; }
    }
}
