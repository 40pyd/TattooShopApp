using BLL.Models;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace _2ScullTattooShop.Components
{
    public class CategoryViewComponent : ViewComponent
    {
        IService<ProductDTO> service;

        public CategoryViewComponent(IService<ProductDTO> _service)
        {
            service = _service;
        }

        public IViewComponentResult Invoke()
        {
            return View(service.GetAll()
                .Select(p => p.CategoryName)
                .Distinct()
                .OrderBy(p => p));
        }
    }
}
