using Identity.EF;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace Identity.Services
{
    public static class ApplicationIdentityDatabaseFactory
    {
        public static void Seed(ApplicationDbContext context,
                         UserManager<ApplicationUser> userManager,
                         RoleManager<ApplicationRole> roleManager,
                         SignInManager<ApplicationUser> _signInManager)
        {

            //context.Database.EnsureCreated();

            if (context.Users.Any() || context.Roles.Any())
            {
                return;
            }

            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new ApplicationRole { Name = "Administrator" };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Manager").Result)
            {
                var role = new ApplicationRole { Name = "Manager" };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("User").Result)
            {
                var role = new ApplicationRole { Name = "User" };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (userManager.FindByNameAsync("40pyd").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "Andrey",
                    FirstName = "Andrey",
                    LastName = "40Pyd",
                    Birthday = new DateTime(1985, 09, 04),
                    Email = "40pyd40@gmail.com",
                    EmailConfirmed = true,
                    NormalizedEmail = ("40pyd40@gmail.com").ToUpper(),
                    NormalizedUserName = ("Andrey").ToUpper(),
                    PhoneNumberConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(user, "123qweASD@").Result;

                if (result.Succeeded)
                {
                    var code = userManager.GenerateEmailConfirmationTokenAsync(user);
                    _signInManager.SignInAsync(user, isPersistent: false);
                    userManager.AddToRoleAsync(user,"Administrator").Wait();
                }
            }

        }
    }
}
