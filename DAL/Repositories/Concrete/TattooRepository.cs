using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories.Concrete
{
    public class TattooRepository : GenericRepository<Tattoo>
    {
        public TattooRepository(IConfiguration configuration) : base(configuration){}
    }
}
