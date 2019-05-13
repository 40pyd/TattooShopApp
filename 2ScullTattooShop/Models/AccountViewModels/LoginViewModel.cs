using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _2ScullTattooShop.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
