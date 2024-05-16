using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryManager : ICategoryService
    {
        private readonly IRepositoryManager manager;

        public CategoryManager(IRepositoryManager manager)
        {
            this.manager = manager;
 
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges)
        {
            return await manager.Category.GetAllCategoriesAsync(trackChanges);
        }

        public async Task<Category> GetOneByIdCategoryAsync(int id, bool trackChanges)
        {
            var values =  await manager.Category.GetCategoryByIdAsync(id, trackChanges);

            if(values is null)
            {
                throw new CategoryNotFoundException(id);
            }

            return values;
           
        }
    }
}
