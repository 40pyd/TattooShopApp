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
    [Route("2ScullTattooStudioShop/Authors")]
    public class AuthorsApiController : Controller
    {
        IService<AuthorDTO> authorService;

        public AuthorsApiController(IService<AuthorDTO> _authorService)
        {
            authorService = _authorService;
        }

        // GET: 2ScullTattooStudioShop/Authors
        [HttpGet]
        public IEnumerable<AuthorDTO> GetAuthors()
        {
            return authorService.GetAll();
        }

        // GET: 2ScullTattooStudioShop/Authors/5
        [HttpGet("{id}", Name = "GetAuthor")]
        public AuthorDTO GetAuthor(int id)
        {
            return authorService.Get(id);
        }

        // POST: 2ScullTattooStudioShop/Authors
        [HttpPost]
        public void Post([FromBody]AuthorDTO value)
        {
            authorService.Update(value);
        }

        // PUT: 2ScullTattooStudioShop/Authors
        [HttpPut]
        public void PutAuthor([FromBody]AuthorDTO value)
        {
            authorService.Add(value);
            authorService.Save();
        }

        // DELETE: 2ScullTattooStudioShop/Authors/5
        [HttpDelete("{id}")]
        public void DeleteAuthor(int id)
        {
            authorService.Delete(authorService.Get(id));
        }
    }
}
