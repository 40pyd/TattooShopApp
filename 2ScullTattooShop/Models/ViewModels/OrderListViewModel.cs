using BLL.Models;
using System.Collections.Generic;

namespace _2ScullTattooShop.Models.ViewModels
{
    public class OrderListViewModel
    {
        public IEnumerable<OrderDTO> Orders { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
