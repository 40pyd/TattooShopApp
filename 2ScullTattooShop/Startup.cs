using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using _2ScullTattooShop.Services;
using Identity.EF;
using Identity.Models;
using DAL.Models;
using DAL.Repositories.Interfaces;
using DAL.Repositories.Concrete;
using DAL.Repositories;
using Identity.Services;
using DAL.EF;
using BLL.Services.Interfaces;
using BLL.Services.Concrete;
using BLL.Models;
using _2ScullTattooShop.Models.BasketModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace _2ScullTattooShop
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DAL.EF.ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DataConnection"),b => b.MigrationsAssembly("DAL")));
            services.AddDbContext<Identity.EF.ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DataConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<Identity.EF.ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                AddCookie(options => options.SlidingExpiration = true).
                AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<IService<ProductDTO>, ProductService>();
            services.AddScoped<IService<CategoryDTO>, CategoryService>();
            services.AddScoped<IService<BrandDTO>, BrandService>();
            services.AddScoped<IService<StyleDTO>, StyleService>();
            services.AddScoped<IService<AuthorDTO>, AuthorService>();
            services.AddScoped<IService<TattooDTO>, TattooService>();
            services.AddScoped<IService<CountryDTO>, CountryService>();
            services.AddScoped<IService<AddressDTO>, AddressService>();
            services.AddScoped<IService<OrderDTO>, OrderService>();

            services.AddTransient<IGenericRepository<Brand>, BrandRepository>();
            services.AddTransient<IGenericRepository<Product>, ProductRepository>();
            services.AddTransient<IGenericRepository<Author>, AuthorRepository>();
            services.AddTransient<IGenericRepository<Style>, StyleRepository>();
            services.AddTransient<IGenericRepository<Category>, CategoryRepository>();
            services.AddTransient<IGenericRepository<Tattoo>, TattooRepository>();
            services.AddTransient<IGenericRepository<Country>, CountryRepository>();
            services.AddTransient<IGenericRepository<Address>, AddressRepository>();
            services.AddTransient<IGenericRepository<Order>, OrderRepository>();

            services.AddScoped(sp => SessionBasket.GetBasket(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app
                             , IHostingEnvironment env
                             , IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseStaticFiles();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<Identity.EF.ApplicationDbContext>();
            var signManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();

            // Use to create entity database and seed data
            //ApplicationIdentityDatabaseFactory.Seed(context, userManager, roleManager, signManager);
            
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new { Controller = "Home", action = "Index" });

                routes.MapRoute(
                    name: null,
                    template: "{controller}/{action}/{id?}");
            });
        }
    }
}
