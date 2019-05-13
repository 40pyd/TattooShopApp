using System.Linq;
using _2ScullTattooShop.Helpers;
using _2ScullTattooShop.Models.ViewModels;
using Identity.EF;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _2ScullTattooShop.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        ApplicationIdentityContext context;
        UserManager<ApplicationUser> userManager;
        SignInManager<ApplicationUser> signInManager;

        public int PageSize = 6;

        public UsersController(ApplicationIdentityContext _context,
                               UserManager<ApplicationUser> _userManager,
                               SignInManager<ApplicationUser> _signInManager)
        {
            context = _context;
            userManager = _userManager;
            signInManager = _signInManager;
        }

        public ActionResult List(int page = 1)
        {
            var users = userManager.Users.ToList();
            ViewBag.IsAdmin = HttpContext.User.IsInRole("Administrator");

            return View(new ApplicationUsersListViewModel
            {
                Users = users
                    .OrderBy(c => c.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PageViewModel = new PageViewModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = users.Count()
                }
            });
        }

        public ActionResult Details(string id)
        {
            var user = userManager.Users.Where(u => u.Id == id).FirstOrDefault(); ;
            if (user == null)
            {
                return NotFound();
            }
            var _user = new ApplicationUserViewModel
            {
                Id=user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday,
                Email = user.Email
            };
            ViewBag.Image = user.Image;
            ViewBag.IsAdmin = HttpContext.User.IsInRole("Administrator");

            return View(_user);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ApplicationUserViewModel _user)
        {
            ApplicationUser user = new ApplicationUser();
            if (ModelState.IsValid)
            {
                user.UserName = _user.FirstName;
                user.FirstName = _user.FirstName;
                user.LastName = _user.LastName;
                user.Birthday = _user.Birthday;
                user.Email = _user.Email;
                user.EmailConfirmed = true;
                user.NormalizedEmail = _user.Email.ToUpper();
                user.NormalizedUserName = _user.FirstName.ToUpper();
                user.PhoneNumberConfirmed = true;
                user.Image = _user.Image != null ? ImageConverter.GetBytes(_user.Image) : null;
                IdentityResult result = userManager.CreateAsync(user, "123qweASD@").Result;
                if (result.Succeeded)
                {
                    var code = userManager.GenerateEmailConfirmationTokenAsync(user);
                    signInManager.SignInAsync(user, isPersistent: false);
                    userManager.AddToRoleAsync(user,"User").Wait();
                }
                return RedirectToAction(nameof(List));
            }
            return View(_user);
        }

        public ActionResult Edit(string id)
        {
            var user = userManager.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            var _user = new ApplicationUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday,
                Email = user.Email
            };
            ViewBag.Image = user.Image;

            return View(_user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUserViewModel _user)
        {
            var user = userManager.Users.Where(u => u.Id == _user.Id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                try
                {
                    user.UserName = _user.FirstName;
                    user.FirstName = _user.FirstName;
                    user.LastName = _user.LastName;
                    user.Birthday = _user.Birthday;
                    user.Email = _user.Email;
                    user.EmailConfirmed = true;
                    user.NormalizedEmail = _user.Email.ToUpper();
                    user.NormalizedUserName = _user.FirstName.ToUpper();
                    user.PhoneNumberConfirmed = true;
                    user.Image = _user.Image != null ? ImageConverter.GetBytes(_user.Image) : user.Image;
                    userManager.UpdateAsync(user).Wait();
                    signInManager.SignInAsync(user, isPersistent: true).Wait();
                    context.SaveChangesAsync().Wait();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction("List");
            }
            return View(_user);
        }

        public ActionResult Delete(string id)
        {
            var user = userManager.Users.Where(u => u.Id == id).FirstOrDefault();
            var _user = new ApplicationUserViewModel
            {
                Id=user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday,
                Email = user.Email
            };
            ViewBag.Image = user.Image;
            return View(_user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ApplicationUserViewModel _user)
        {
            var user = userManager.Users.Where(u => u.Id == _user.Id).FirstOrDefault();
            userManager.DeleteAsync(user).Wait();
            context.SaveChangesAsync().Wait();
            return RedirectToAction("List");
        }
    }
}