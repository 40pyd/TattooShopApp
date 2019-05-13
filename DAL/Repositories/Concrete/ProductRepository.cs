using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository(IConfiguration configuration) : base(configuration){}
    }
}
