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
    public class StyleService : IService<StyleDTO>
    {
        protected IGenericRepository<Style> repo;

        public StyleService(IConfiguration configuration)
        {
            repo = new StyleRepository(configuration);
        }

        public void Add(StyleDTO styleViewModel)
        {
            Style newStyle = new Style
            {
                Id = styleViewModel.Id,
                Name = styleViewModel.Name,
                Description = styleViewModel.Description,
                Image = styleViewModel.Image
            };
            repo.Add(newStyle);
        }

        public void Delete(StyleDTO entity)
        {
            Style delStyle = repo.Get(entity.Id);
            repo.Delete(delStyle);
        }

        public StyleDTO Get(int id)
        {
            Style style = repo.Get(id);
            return new StyleDTO
            {
                Id = style.Id,
                Name = style.Name,
                Description = style.Description,
                Image = style.Image
            };
        }

        public IEnumerable<StyleDTO> GetAll()
        {
            return repo
                        .GetAll()
                          .Select(x => new StyleDTO
                          {
                              Id = x.Id,
                              Name = x.Name,
                              Description = x.Description,
                              Image = x.Image
                          });
        }

        public void Save()
        {
            repo.Save();
        }

        public void Update(StyleDTO styleViewModel)
        {
            Style newStyle = repo.Get(styleViewModel.Id);
            if (newStyle != null)
            {
                newStyle.Id = styleViewModel.Id;
                newStyle.Name = styleViewModel.Name;
                newStyle.Description = styleViewModel.Description;
                newStyle.Image = styleViewModel.Image;
            }
            repo.Save();
        }
    }
}
