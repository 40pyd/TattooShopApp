using BLL.Models;
using System.Collections.Generic;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class CategoriesListViewModel
    {
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
