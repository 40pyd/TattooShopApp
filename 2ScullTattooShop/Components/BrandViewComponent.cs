using BLL.Models;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace _2ScullTattooShop.Components
{
    public class BrandViewComponent : ViewComponent
    {
        IService<ProductDTO> service;

        public BrandViewComponent(IService<ProductDTO> _service)
        {
            service = _service;
        }

        public IViewComponentResult Invoke()
        {
            return View(service.GetAll()
                .Select(p => p.BrandName)
                .Distinct()
                .OrderBy(p => p));
        }
    }
}
