using BLL.Models;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace _2ScullTattooShop.Components
{
    public class AuthorViewComponent : ViewComponent
    {
        IService<AuthorDTO> service;

        public AuthorViewComponent(IService<AuthorDTO> _service)
        {
            service = _service;
        }

        public IViewComponentResult Invoke()
        {
            return View(service.GetAll()
                .Select(p => p.Name)
                .Distinct()
                .OrderBy(p => p));
        }
    }
}
