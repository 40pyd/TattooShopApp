using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.EF;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Concrete;
using DAL.Repositories.Interfaces;
using Identity.EF;
using Microsoft.AspNetCore.Mvc;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _2ScullTattoo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DataConnection")));
            services.AddDbContext<ApplicationIdentityContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("IdentityDataConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationIdentityContext>()
                .AddDefaultTokenProviders();
            

            services.AddScoped<DbContext, ApplicationDbContext>();

            services.AddTransient<IGenericRepository<Brand>, BrandRepository>();
            services.AddTransient<IGenericRepository<Product>, ProductRepository>();
            services.AddTransient<IGenericRepository<Author>, AuthorRepository>();
            services.AddTransient<IGenericRepository<Style>, StyleRepository>();
            services.AddTransient<IGenericRepository<Category>, CategoryRepository>();
            services.AddTransient<IGenericRepository<DAL.Models.File>, FileRepository>();
            services.AddTransient<IGenericRepository<Tattoo>, TattooRepository>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app
                             ,IHostingEnvironment env
                             ,IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationIdentityContext>();
            ApplicationIdentityDatabaseFactory.Seed(context, userManager, roleManager);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void CreateRoles(IServiceProvider serviceProvider)
        {

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            Task<IdentityResult> roleResult;
            string email = "admin@ukr.net";

            //Check that there is an Administrator role and create if not
            Task<bool> hasAdminRole = roleManager.RoleExistsAsync("Admin");
            hasAdminRole.Wait();

            if (!hasAdminRole.Result)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole("Admin"));
                roleResult.Wait();
            }

            //Check if the admin user exists and create it if not
            //Add to the Administrator role

            Task<ApplicationUser> testUser = userManager.FindByEmailAsync(email);

            testUser.Wait();

            if (testUser.Result == null)
            {
                ApplicationUser administrator = new ApplicationUser
                {
                    Email = email,
                    UserName = email
                };

                Task<IdentityResult> newUser = userManager.CreateAsync(administrator, "12345qweASD@");
                newUser.Wait();

                if (newUser.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(administrator, "Admin");
                    newUserRole.Wait();
                }
            }

        }
    }
}
