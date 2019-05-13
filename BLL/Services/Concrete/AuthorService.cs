using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services.Concrete
{
    public class AuthorService : IService<AuthorDTO>
    {
        protected IGenericRepository<Author> repo;

        public AuthorService(IConfiguration configuration)
        {
            repo = new AuthorRepository(configuration);
        }

        public void Add(AuthorDTO authorViewModel)
        {
            Author newAuthor = new Author
            {
                AuthorId = authorViewModel.AuthorId,
                Name = authorViewModel.Name,
                Sername = authorViewModel.Sername,
                NickName = authorViewModel.NickName,
                Age = authorViewModel.Age,
                Photo = authorViewModel.Photo
            };
            repo.Add(newAuthor);
        }

        public void Delete(AuthorDTO entity)
        {
            Author delAuthor = repo.Get(entity.AuthorId);
            repo.Delete(delAuthor);
        }

        public AuthorDTO Get(int id)
        {
            Author style = repo.Get(id);
            return new AuthorDTO
            {
                AuthorId = style.AuthorId,
                Name = style.Name,
                Sername = style.Sername,
                NickName = style.NickName,
                Age = style.Age,
                Photo = style.Photo
            };
        }

        public IEnumerable<AuthorDTO> GetAll()
        {
            return repo
                        .GetAll()
                          .Select(x => new AuthorDTO
                          {
                              AuthorId = x.AuthorId,
                              Name = x.Name,
                              Sername = x.Sername,
                              NickName = x.NickName,
                              Age = x.Age,
                              Photo = x.Photo
                          });
        }

        public void Save()
        {
            repo.Save();
        }

        public void Update(AuthorDTO authorViewModel)
        {
            Author newAuthor = repo.Get(authorViewModel.AuthorId);
            if (newAuthor != null)
            {
                newAuthor.AuthorId = authorViewModel.AuthorId;
                newAuthor.Name = authorViewModel.Name;
                newAuthor.Sername = authorViewModel.Sername;
                newAuthor.NickName = authorViewModel.NickName;
                newAuthor.Age = authorViewModel.Age;
                newAuthor.Photo = authorViewModel.Photo;
            }
            repo.Save();
        }
    }
}
