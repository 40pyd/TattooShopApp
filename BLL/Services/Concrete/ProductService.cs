using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services.Concrete
{
    public class ProductService : IService<ProductDTO>
    {
        protected IGenericRepository<Product> repo;

        public ProductService(IConfiguration configuration)
        {
            repo = new ProductRepository(configuration);
        }

        public void Add(ProductDTO productViewModel)
        {
            Product newProduct = new Product
            {
                ProductId = productViewModel.Id,
                Name=productViewModel.Name,
                Description=productViewModel.Description,
                Price=productViewModel.Price,
                CategoryId=productViewModel.CategoryId,
                BrandId=productViewModel.BrandId,
                Image=productViewModel.Image
            };
            repo.Add(newProduct);
        }

        public void Delete(ProductDTO entity)
        {
            Product delProduct = repo.Get(entity.Id);
            repo.Delete(delProduct);
        }

        public ProductDTO Get(int id)
        {
            Product product = repo.Get(id);
            ProductDTO productViewModel = new ProductDTO();

            productViewModel.Id = product.ProductId;
            productViewModel.Name = product.Name;
            productViewModel.Description = product.Description;
            productViewModel.Price = product.Price;
            productViewModel.CategoryId = product.CategoryId;
            productViewModel.BrandId = product.BrandId;
            productViewModel.CategoryName = product.Category?.Name;
            productViewModel.BrandName = product.Brand?.Name;
            productViewModel.Image = product.Image;
            return productViewModel;
        }

        public IEnumerable<ProductDTO> GetAll()
        {
            return repo
                        .GetAll()
                          .Select(x => new ProductDTO
                          {
                              Id = x.ProductId,
                              Name = x.Name,
                              CategoryId = x.Category.CategoryId,
                              CategoryName = x.Category.Name,
                              BrandId = x.BrandId,
                              BrandName = x.Brand.Name,
                              Price=x.Price,
                              Description=x.Description,
                              Image=x.Image
                          });
        }

        public void Save()
        {
            repo.Save();
        }

        public void Update(ProductDTO productViewModel)
        {
            Product newProduct = repo.Get(productViewModel.Id);
            if(newProduct!=null)
            {
                newProduct.ProductId = productViewModel.Id;
                newProduct.Name = productViewModel.Name;
                newProduct.Description = productViewModel.Description;
                newProduct.Price = productViewModel.Price;
                newProduct.CategoryId = productViewModel.CategoryId;
                newProduct.BrandId = productViewModel.BrandId;
                newProduct.Image = productViewModel.Image;
            }
            repo.Save();
        }
    }
}
