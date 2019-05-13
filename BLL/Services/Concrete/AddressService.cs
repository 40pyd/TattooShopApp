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
    public class AddressService : IService<AddressDTO>
    {
        protected IGenericRepository<Address> repo;

        public AddressService(IConfiguration configuration)
        {
            repo = new AddressRepository(configuration);
        }

        public void Add(AddressDTO addressViewModel)
        {
            Address newAddress = new Address
            {
               AddressId=addressViewModel.AddressId,
               AddressLine=addressViewModel.AddressLine,
               ContactName=addressViewModel.ContactName,
               City=addressViewModel.City,
               CountryId=addressViewModel.CountryId
            };
            repo.Add(newAddress);
        }

        public void Delete(AddressDTO entity)
        {
            Address delAddress = repo.Get(entity.AddressId);
            repo.Delete(delAddress);
        }

        public AddressDTO Get(int id)
        {
            Address address = repo.Get(id);
            AddressDTO addressViewModel = new AddressDTO();

            addressViewModel.AddressId = address.AddressId;
            addressViewModel.AddressLine = address.AddressLine;
            addressViewModel.CountryName = address.ContactName;
            addressViewModel.CountryId = address.CountryId;
            addressViewModel.CountryName = address.Country?.Name;
            addressViewModel.City = address.City;

            return addressViewModel;
        }

        public IEnumerable<AddressDTO> GetAll()
        {
            return repo
                        .GetAll()
                          .Select(x => new AddressDTO
                          {
                              AddressId = x.AddressId,
                              AddressLine = x.AddressLine,
                              ContactName=x.ContactName,
                              CountryName = x.ContactName,
                              CountryId = x.CountryId,
                              City = x.City
                          });
        }

        public void Save()
        {
            repo.Save();
        }

        public void Update(AddressDTO addressViewModel)
        {
            Address newAddress = repo.Get(addressViewModel.AddressId);
            if (newAddress != null)
            {
                newAddress.AddressId = addressViewModel.AddressId;
                newAddress.AddressLine = addressViewModel.AddressLine;
                newAddress.ContactName = addressViewModel.ContactName;
                newAddress.City = addressViewModel.City;
                newAddress.CountryId = addressViewModel.CountryId;
            }
            repo.Save();
        }
    }
}
