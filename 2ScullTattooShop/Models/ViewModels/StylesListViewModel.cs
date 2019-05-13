using BLL.Models;
using System.Collections.Generic;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class StylesListViewModel
    {
        public IEnumerable<StyleDTO> Styles { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
