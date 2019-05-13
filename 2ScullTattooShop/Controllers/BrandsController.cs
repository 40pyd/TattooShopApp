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
    [Authorize(Roles = "Administrator")]
    public class BrandsController : Controller
    {
        IService<BrandDTO> brandService;
        IService<ProductDTO> productService;
        public int PageSize = 8;

        public BrandsController(IService<BrandDTO> _brandService,
                                IService<ProductDTO> _productService)
        {
            brandService = _brandService;
            productService = _productService;
        }

        public ActionResult List(int page = 1)
        {
            var brands = brandService.GetAll();

            return View(new BrandsListViewModel
            {
                Brands = brands
                    .OrderBy(c => c.BrandId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PageViewModel = new PageViewModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = brands.Count()
                }
            });
        }

        public ActionResult Details(int id)
        {
            var brand = brandService.Get(id);
            if (brand == null)
            {
                return NotFound();
            }
            var _brand = new BrandViewModel
            {
                Id = brand.BrandId,
                Name = brand.Name
            };
            ViewBag.Image = brand.Image;

            return View(_brand);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BrandViewModel _brand)
        {
            BrandDTO brand = new BrandDTO();
            if (ModelState.IsValid)
            {
                if (brandService.GetAll().Where(x => x.Name == _brand.Name).Count() > 0)
                {
                    return RedirectToAction("Error", new { exeption = "The brand already exist!" });
                }
                else
                {
                    brand.Name = _brand.Name;
                    brand.Image = _brand.Image != null ? ImageConverter.GetBytes(_brand.Image) : null;
                    brandService.Add(brand);
                    brandService.Save();
                    return RedirectToAction(nameof(List));
                }
            }
            return View(_brand);
        }

        public ActionResult Edit(int id)
        {
            var brand = brandService.Get(id);

            if (brand == null)
            {
                return NotFound();
            }

            var _brand = new BrandViewModel
            {
                Name = brand.Name
            };
            ViewBag.Image = brand.Image;
            return View(_brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BrandViewModel _brand)
        {
            var brand = brandService.Get(id);
                if (ModelState.IsValid)
                {
                    try
                    {
                        brand.Name = _brand.Name;
                        brand.Image = _brand.Image != null ? ImageConverter.GetBytes(_brand.Image) : brand.Image;
                        brandService.Update(brand);
                        brandService.Save();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return NotFound();
                    }
                    return RedirectToAction(nameof(List));
            }
            return View(_brand);
        }

        public ActionResult Delete(int id)
        {
            var brand = brandService.Get(id);
            if (brand == null)
            {
                return NotFound();
            }
            var _brand = new BrandViewModel
            {
                Id = brand.BrandId,
                Name = brand.Name
            };

            ViewBag.Image = brand.Image;
            return View(_brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var brand = brandService.Get(id);
            if (productService.GetAll().Where(x => x.BrandName == brand.Name).Count() > 0)
            {
                return RedirectToAction("Error", new { exeption = "There are products of the brand you are trying to delete" });
            }
            else
            {
                brandService.Delete(brand);
                brandService.Save();
            }
            return RedirectToAction(nameof(List));
        }

        public IActionResult Error(string exeption)
        {
            return View((object)exeption);
        }
    }
}