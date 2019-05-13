using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using _2ScullTattooShop.Models;
using Microsoft.AspNetCore.Authorization;

namespace _2ScullTattooShop.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.IsAdmin = HttpContext.User.IsInRole("Administrator");
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Administrate()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult ApiInfo()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
