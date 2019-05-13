using BLL.Models;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace _2ScullTattooShop.Components
{
    public class StyleViewComponent : ViewComponent
    {
        IService<TattooDTO> service;

        public StyleViewComponent(IService<TattooDTO> _service)
        {
            service = _service;
        }

        public IViewComponentResult Invoke()
        {
            return View(service.GetAll()
                .Select(p => p.StyleName)
                .Distinct()
                .OrderBy(p => p));
        }
    }
}
