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
    [Route("2ScullTattooStudioShop/Tattoos")]
    public class TattoosApiController : Controller
    {
        IService<TattooDTO> tattooService;

        public TattoosApiController(IService<TattooDTO> _tattooService)
        {
            tattooService = _tattooService;
        }

        // GET: 2ScullTattooStudioShop/Tattoos
        [HttpGet]
        public IEnumerable<TattooDTO> GetTattoos()
        {
            return tattooService.GetAll();
        }

        // GET: 2ScullTattooStudioShop/Tattoos/5
        [HttpGet("{id}", Name = "GetTattoo")]
        public TattooDTO GetTattoo(int id)
        {
            return tattooService.Get(id);
        }

        // POST: 2ScullTattooStudioShop/Tattoos
        [HttpPost]
        public void PostTattoo([FromBody]TattooDTO value)
        {
            tattooService.Update(value);
        }

        // PUT: 2ScullTattooStudioShop/Tattoo
        [HttpPut]
        public void PutTattoo([FromBody]TattooDTO value)
        {
            tattooService.Add(value);
            tattooService.Save();
        }

        // DELETE: 2ScullTattooStudioShop/Tattoos/5
        [HttpDelete("{id}")]
        public void DeleteTattoo(int id)
        {
            tattooService.Delete(tattooService.Get(id));
        }
    }
}
