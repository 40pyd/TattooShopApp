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
    [Route("2ScullTattooStudioShop/Styles")]
    public class StylesApiController : Controller
    {
        IService<StyleDTO> stylesService;

        public StylesApiController(IService<StyleDTO> _stylesService)
        {
            stylesService = _stylesService;
        }

        // GET: 2ScullTattooStudioShop/Styles
        [HttpGet]
        public IEnumerable<StyleDTO> GetStyles()
        {
            return stylesService.GetAll();
        }

        // GET: 2ScullTattooStudioShop/Styles/5
        [HttpGet("{id}", Name = "GetStyle")]
        public StyleDTO GetStyle(int id)
        {
            return stylesService.Get(id);
        }

        // POST: 2ScullTattooStudioShop/Styles
        [HttpPost]
        public void PostStyle([FromBody]StyleDTO value)
        {
            stylesService.Update(value);
        }

        // PUT: 2ScullTattooStudioShop/Styles
        [HttpPut]
        public void PutStyle([FromBody]StyleDTO value)
        {
            stylesService.Add(value);
            stylesService.Save();
        }

        // DELETE: 2ScullTattooStudioShop/Styles/5
        [HttpDelete("{id}")]
        public void DeleteStyle(int id)
        {
            stylesService.Delete(stylesService.Get(id));
        }
    }
}
