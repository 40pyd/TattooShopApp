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
    public class OrderService:IService<OrderDTO>
    {
            protected IGenericRepository<Order> repo;

            public OrderService(IConfiguration configuration)
            {
                repo = new OrderRepository(configuration);
            }

            public void Add(OrderDTO orderViewModel)
            {
            Order newOrder = new Order
            {
                OrderId = orderViewModel.OrderId,
                CustomerId = orderViewModel.CustomerId,
                AddressId = orderViewModel.AddressId,
                TotalValue = orderViewModel.TotalValue
            };
                repo.Add(newOrder);
                repo.Save();
            }

            public void Delete(OrderDTO entity)
            {
                Order delOrder = repo.Get(entity.OrderId);
                repo.Delete(delOrder);
            }

            public OrderDTO Get(int id)
            {
                Order order = repo.Get(id);
                OrderDTO orderViewModel = new OrderDTO();

                orderViewModel.OrderId = order.OrderId;
                orderViewModel.CustomerId = order.CustomerId;
                orderViewModel.CreatedOn = order.CreatedOn;
                orderViewModel.AddressId = order.AddressId;
                orderViewModel.AddressLine = order.Address?.AddressLine;
                orderViewModel.TotalValue = order.TotalValue;
            
                return orderViewModel;
            }

            public IEnumerable<OrderDTO> GetAll()
            {
                 return repo
                            .GetAll()
                              .Select(x => new OrderDTO
                              {
                                  OrderId = x.OrderId,
                                  CustomerId = x.CustomerId,
                                  CreatedOn=x.CreatedOn,
                                  AddressId = x.AddressId,
                                  AddressLine = x.Address.AddressLine,
                                  TotalValue = x.TotalValue
                              });
            }

            public void Save()
            {
                repo.Save();
            }

            public void Update(OrderDTO orderViewModel)
            {
                Order newOrder = repo.Get(orderViewModel.OrderId);
                if (newOrder != null)
                {
                    newOrder.OrderId = orderViewModel.OrderId;
                    newOrder.CustomerId = orderViewModel.CustomerId;
                    newOrder.AddressId = orderViewModel.AddressId;
                    newOrder.TotalValue = orderViewModel.TotalValue;
                }
                repo.Save();
            }
        }
}
