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
    public class AuthorsController : Controller
    {
        IService<AuthorDTO> authorsService;
        IService<TattooDTO> tattooService;

        public int PageSize = 6;

        public AuthorsController(IService<AuthorDTO> _authorsService,
                                 IService<TattooDTO> _tattooService)
        {
            authorsService = _authorsService;
            tattooService = _tattooService;
        }

        public ActionResult List(int page = 1)
        {
            var authors = authorsService.GetAll();
            ViewBag.IsAdmin = HttpContext.User.IsInRole("Administrator");

            return View(new AuthorsListViewModel
            {
                Authors = authors
                    .OrderBy(c => c.AuthorId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PageViewModel = new PageViewModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = authors.Count()
                }
            });
        }

        public ActionResult Details(int id)
        {
            AuthorDTO author = null;
            try
            {
                author = authorsService.Get(id);
            }
            catch
            {
                return RedirectToAction("Error", new { exeption = "The author doesn't exist in database anymore!" });
            }
            if (author == null)
            {
                return NotFound();
            }
            var _author = new AuthorViewModel
            {
                AuthorId = author.AuthorId,
                Name = author.Name,
                Sername=author.Sername,
                Age=author.Age,
                NickName=author.NickName
            };

            ViewBag.Image = author.Photo;
            ViewBag.IsAdmin = HttpContext.User.IsInRole("Administrator");

            return View(_author);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(AuthorViewModel _author)
        {
            AuthorDTO author = new AuthorDTO();
            if (ModelState.IsValid)
            {
                if (authorsService.GetAll().Where(x => x.NickName == _author.NickName).Count() > 0)
                {
                    return RedirectToAction("Error", new { exeption = "The author already exist!" });
                }
                else
                {
                    author.Name = _author.Name;
                    author.Sername = _author.Sername;
                    author.NickName = _author.NickName;
                    author.Age = _author.Age;
                    author.Photo = _author.Image != null ? ImageConverter.GetBytes(_author.Image) : null;
                    authorsService.Add(author);
                    authorsService.Save();
                    return RedirectToAction(nameof(List));
                }
            }
            return View(_author);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            var author = authorsService.Get(id);

            if (author == null)
            {
                return NotFound();
            }

            var _author = new AuthorViewModel
            {
                Name = author.Name,
                Sername=author.Sername,
                NickName=author.NickName,
                Age=author.Age
            };

            ViewBag.Image = author.Photo;
            return View(_author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id, AuthorViewModel _author)
        {
            var author = authorsService.Get(id);
            if (ModelState.IsValid)
            {
                try
                {
                    author.Name = _author.Name;
                    author.Sername = _author.Sername;
                    author.NickName = _author.NickName;
                    author.Age = _author.Age;
                    author.Photo = _author.Image != null ? ImageConverter.GetBytes(_author.Image) : author.Photo;
                    authorsService.Update(author);
                    authorsService.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(List));
            }
            return View(_author);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var author = authorsService.Get(id);
            if (author == null)
            {
                return NotFound();
            }
            var _author = new AuthorViewModel
            {
                Name = author.Name,
                Sername = author.Sername,
                NickName = author.NickName,
                Age = author.Age
            };
            ViewBag.Image = author.Photo;
            return View(_author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var author = authorsService.Get(id);
            if (tattooService.GetAll().Where(x => x.AuthorName == author.Name).Count() > 0)
            {
                return RedirectToAction("Error", new { exeption = "There are tattoos of the author you are trying to delete" });
            }
            else
            {
                authorsService.Delete(author);
                authorsService.Save();
            }
            return RedirectToAction(nameof(List));
        }

        public IActionResult Error(string exeption)
        {
            return View((object)exeption);
        }
    }
}