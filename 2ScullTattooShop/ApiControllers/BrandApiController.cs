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
    [Route("2ScullTattooStudioShop/Brands")]
    public class BrandsApiController : Controller
    {
        IService<BrandDTO> brandsService;

        public BrandsApiController(IService<BrandDTO> _brandsService)
        {
            brandsService = _brandsService;
        }

        // GET: 2ScullTattooStudioShop/Brands
        [HttpGet]
        public IEnumerable<BrandDTO> GetBrands()
        {
            return brandsService.GetAll();
        }

        // GET: 2ScullTattooStudioShop/Brands/5
        [HttpGet("{id}", Name = "GetBrand")]
        public BrandDTO GetBrand(int id)
        {
            return brandsService.Get(id);
        }

        // POST: 2ScullTattooStudioShop/Brands
        [HttpPost]
        public void PostBrand([FromBody]BrandDTO value)
        {
            brandsService.Update(value);
        }

        // PUT: 2ScullTattooStudioShop/Brands
        [HttpPut]
        public void PutBrand([FromBody]BrandDTO value)
        {
            brandsService.Add(value);
            brandsService.Save();
        }

        // DELETE: 2ScullTattooStudioShop/Brands/5
        [HttpDelete("{id}")]
        public void DeleteBrand(int id)
        {
            brandsService.Delete(brandsService.Get(id));
        }
    }
}
