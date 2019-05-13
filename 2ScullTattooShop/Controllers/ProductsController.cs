using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
    public class ProductsController : Controller
    {
        IService<ProductDTO> productService;
        IService<CategoryDTO> categoryService;
        IService<BrandDTO> brandService;

        public ProductsController(IService<ProductDTO> _productService,
                                  IService<CategoryDTO> _categoryService,
                                  IService<BrandDTO> _brandService)
        {
            productService = _productService;
            categoryService = _categoryService;
            brandService = _brandService;
        }

        public ActionResult List(string category,
                                 string brand,
                                 string searchString,
                                 int page = 1,
                                 int PageSize = 12,
                                 int sort=1)
        {
            var products = productService.GetAll();

            var predicate = PredicateBuilder.New<ProductDTO>(true);

            if (!string.IsNullOrEmpty(searchString))
            {
                predicate = predicate.And(x => x.Name.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(category))
            {
                predicate = predicate.And(i => i.CategoryName==category);
            }

            if (!string.IsNullOrEmpty(brand))
            {
                predicate = predicate.And(i => i.BrandName == brand);
            }

            IEnumerable<ProductDTO> selectedProducts;

            selectedProducts=products.Where(predicate).Select(i => i);
            var total = selectedProducts.Count();
            selectedProducts = sort > 1 ? selectedProducts.OrderByDescending(p => p.Price) : selectedProducts.OrderBy(p => p.Name);

            var productListModel = new ProductListViewModel
                {
                    Products = selectedProducts
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize),
                    PageViewModel = new PageViewModel
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = total
                    },
                    CurrentCategory = category,
                    CurrentBrand = brand
                };
            ViewBag.CurrentCategory = productListModel.CurrentCategory;
            ViewBag.CurrentBrand = productListModel.CurrentBrand;
            ViewBag.SortBy = sort;
            ViewBag.TotalCount = total;
            ViewBag.ShowingCount = total<PageSize?total:PageSize;
            ViewBag.IsAdmin = HttpContext.User.IsInRole("Administrator");

            return View(productListModel);
        }

        public ActionResult Details(int id)
        {
            var product = productService.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            var _product = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
                CategoryName = categoryService.GetAll().FirstOrDefault(x => x.CategoryId == product.CategoryId).Name,
                BrandId = product.BrandId,
                BrandName = brandService.GetAll().FirstOrDefault(x => x.BrandId == product.BrandId).Name
            };
            ViewBag.Image = product.Image;
            ViewBag.IsAdmin = HttpContext.User.IsInRole("Administrator");

            return View(_product);
        }

        public ActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(brandService.GetAll(), "BrandId", "Name");
            ViewData["CategoryId"] = new SelectList(categoryService.GetAll(), "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(ProductViewModel _product)
        {
            ProductDTO product = new ProductDTO();
            if (ModelState.IsValid)
            {
                if (productService.GetAll().Where(x => x.Name == _product.Name).Count() > 0)
                {
                    return RedirectToAction("Error", new { exeption = "The product already exist!" });
                }
                else
                {
                    product.Name = _product.Name;
                    product.Price = _product.Price;
                    product.Description = _product.Description;
                    product.CategoryId = _product.CategoryId;
                    product.BrandId = _product.BrandId;
                    product.Image = _product.Image != null ? ImageConverter.GetBytes(_product.Image) : null;
                    productService.Add(product);
                    productService.Save();
                    return RedirectToAction(nameof(List));
                }
            }
            ViewData["BrandId"] = new SelectList(brandService.GetAll(), "BrandId", "Name", _product.BrandId);
            ViewData["CategoryId"] = new SelectList(categoryService.GetAll(), "CategoryId", "Name", _product.CategoryId);
            return View(_product);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            var product = productService.Get(id);

            if (product == null)
            {
                return NotFound();
            }
            var _product = new ProductViewModel
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId
            };

            ViewBag.Image = product.Image;
            ViewData["Brands"] = new SelectList(brandService.GetAll(), "BrandId", "Name");
            ViewData["Categories"] = new SelectList(categoryService.GetAll(), "CategoryId", "Name");

            return View(_product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id, ProductViewModel _product)
        {
            var product = productService.Get(id);
            if (ModelState.IsValid)
            {
                try
                {
                    product.Name = _product.Name;
                    product.Price = _product.Price;
                    product.Description = _product.Description;
                    product.CategoryId = _product.CategoryId;
                    product.BrandId = _product.BrandId;
                    product.Image = _product.Image != null? ImageConverter.GetBytes(_product.Image):product.Image;
                    productService.Update(product);
                    productService.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                        return NotFound();
                }
                return RedirectToAction(nameof(List));
            }
            ViewData["BrandId"] = new SelectList(brandService.GetAll(), "BrandId", "BrandId", _product.BrandId);
            ViewData["CategoryId"] = new SelectList(categoryService.GetAll(), "CategoryId", "CategoryId", _product.CategoryId);
            return View(_product);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var product = productService.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            var _product = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryName = categoryService.GetAll().Where(x => x.CategoryId == product.CategoryId).FirstOrDefault().Name,
                BrandName = brandService.GetAll().Where(x => x.BrandId == product.BrandId).FirstOrDefault().Name
            };
            ViewBag.Image = product.Image;

            return View(_product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var product = productService.Get(id);
            productService.Delete(product);
            productService.Save();
            return RedirectToAction(nameof(List));
        }

        public IActionResult Error(string exeption)
        {
            return View((object)exeption);
        }
    }
}