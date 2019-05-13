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
    [Route("2ScullTattooStudioShop/Products")]
    public class ProductsApiController : Controller
    {
        IService<ProductDTO> productService;

        public ProductsApiController(IService<ProductDTO> _productService)
        {
            productService = _productService;
        }

        // GET: 2ScullTattooStudioShop/Products
        [HttpGet]
        public IEnumerable<ProductDTO> GetProducts()
        {
            return productService.GetAll();
        }

        // GET: 2ScullTattooStudioShop/Products/5
        [HttpGet("{id}", Name = "GetProduct")]
        public ProductDTO GetProduct(int id)
        {
            return productService.Get(id);
        }

        // POST: 2ScullTattooStudioShop/Products
        [HttpPost]
        public void PostProduct([FromBody]ProductDTO value)
        {
            productService.Update(value);
        }

        // PUT: 2ScullTattooStudioShop/Products
        [HttpPut]
        public void PutProduct([FromBody]ProductDTO value)
        {
            productService.Add(value);
            productService.Save();
        }

        // DELETE: 2ScullTattooStudioShop/Products/5
        [HttpDelete("{id}")]
        public void DeleteProduct(int id)
        {
            productService.Delete(productService.Get(id));
        }
    }
}
