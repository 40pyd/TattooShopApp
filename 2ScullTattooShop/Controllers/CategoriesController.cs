using System.Linq;
using _2ScullTattooShop.Models.ViewModels;
using BLL.Models;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _2ScullTattooShop.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CategoriesController : Controller
    {
        IService<CategoryDTO> categoryService;
        IService<ProductDTO> productService;

        public int PageSize { get; set; } = 8;

        public CategoriesController(IService<CategoryDTO> _categoryService,
                                    IService<ProductDTO> _productService)
        {
            categoryService = _categoryService;
            productService = _productService;
        }

        public ActionResult List(int page = 1)
        {
            var categories = categoryService.GetAll();

            return View(new CategoriesListViewModel
            {
                Categories = categories
                    .OrderBy(c => c.CategoryId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PageViewModel = new PageViewModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = categories.Count()
                }
            });
        }

        public ActionResult Details(int id)
        {
            var category = categoryService.Get(id);
            if (category == null)
            {
                return NotFound();
            }
            var _category = new CategoryViewModel
            {
                Id = category.CategoryId,
                Name = category.Name
            };
            return View(_category);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryViewModel _category)
        {
            CategoryDTO category = new CategoryDTO();
            if (ModelState.IsValid)
            {
                if (categoryService.GetAll().Where(x => x.Name == _category.Name).Count() > 0)
                {
                    return RedirectToAction("Error", new { exeption = "The category already exist!" });
                }
                else
                {
                    category.Name = _category.Name;
                    categoryService.Add(category);
                    categoryService.Save();
                    return RedirectToAction(nameof(List));
                }
            }
            return View(_category);
        }

        public ActionResult Edit(int id)
        {
            var category = categoryService.Get(id);

            if (category == null)
            {
                return NotFound();
            }

            var _category = new CategoryViewModel
            {
                Name = category.Name
            };
            
            return View(_category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategoryViewModel _category)
        {
            var category = categoryService.Get(id);
                if (ModelState.IsValid)
                {
                    try
                    {
                        category.Name = _category.Name;
                        categoryService.Update(category);
                        categoryService.Save();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return NotFound();
                    }
                    return RedirectToAction(nameof(List));
                }
            return View(_category);
        }

        public ActionResult Delete(int id)
        {
            var category = categoryService.Get(id);
            if (category == null)
            {
                return NotFound();
            }
            var _category = new CategoryViewModel
            {
                Id = category.CategoryId,
                Name = category.Name
            };

            return View(_category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var category = categoryService.Get(id);
            if (productService.GetAll().Where(x => x.CategoryName == category.Name).Count() > 0)
            {
                return RedirectToAction("Error", new { exeption = "There are products in category you are trying to delete" });
            }
            else
            {
                categoryService.Delete(category);
                categoryService.Save();
            }
            return RedirectToAction(nameof(List));
        }

        public IActionResult Error(string exeption)
        {
            return View((object)exeption);
        }
    }
}