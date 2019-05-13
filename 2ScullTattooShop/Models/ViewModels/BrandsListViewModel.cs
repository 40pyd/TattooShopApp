using BLL.Models;
using System.Collections.Generic;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class BrandsListViewModel
    {
        public IEnumerable<BrandDTO> Brands { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
