using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2ScullTattooShop.Models.ViewModels;
using _ScullTattooShop.Models.ViewModels;
using BLL.Models;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _2ScullTattooShop.Controllers
{
    public class CountriesController : Controller
    {
        IService<CountryDTO> countriesService;

        public CountriesController(IService<CountryDTO> _countriesService)
        {
            countriesService = _countriesService;
        }

        public ActionResult List(int page = 1,
                                 int PageSize = 12)
        {
            var countries = countriesService.GetAll();

            var countriesListModel = new CountryListViewModel
            {
                Countries = countries
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize),
                PageViewModel = new PageViewModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = countries.Count()
                }
            };

            ViewBag.IsAdmin = HttpContext.User.IsInRole("Administrator");

            return View(countriesListModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(CountryViewModel _country)
        {
            CountryDTO country = new CountryDTO();
            if (ModelState.IsValid)
            {
                if (countriesService.GetAll().Where(x => x.Name == _country.Name).Count() > 0)
                {
                    return RedirectToAction("Error", new { exeption = "The country already exist!" });
                }
                else
                {
                    country.Name = _country.Name;
                    country.ShortName = _country.ShortName;
                    countriesService.Add(country);
                    countriesService.Save();
                    return RedirectToAction(nameof(List));
                }
            }

            return View(_country);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            var country = countriesService.Get(id);

            if (country == null)
            {
                return NotFound();
            }
            var _country = new CountryViewModel
            {
                Name = country.Name,
                ShortName = country.ShortName
            };

            return View(_country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id, CountryViewModel _country)
        {
            var country = countriesService.Get(id);
            if (ModelState.IsValid)
            {
                try
                {
                    country.Name = _country.Name;
                    country.ShortName = _country.ShortName;
                    countriesService.Update(country);
                    countriesService.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(List));
            }

            return View(_country);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var country = countriesService.Get(id);
            if (country == null)
            {
                return NotFound();
            }
            var _country = new CountryViewModel
            {
                CountryId = country.CountryId,
                Name = country.Name,
                ShortName = country.ShortName
            };

            return View(_country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var country = countriesService.Get(id);
            countriesService.Delete(country);
            countriesService.Save();
            return RedirectToAction(nameof(List));
        }

        public IActionResult Error(string exeption)
        {
            return View((object)exeption);
        }
    }
}