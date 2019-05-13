using BLL.Models;
using System.Collections.Generic;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class AuthorsListViewModel
    {
        public IEnumerable<AuthorDTO> Authors { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
