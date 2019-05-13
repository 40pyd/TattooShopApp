using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories.Concrete
{
    public class AddressRepository : GenericRepository<Address>
    {
        public AddressRepository(IConfiguration configuration) : base(configuration){}
    }
}
