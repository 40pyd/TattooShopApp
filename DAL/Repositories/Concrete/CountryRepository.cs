using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories.Concrete
{
    public class CountryRepository : GenericRepository<Country>
    {
        public CountryRepository(IConfiguration configuration) : base(configuration){}
    }
}
