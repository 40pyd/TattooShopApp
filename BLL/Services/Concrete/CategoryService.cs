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
    public class CategoryService : IService<CategoryDTO>
    {
        protected IGenericRepository<Category> repo;

        public CategoryService(IConfiguration configuration)
        {
            repo = new CategoryRepository(configuration);
        }

        public void Add(CategoryDTO categoryViewModel)
        {
            Category newCategory = new Category
            {
                CategoryId = categoryViewModel.CategoryId,
                Name = categoryViewModel.Name
            };
            repo.Add(newCategory);
        }

        public void Delete(CategoryDTO categoryViewModel)
        {
            Category delCategory = repo.Get(categoryViewModel.CategoryId);
            repo.Delete(delCategory);
        }

        public CategoryDTO Get(int id)
        {
            Category category = repo.Get(id);
            CategoryDTO categoryViewModel = new CategoryDTO
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };
            return categoryViewModel;
        }

        public IEnumerable<CategoryDTO> GetAll()
        {
            return repo
                        .GetAll()
                          .Select(x => new CategoryDTO
                          {
                              CategoryId = x.CategoryId,
                              Name = x.Name
                          });
        }

        public void Save()
        {
            repo.Save();
        }

        public void Update(CategoryDTO categoryViewModel)
        {
            Category newCategory = repo.Get(categoryViewModel.CategoryId);
            if (newCategory != null)
            {
                newCategory.CategoryId = categoryViewModel.CategoryId;
                newCategory.Name = categoryViewModel.Name;
            }
            repo.Save();
        }
    }
}

