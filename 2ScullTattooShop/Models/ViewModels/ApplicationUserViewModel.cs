using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class ApplicationUserViewModel
{
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Category name is required")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Image")]
        public IFormFile Image { get; set; }
    }
}
