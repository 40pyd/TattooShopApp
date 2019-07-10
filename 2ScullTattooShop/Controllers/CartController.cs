using System.Linq;
using _2ScullTattooShop.Models.BasketModels;
using _2ScullTattooShop.Models.ViewModels;
using BLL.Models;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _2ScullTattooShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        IService<ProductDTO> productService;
        IService<CategoryDTO> categoryService;
        IService<BrandDTO> brandService;
        IService<OrderDTO> orderService;
        IService<CountryDTO> countryService;
        IService<AddressDTO> addressService;

        private Basket basket;

        public CartController(IService<ProductDTO> _productService
                              ,IService<CategoryDTO> _categoryService
                              ,IService<BrandDTO> _brandService 
                              ,IService<OrderDTO> _orderService
                              , IService<CountryDTO> _countryService
                              , IService<AddressDTO> _addressService
                              , Basket _basket)
        {
            productService = _productService;
            categoryService = _categoryService;
            brandService = _brandService;
            orderService = _orderService;
            countryService = _countryService;
            addressService = _addressService;
            basket = _basket;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new BasketViewModel
            {
                Basket = basket,
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public RedirectToActionResult AddToBasket(int itemId,string returnUrl)
        {
            var product = productService.Get(itemId);
            if (product!=null)
            {
                var _product = new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    CategoryName = categoryService.GetAll()
                                  .FirstOrDefault(x => x.CategoryId == product.CategoryId).Name,
                    BrandId = product.BrandId,
                    BrandName = brandService.GetAll()
                                .FirstOrDefault(x => x.BrandId == product.BrandId).Name
                };
                basket.AddItem(_product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        
        public RedirectToActionResult RemoveFromBasket(int id, string returnUrl=null)
        {
            var product = productService.Get(id);
            
            if (product != null)
            {
                var _product = new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    CategoryName = categoryService.GetAll()
                                   .FirstOrDefault(x => x.CategoryId == product.CategoryId).Name,
                    BrandId = product.BrandId,
                    BrandName = brandService.GetAll()
                                   .FirstOrDefault(x => x.BrandId == product.BrandId).Name
                };
                basket.RemoveItem(_product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public ActionResult CreateCountry()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCountry(CountryViewModel _country)
        {
            CountryDTO country = new CountryDTO();
            if (ModelState.IsValid)
            {
                if (countryService.GetAll().Where(x => x.Name == _country.Name).Count()>0)
                {
                    return RedirectToAction("Error", new { exeption = "The country already exists" });
                }
                else
                {
                    country.Name = _country.Name;
                    countryService.Add(country);
                    countryService.Save();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(_country);
        }

        public ActionResult DeleteCountry(int id)
        {
            var country = countryService.Get(id);
            if (country == null)
            {
                return RedirectToAction("Error", new { exeption = "The country is not found" });
            }
            var _country = new CountryViewModel
            {
                CountryId = country.CountryId,
                Name = country.Name
            };
            return View(_country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCountry(int id, IFormCollection collection)
        {
            var country = countryService.Get(id);
            if (addressService.GetAll().Where(x => x.CountryName == country.Name).Count() > 0)
            {
                return RedirectToAction("Error", new { exeption = "There are addresses saved for the country you are trying to delete" });
            }
            else
            {
                countryService.Delete(country);
                countryService.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult CreateAddress()
        {
            ViewData["CountryId"] = new SelectList(countryService.GetAll(), "CountryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAddress(AddressViewModel _address)
        {
            AddressDTO address = new AddressDTO();
            if (ModelState.IsValid)
            {
                if (addressService.GetAll().Where(x => x.AddressLine == _address.AddressLine).Count() > 0)
                {
                    return RedirectToAction("Error", new { exeption = "The address already exists" });
                }
                else
                {
                    address.AddressLine = _address.AddressLine;
                    address.ContactName = _address.ContactName;
                    address.CountryId = _address.CountryId;
                    address.City = _address.City;
                    addressService.Add(address);
                    addressService.Save();
                    return RedirectToAction(nameof(CreateAddress));
                }
            }
            return View(_address);
        }

        public ActionResult DeleteAddress(int id)
        {
            var address = addressService.Get(id);
            if (address == null)
            {
                return RedirectToAction("Error", new { exeption = "There are orders with address you are trying to delete" });

            }
            var _address = new AddressViewModel
            {
                AddressId = address.AddressId,
                AddressLine = address.AddressLine,
                ContactName=address.ContactName,
                City=address.City,
                CountryName=address.ContactName
            };

            return View(_address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAddress(int id, IFormCollection collection)
        {
            var address = addressService.Get(id);
            if (orderService.GetAll().Where(x => x.AddressLine == address.AddressLine).Count() > 0)
            {
                return RedirectToAction("Error",new { exeption = "There are orders with address you are trying to delete" });
            }
            else
            {
                addressService.Delete(address);
                addressService.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error(string exeption)
        {
            return View((object)exeption);
        }
    }
}