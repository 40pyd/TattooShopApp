using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class AuthorViewModel
    {
        public int? AuthorId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Sername { get; set; }
        public string NickName { get; set; }
        public int Age { get; set; }
        public IFormFile Image { get; set; }
    }
}
