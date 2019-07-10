using System;
using System.Linq;
using System.Security.Claims;
using _2ScullTattooShop.Models.BasketModels;
using _2ScullTattooShop.Models.ViewModels;
using _2ScullTattooShop.Services;
using BLL.Models;
using BLL.Services.Interfaces;
using Identity.EF;
using Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace _2ScullTattooShop.Controllers
{
    public class OrderController : Controller
    {
        ApplicationDbContext context;
        UserManager<ApplicationUser> userManager;
        SignInManager<ApplicationUser> signInManager;
        private IService<OrderDTO> orderService;
        private IService<AddressDTO> addressService;
        private IService<CountryDTO> countriesService;
        private Basket basket;
        private IConfiguration configuration;
        private IEmailSender emailSender;

        public int PageSize { get; set; } = 10;

        public OrderController(IService<OrderDTO> _orderService,
                               IService<AddressDTO> _addressService,
                               IService<CountryDTO> _countriesService,
                               Basket _basket,
                               ApplicationDbContext _context,
                               UserManager<ApplicationUser> _userManager,
                               SignInManager<ApplicationUser> _signInManager,
                               IConfiguration _configuration,
                               IEmailSender _emailSender)
        {
            orderService = _orderService;
            addressService = _addressService;
            countriesService = _countriesService;
            basket = _basket;
            context = _context;
            userManager = _userManager;
            signInManager = _signInManager;
            configuration = _configuration;
            emailSender = _emailSender;
        }

        public ActionResult List(int page = 1)
        {
            var orders = orderService.GetAll().ToList();
            for(int i=0; i< orders.Count(); i++)
            {
                try
                {
                    orders[i].CustomerName = userManager.Users.Where(u => u.Id == orders[i].CustomerId).FirstOrDefault().FirstName;
                }
                catch
                {
                    orders[i].CustomerName = "unknown";
                }
            }
            return View(new OrderListViewModel
            {
                Orders = orders
                    .OrderBy(c => c.OrderId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PageViewModel = new PageViewModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = orders.Count()
                }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var order = orderService.Get(id);
            orderService.Delete(order);
            orderService.Save();
            return RedirectToAction(nameof(List));
        }

        public IActionResult Checkout()
        {
            ViewData["Countries"] = new SelectList(countriesService.GetAll(), "CountryId", "Name");

            return View(new OrderViewModel());
        }

        public IActionResult Error(string exeption)
        {
            return View((object)exeption);
        }

        [HttpPost]
        public IActionResult Checkout(OrderViewModel _order)
        {
            if(basket.Items.Count()==0)
            {
                return RedirectToAction("Error", new { exeption = "There are no products in your basket" });
            }
            if(ModelState.IsValid)
            {
                OrderDTO order = new OrderDTO
                {
                    CustomerId = userManager.GetUserId(HttpContext.User),
                    CustomerName = this.User.FindFirstValue(ClaimTypes.Name),
                    AddressLine = _order.AddressLine,
                    CountryId = _order.CountryId,
                    TotalValue = (int)basket.ComputeTotalValue()
                };
                if (addressService.GetAll().Where(x => x.AddressLine == _order.AddressLine).Count() > 0)
                {
                    order.AddressId = addressService.GetAll()
                                      .FirstOrDefault(x => x.AddressLine == _order.AddressLine).AddressId;
                }
                else
                {
                    addressService.Add(new AddressDTO
                    {
                        AddressLine=order.AddressLine,
                        ContactName=order.CustomerName
                    });
                    order.AddressId = addressService.GetAll()
                                      .FirstOrDefault(x => x.AddressLine == _order.AddressLine).AddressId;
                }
                orderService.Add(order);
                orderService.Save();

                var email = userManager.Users.Where(u => u.Id == order.CustomerId).FirstOrDefault().Email;
                emailSender.SendEmailOrderConfirmationAsync(email,order);

                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(_order);
            }
        }

        public RedirectToActionResult Clear(string returnUrl)
        {
            basket.Clear();
            return RedirectToAction("Index", "Cart", new { returnUrl });
        }

        public ViewResult Completed()
        {
            basket.Clear();
            return View();
        }
    }
}