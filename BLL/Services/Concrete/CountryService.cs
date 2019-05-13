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
    public class CountryService : IService<CountryDTO>
    {
        protected IGenericRepository<Country> repo;

        public CountryService(IConfiguration configuration)
        {
            repo = new CountryRepository(configuration);
        }

        public void Add(CountryDTO countryViewModel)
        {
            Country newCountry = new Country
            {
                CountryId = countryViewModel.CountryId,
                Name = countryViewModel.Name
            };
            repo.Add(newCountry);
        }

        public void Delete(CountryDTO countryViewModel)
        {
            Country delCountry = repo.Get(countryViewModel.CountryId);
            repo.Delete(delCountry);
        }

        public CountryDTO Get(int id)
        {
            Country country = repo.Get(id);
            CountryDTO countryViewModel = new CountryDTO
            {
                CountryId = country.CountryId,
                Name = country.Name
            };
            return countryViewModel;
        }

        public IEnumerable<CountryDTO> GetAll()
        {
            return repo
                        .GetAll()
                          .Select(x => new CountryDTO
                          {
                              CountryId = x.CountryId,
                              Name = x.Name
                          });
        }

        public void Save()
        {
            repo.Save();
        }

        public void Update(CountryDTO countryViewModel)
        {
            Country newCountry = repo.Get(countryViewModel.CountryId);
            if (newCountry != null)
            {
                newCountry.CountryId = countryViewModel.CountryId;
                newCountry.Name = countryViewModel.Name;
            }
            repo.Save();
        }
    }
}
