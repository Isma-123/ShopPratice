

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoryDataBase.cs.Base;
using StoryDataBase.cs.Context;
using StoryDataBase.cs.Intefaces;
using StoryDates.cs.BussinessEntities;
using StoryDates.cs.Repository;
using System.Data.Entity;

namespace StoryDataBase.cs.Repository.cs
{
    public class CategoryRepository : BaseRepository<Category>, ICategoriesDates
    {   

        private readonly StoryContext _storyContext;
        private readonly ILogger<CategoryRepository> _logger;   
        public CategoryRepository(StoryContext storyContext, 
            ILogger<CategoryRepository> _logger) : base(storyContext)
        { 
            _storyContext = storyContext;
            this._logger = _logger; 
        }

        public override async Task<SucefullyResult> Add(Category entity)
        {
            SucefullyResult result = new SucefullyResult();

            // Validaciones manuales
            if (string.IsNullOrWhiteSpace(entity.CategoryName))
            {
                result.status = false;
                result.message = "Category name is required.";
                return result;
            }

            if (entity.CategoryName.Length > 100)
            {
                result.status = false;
                result.message = "Category name must not exceed 100 characters.";
                return result;
            }

         
            if (await base.Exist(c => c.CategoryName == entity.CategoryName))
            {
                result.status = false;
                result.message = $"Category '{entity.CategoryName}' already exists.";
                return result;
            }

            try
            {

                await base.Add(entity);
                result.message = "Category added successfully.";
                result.result = entity;
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error adding category: {ex.Message}";
                _logger.LogError($"Exception in Add(Category): {ex.Message}", ex);
            }

            return result;
        }

    
        public async override Task<SucefullyResult> GetAll()
        {
            SucefullyResult result = new SucefullyResult();
            try
            {

                var data = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                    .Include(_storyContext.Category, s => s.Products)
                     .OrderByDescending(s => s.date)
                     .Select(d => new
                     {
                     
                         d.CategoryId,
                         d.CategoryName,
                         d.date,
                         d.IsActive,
                         Products = d.Products.Select(a => new Product
                         {
                             ProductId = a.ProductId,
                         }).ToList()
                     }).ToListAsync();

                result.result = data;

            }
            catch (Exception ex)
            {

                result.status = false;
                result.message = $"Error adding category: {ex.Message}";
               _logger.LogError($"Exception in list all of the(Category): {ex.Message}", ex);

            }

            return result;
        }

        public async override Task<SucefullyResult> Update(Category entity)
        {
            SucefullyResult result = new SucefullyResult();

            if (entity.CategoryId <= 0)
            {
                result.status = false;
                result.message = "Invalid category ID. It must be greater than zero.";
                return result;
            }

            if (string.IsNullOrWhiteSpace(entity.CategoryName))
            {
                result.status = false;
                result.message = "Category name is required.";
                return result;
            }

            if (entity.CategoryName.Length > 100)
            {
                result.status = false;
                result.message = "Category name must not exceed 100 characters.";
                return result;
            }


            try
            {
                Category? date = await _storyContext.Category.FindAsync(entity.CategoryId);

                if(date != null)
                {
                    Category categoryupdate = new Category();
                    categoryupdate.CategoryId = entity.CategoryId;
                    categoryupdate.ProductId = entity.ProductId;    
                    categoryupdate.CategoryName = entity.CategoryName;
                    categoryupdate.date = DateTime.Now;
                    categoryupdate.IsActive = entity.IsActive;

                    var response = await base.Update(categoryupdate);
                    result.message = "the category has been updated on the system already!";
                } else
                {
                    result.status = false;
                    result.message = "unable to find the category on the system!";
                }
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error updating the category: {ex.Message}";
               _logger.LogError($"Exception in Update(Category): {ex.Message}", ex);

            }

            return result;
        }
    }
}
