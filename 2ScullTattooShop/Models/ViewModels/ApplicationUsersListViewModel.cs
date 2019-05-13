using Identity.Models;
using System.Collections.Generic;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class ApplicationUsersListViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
