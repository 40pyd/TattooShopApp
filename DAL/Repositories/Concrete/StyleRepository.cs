using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories.Concrete
{
    public class StyleRepository : GenericRepository<Style>
    {
        public StyleRepository(IConfiguration configuration) : base(configuration){}
    }
}
