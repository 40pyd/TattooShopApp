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
    public class BrandService : IService<BrandDTO>
    {
        protected IGenericRepository<Brand> repo;

        public BrandService(IConfiguration configuration)
        {
            repo = new BrandRepository(configuration);
        }

        public void Add(BrandDTO brandViewModel)
        {
            Brand newBrand = new Brand
            {
                BrandId = brandViewModel.BrandId,
                Name = brandViewModel.Name,
                Image = brandViewModel.Image
            };
            repo.Add(newBrand);
        }

        public void Delete(BrandDTO brandViewModel)
        {
            Brand delBrand = repo.Get(brandViewModel.BrandId);
            repo.Delete(delBrand);
        }

        public BrandDTO Get(int id)
        {
            Brand brand = repo.Get(id);
            BrandDTO brandViewModel = new BrandDTO
            {
                BrandId = brand.BrandId,
                Name = brand.Name,
                Image=brand.Image
            };
            return brandViewModel;
        }

        public IEnumerable<BrandDTO> GetAll()
        {
            return repo
                        .GetAll()
                          .Select(x => new BrandDTO
                          {
                              BrandId = x.BrandId,
                              Name = x.Name,
                              Image=x.Image
                          });
        }

        public void Save()
        {
            repo.Save();
        }

        public void Update(BrandDTO brandViewModel)
        {
            Brand newBrand = repo.Get(brandViewModel.BrandId);
            if (newBrand != null)
            {
                newBrand.BrandId = brandViewModel.BrandId;
                newBrand.Name = brandViewModel.Name;
                newBrand.Image = brandViewModel.Image;
            }
            repo.Save();
        }
    }
}
