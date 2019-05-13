using System.Linq;
using System.Security.Claims;
using _2ScullTattooShop.Models.BasketModels;
using _2ScullTattooShop.Models.ViewModels;
using _2ScullTattooShop.Services;
using BLL.Models;
using BLL.Services.Interfaces;
using Identity.EF;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace _2ScullTattooShop.Controllers
{
    public class OrderController : Controller
    {
        ApplicationIdentityContext context;
        UserManager<ApplicationUser> userManager;
        SignInManager<ApplicationUser> signInManager;
        private IService<OrderDTO> orderService;
        private IService<AddressDTO> addressService;
        private Basket basket;
        private IConfiguration configuration;
        private IEmailSender emailSender;

        public OrderController(IService<OrderDTO> _orderService,
                               IService<AddressDTO> _addressService,
                               Basket _basket,
                               ApplicationIdentityContext _context,
                               UserManager<ApplicationUser> _userManager,
                               SignInManager<ApplicationUser> _signInManager,
                               IConfiguration _configuration,
                               IEmailSender _emailSender)
        {
            orderService = _orderService;
            addressService = _addressService;
            basket = _basket;
            context = _context;
            userManager = _userManager;
            signInManager = _signInManager;
            configuration = _configuration;
            emailSender = _emailSender;
        }

        public IActionResult Checkout()
        {
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