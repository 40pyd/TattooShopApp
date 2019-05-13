using System.Collections.Generic;
using System.Linq;
using _2ScullTattooShop.Helpers;
using _2ScullTattooShop.Models.ViewModels;
using BLL.Models;
using BLL.Services.Interfaces;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace _2ScullTattooShop.Controllers
{
    public class TattoosController : Controller
    {
        IService<TattooDTO> tattooService;
        IService<StyleDTO> styleService;
        IService<AuthorDTO> authorService;

        public int PageSize = 6;

        public TattoosController(IService<TattooDTO> _tattooService,
                                 IService<StyleDTO> _styleService,
                                 IService<AuthorDTO> _authorService)
        {
            tattooService = _tattooService;
            styleService = _styleService;
            authorService = _authorService;
        }

        public ActionResult List(string style
                                ,string author
                                ,string searchString
                                ,int page = 1
                                ,int PageSize = 12
                                ,int sort = 1)
        {
            var tattoos = tattooService.GetAll();

            var predicate = PredicateBuilder.New<TattooDTO>(true);

            if (!string.IsNullOrEmpty(searchString))
            {
                predicate = predicate.And(x => x.Name.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(style))
            {
                predicate = predicate.And(i => i.StyleName == style);
            }

            if (!string.IsNullOrEmpty(author))
            {
                predicate = predicate.And(i => i.AuthorName == author);
            }

            IEnumerable<TattooDTO> selectedTattoos;

            selectedTattoos = tattoos.Where(predicate).Select(i => i);
            var total = selectedTattoos.Count();

            selectedTattoos = sort > 1 ? selectedTattoos.OrderByDescending(p => p.Price) : selectedTattoos.OrderBy(p => p.Name);
            
            var tattooListModel = new TattoosListViewModel
            {
                Tattoos = selectedTattoos
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PageViewModel = new PageViewModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = total
                },
                CurrentStyle = style,
                CurrentAuthor = author
            };
            ViewBag.CurrentAuthor = tattooListModel.CurrentAuthor;
            ViewBag.CurrentStyle = tattooListModel.CurrentStyle;
            ViewBag.TotalCount = total;
            ViewBag.SortBy = sort;
            ViewBag.ShowingCount = total < PageSize ? total : PageSize;
            ViewBag.IsAdmin = HttpContext.User.IsInRole("Administrator");

            return View(tattooListModel);
        }

        public ActionResult Details(int id)
        {
            var tattoo = tattooService.Get(id);
            if (tattoo == null)
            {
                return NotFound();
            }
            var _tattoo = new TattooViewModel
            {
                TattooId = tattoo.TattoId,
                Name = tattoo.Name,
                Price = tattoo.Price,
                StyleId = tattoo.StyleId,
                StyleName = styleService.GetAll().FirstOrDefault(x => x.Id == tattoo.StyleId).Name,
                AuthorId = tattoo.AuthorId,
                AuthorName = authorService.GetAll().FirstOrDefault(x => x.AuthorId == tattoo.AuthorId).Name
            };
            ViewBag.Image = tattoo.Image;
            ViewBag.IsAdmin = HttpContext.User.IsInRole("Administrator");

            return View(_tattoo);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewData["StyleId"] = new SelectList(styleService.GetAll(), "Id", "Name");
            ViewData["AuthorId"] = new SelectList(authorService.GetAll(), "AuthorId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(TattooViewModel _tattoo)
        {
            TattooDTO tattoo = new TattooDTO();
            if (ModelState.IsValid)
            {
                if (tattooService.GetAll().Where(x => x.Name == _tattoo.Name).Count() > 0)
                {
                    return RedirectToAction("Error", new { exeption = "The tattoo already exist!" });
                }
                else
                {
                    tattoo.Name = _tattoo.Name;
                    tattoo.Price = _tattoo.Price;
                    tattoo.StyleName = _tattoo.StyleName;
                    tattoo.StyleId = _tattoo.StyleId;
                    tattoo.AuthorName = authorService.GetAll().FirstOrDefault(x => x.AuthorId == _tattoo.AuthorId).Name;
                    tattoo.AuthorId = _tattoo.AuthorId;
                    tattoo.Image = _tattoo.Image != null ? ImageConverter.GetBytes(_tattoo.Image) : null;
                    tattooService.Add(tattoo);
                    tattooService.Save();
                    return RedirectToAction(nameof(List));
                }
            }
            ViewData["StyleId"] = new SelectList(styleService.GetAll(), "Id", "Name", _tattoo.StyleId);
            ViewData["AuthorId"] = new SelectList(authorService.GetAll(), "AuthorId", "Name", _tattoo.AuthorId);
            return View(_tattoo);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            var tattoo = tattooService.Get(id);

            if (tattoo == null)
            {
                return NotFound();
            }

            var _tattoo = new TattooViewModel()
            {
                Name = tattoo.Name,
                Price = tattoo.Price,
                StyleName = tattoo.StyleName,
                StyleId = tattoo.StyleId,
                AuthorName = tattoo.AuthorName,
                AuthorId = tattoo.AuthorId
            };

            ViewBag.Image = tattoo.Image;
            ViewData["Styles"] = new SelectList(styleService.GetAll(), "Id", "Name");
            ViewData["Authors"] = new SelectList(authorService.GetAll(), "AuthorId", "Name");
            return View(_tattoo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id, TattooViewModel _tattoo)
        {
            var tattoo = tattooService.Get(id);
            if (ModelState.IsValid)
            {
                try
                {
                    tattoo.Name = _tattoo.Name;
                    tattoo.Price = _tattoo.Price;
                    tattoo.StyleName = _tattoo.StyleName;
                    tattoo.StyleId = _tattoo.StyleId;
                    tattoo.AuthorName = _tattoo.AuthorName;
                    tattoo.AuthorId = _tattoo.AuthorId;
                    tattoo.Image = _tattoo.Image != null ? ImageConverter.GetBytes(_tattoo.Image) : tattoo.Image;
                    tattooService.Update(tattoo);
                    tattooService.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(List));
            }
            ViewData["StyleId"] = new SelectList(styleService.GetAll(), "Id", "Id", _tattoo.StyleId);
            ViewData["AuthorId"] = new SelectList(authorService.GetAll(), "Id", "Id", _tattoo.AuthorId);
            return View(_tattoo);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var tattoo = tattooService.Get(id);
            if (tattoo == null)
            {
                return NotFound();
            }
            var _tattoo = new TattooViewModel
            {
                Name = tattoo.Name,
                Price = tattoo.Price,
                StyleName = styleService.GetAll().FirstOrDefault(x => x.Id == tattoo.StyleId).Name,
                StyleId = tattoo.StyleId,
                AuthorName = authorService.GetAll().FirstOrDefault(x => x.AuthorId == tattoo.AuthorId).Name,
                AuthorId = tattoo.AuthorId
            };
            ViewBag.Image = tattoo.Image;

            return View(_tattoo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var tattoo = tattooService.Get(id);
            tattooService.Delete(tattoo);
            tattooService.Save();
            return RedirectToAction(nameof(List));
        }

        public IActionResult Error(string exeption)
        {
            return View((object)exeption);
        }
    }
}