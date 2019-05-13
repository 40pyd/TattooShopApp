using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories
{
    public class AuthorRepository : GenericRepository<Author>
    {
        public AuthorRepository(IConfiguration configuration) : base(configuration){} 
    }
}
