using _2ScullTattooShop.Models.ViewModels;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _ScullTattooShop.Models.ViewModels
{
    public class CountryListViewModel
    {
        public IEnumerable<CountryDTO> Countries { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
