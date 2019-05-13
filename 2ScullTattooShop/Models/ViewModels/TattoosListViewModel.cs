using BLL.Models;
using System.Collections.Generic;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class TattoosListViewModel
    {
        public IEnumerable<TattooDTO> Tattoos { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string CurrentStyle { get; set; }
        public string CurrentAuthor { get; set; }
    }
}
