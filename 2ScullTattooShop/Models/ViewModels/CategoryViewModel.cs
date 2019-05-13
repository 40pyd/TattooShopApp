using System.ComponentModel.DataAnnotations;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        public string Name { get; set; }
    }
}
