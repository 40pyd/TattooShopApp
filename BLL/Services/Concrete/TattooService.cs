using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.Repositories.Concrete;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services.Concrete
{
    public class TattooService : IService<TattooDTO>
    {
        protected IGenericRepository<Tattoo> repo;

        public TattooService(IConfiguration configuration)
        {
            repo = new TattooRepository(configuration);
        }

        public void Add(TattooDTO tattooViewModel)
        {
            Tattoo newTattoo = new Tattoo
            {
                TattoId = tattooViewModel.TattoId,
                Name = tattooViewModel.Name,
                Price= tattooViewModel.Price,
                StyleId = tattooViewModel.StyleId,
                AuthorId= tattooViewModel.AuthorId,
                Image = tattooViewModel.Image,
            };
            repo.Add(newTattoo);
        }

        public void Delete(TattooDTO entity)
        {
            Tattoo delTattoo = repo.Get(entity.TattoId);
            repo.Delete(delTattoo);
        }

        public TattooDTO Get(int id)
        {
            Tattoo tattoo = repo.Get(id);
            return new TattooDTO
            {
                TattoId = tattoo.TattoId,
                Name = tattoo.Name,
                Price = tattoo.Price,
                StyleId = tattoo.StyleId,
                StyleName = tattoo.Style?.Name,
                AuthorId = tattoo.AuthorId,
                AuthorName = tattoo.Author?.Name,
                Image = tattoo.Image
            };
        }

        public IEnumerable<TattooDTO> GetAll()
        {
            return repo
                        .GetAll()
                          .Select(tattoo => new TattooDTO
                          {
                              TattoId = tattoo.TattoId,
                              Name = tattoo.Name,
                              Price = tattoo.Price,
                              StyleId = tattoo.StyleId,
                              StyleName = tattoo.Style.Name,
                              AuthorId = tattoo.AuthorId,
                              AuthorName = tattoo.Author.Name,
                              Image = tattoo.Image
                          });
        }

        public void Save()
        {
            repo.Save();
        }

        public void Update(TattooDTO tattooViewModel)
        {
            Tattoo newTattoo = repo.Get(tattooViewModel.TattoId);
            if (newTattoo != null)
            {
                newTattoo.TattoId = tattooViewModel.TattoId;
                newTattoo.Name = tattooViewModel.Name;
                newTattoo.Price = tattooViewModel.Price;
                newTattoo.StyleId = tattooViewModel.StyleId;
                newTattoo.AuthorId = tattooViewModel.AuthorId;
                newTattoo.Image = tattooViewModel.Image;
            }
            repo.Save();
        }
    }
}
