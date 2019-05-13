using System.IO;
using System.Linq;
using _2ScullTattooShop.Helpers;
using _2ScullTattooShop.Models.ViewModels;
using BLL.Models;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _2ScullTattooShop.Controllers
{
    public class StylesController : Controller
    {
        IService<StyleDTO> styleService;
        IService<TattooDTO> tattooService;

        public int PageSize = 8;

        public StylesController(IService<StyleDTO> _styleService,
                                  IService<TattooDTO> _tattooService)
        {
            styleService = _styleService;
            tattooService = _tattooService;
        }

        public ActionResult List(int page = 1)
        {
            var styles = styleService.GetAll();
            ViewBag.IsAdmin = HttpContext.User.IsInRole("Administrator");

            return View(new StylesListViewModel
            {
                Styles = styles
                    .OrderBy(c => c.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PageViewModel = new PageViewModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = styles.Count()
                }
            });
        }

        public ActionResult Details(int id=1)
        {
            StyleDTO style=null;
            try
            {
                style = styleService.Get(id);
            }
            catch
            {
                return RedirectToAction("Error", new { exeption = "The style doesn't exist anymore!" });
            }
            if (style == null)
            {
                return NotFound();
            }
            var _style = new StyleViewModel
            {
                Id = style.Id,
                Name = style.Name,
                Description=style.Description
            };
            ViewBag.Image = style.Image;
            ViewBag.IsAdmin = HttpContext.User.IsInRole("Administrator");

            return View(_style);
        }
        
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(StyleViewModel _style)
        {
            StyleDTO style = new StyleDTO();
            if (ModelState.IsValid)
            {
                if (styleService.GetAll().Where(x => x.Name == _style.Name).Count() > 0)
                {
                    return RedirectToAction("Error", new { exeption = "The style already exist!" });
                }
                else
                {
                    style.Name = _style.Name;
                    style.Description = _style.Description;
                    style.Image = _style.Image != null ? ImageConverter.GetBytes(_style.Image) : null;
                    styleService.Add(style);
                    styleService.Save();
                    return RedirectToAction(nameof(List));
                }
            }
            return View(_style);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            var style = styleService.Get(id);

            if (style == null)
            {
                return NotFound();
            }

            var _style = new StyleViewModel
            {
                Name = style.Name,
                Description=style.Description
            };
            ViewBag.Image = style.Image;
            return View(_style);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id, StyleViewModel _style)
        {
            var style = styleService.Get(id);
            if (ModelState.IsValid)
            {
                try
                {
                    style.Name = _style.Name;
                    style.Description = _style.Description;
                    style.Image = _style.Image != null ? ImageConverter.GetBytes(_style.Image) : style.Image;
                    styleService.Update(style);
                    styleService.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(List));
            }
            return View(_style);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var style = styleService.Get(id);
            if (style == null)
            {
                return NotFound();
            }
            var _style = new StyleViewModel
            {
                Id = style.Id,
                Name = style.Name,
                Description=style.Description
            };

            ViewBag.Image = style.Image;
            return View(_style);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var style = styleService.Get(id);
            if (tattooService.GetAll().Where(x => x.StyleName == style.Name).Count() > 0)
                return RedirectToAction(nameof(Error));
            else
            {
                styleService.Delete(style);
                styleService.Save();
            }
            return RedirectToAction(nameof(List));
        }

        public IActionResult Error(string exeption)
        {
            return View((object)exeption);
        }
    }
}