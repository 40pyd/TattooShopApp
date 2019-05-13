using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories.Concrete
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository(IConfiguration configuration) : base(configuration){}
    }
}
