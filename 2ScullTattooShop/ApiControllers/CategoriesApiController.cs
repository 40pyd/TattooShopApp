using System.Collections.Generic;
using BLL.Models;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _2ScullTattooShop.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("2ScullTattooStudioShop/Categories")]
    public class CategoriesApiController : Controller
    {
        IService<CategoryDTO> categoriesService;

        public CategoriesApiController(IService<CategoryDTO> _categoriesService)
        {
            categoriesService = _categoriesService;
        }

        // GET: 2ScullTattooStudioShop/Categories
        [HttpGet]
        public IEnumerable<CategoryDTO> GetCategories()
        {
            return categoriesService.GetAll();
        }

        // GET: 2ScullTattooStudioShop/Categories/5
        [HttpGet("{id}", Name = "GetCategory")]
        public CategoryDTO GetCategory(int id)
        {
            return categoriesService.Get(id);
        }

        // POST: 2ScullTattooStudioShop/Categories
        [HttpPost]
        public void PostCategory([FromBody]CategoryDTO value)
        {
            categoriesService.Update(value);
        }

        // PUT: 2ScullTattooStudioShop/Categories
        [HttpPut]
        public void PutCategory([FromBody]CategoryDTO value)
        {
            categoriesService.Add(value);
            categoriesService.Save();
        }

        // DELETE: 2ScullTattooStudioShop/Categories/5
        [HttpDelete("{id}")]
        public void DeleteCategory(int id)
        {
            categoriesService.Delete(categoriesService.Get(id));
        }
    }
}
