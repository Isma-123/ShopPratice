

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


        public async override Task<SucefullyResult> GetById(int id)
        {
            SucefullyResult result = new SucefullyResult();


            if (id <= 0)
            {
                result.status = false;
                result.message = "The Id Category Number must be 0 or greater than 0!";
                return result;
            }

            try
            {
                var response = await (from category in _storyContext.Category
                                      join i in _storyContext.ProductCategories
                                      on category.CategoryId equals i.CategoryId
                                      join products in _storyContext.Products
                                      on i.ProductId equals products.ProductId
                                      where category.CategoryId.Equals(id)
                                      group new { category, products } by new
                                      {
                                          category.date,
                                          category.IsActive,
                                          category.CategoryName,
                                          category.CategoryId
                                      }

                                      into a
                                      select new
                                      {
                                          a.Key.date,
                                          a.Key.IsActive,
                                          a.Key.CategoryName,
                                          a.Key.CategoryId,
                                          Product = a.Select(s => s.products.ProductId).Distinct().ToList(),

                                      }

                                ).ToListAsync()
                                .ConfigureAwait(false); 
                                
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error getting the category id: {ex.Message}";
               _logger.LogError($"Exception Getting the (Category): {ex.Message}", ex);

            }

            return result;
        }

        public async override Task<SucefullyResult> GetAll()
        {
            SucefullyResult result = new SucefullyResult();
            try
            {


                var date = await (from category in _storyContext.Category
                                  join i in _storyContext.ProductCategories
                                  on category.CategoryId equals i.CategoryId
                                  join product in _storyContext.Products
                                  on i.ProductId equals product.ProductId
                                  where category.IsActive
                                  orderby category.date descending
                                  group new { category, product } by new
                                  {
                                      category.CategoryId,
                                      category.CategoryName,
                                      category.date,
                                      category.IsActive
                                  } into g
                                  select new
                                  {
                                      g.Key.CategoryId,
                                      g.Key.CategoryName,
                                      g.Key.date,
                                      g.Key.IsActive,
                                      Products = g.Select(x => new
                                      {
                                          x.product.ProductId,
                                          x.product.Name,
                                          x.product.Price,
                                          x.product.Stock
                                        
                                      }).ToList()
                                  })
                          .ToListAsync();


                result.result = date;
                result.message = "category has been disclosed!";

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
                    categoryupdate.CategoryName = entity.CategoryName;
                    categoryupdate.date = DateTime.Now;
                    categoryupdate.IsActive = entity.IsActive;

                    var response = await base.Update(categoryupdate);
                    result.result = response;
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
