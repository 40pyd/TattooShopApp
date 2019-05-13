using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories.Concrete
{
    public class BrandRepository : GenericRepository<Brand>
    {
        public BrandRepository(IConfiguration configuration) : base(configuration){}
    }
}
