using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories.Concrete
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(IConfiguration configuration) : base(configuration){}
    }
}
