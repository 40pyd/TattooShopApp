using BLL.Models;
using System.Collections.Generic;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<ProductDTO> Products { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string CurrentCategory { get; set; }
        public string CurrentBrand { get; set; }
    }
}
