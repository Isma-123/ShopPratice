using Microsoft.Extensions.Logging;
using StoryBussinessLogic.Dto.CategoryDto.cs;
using StoryBussinessLogic.Response.CategoryResponse.cs;
using StoryBussinessLogic.Services;
using StoryDataBase.cs.Intefaces;
using StoryDates.cs.BussinessEntities;


namespace StoryBussinessLogic.Services_Manager
{
    public class CategoryServices : ICategoryServices
    {


        private readonly ICategoriesDates _categorierepository;
        private readonly ILogger<CategoryServices> _logger;


        public CategoryServices(ICategoriesDates categorierepository, ILogger<CategoryServices> logger)
        {
            _categorierepository = categorierepository ?? throw new ArgumentNullException(nameof(categorierepository));
            _logger = logger;
        } 


        public async Task<CategoryResponse> Add(SaveCategoryDto saveDto)
        {
             CategoryResponse result = new CategoryResponse();    


             try
            { 

                _logger.LogInformation("Starting to add category...");


                 Category category = new Category();    
                 category.CategoryName = saveDto.CategoryName;
                 category.date = DateTime.Now;
             

                var request = await _categorierepository.Add(category);
                result.date = request;

            } catch(Exception ex)
            {
                result.success = false;
                result.text = $"Error while adding category: {ex.Message}";
               _logger.LogError($"Error while adding category: {ex.Message}");
            }   



            return result;
        }

        public async  Task<CategoryResponse> GetAll()
        {
           CategoryResponse result= new CategoryResponse();  

            try
            {
                _logger.LogInformation("Starting to retrieve all categories...");   


                var request = await _categorierepository.GetAll();

                if (request.status)
                {


                    List<GetCategoryDto> getCategoryDtos = ((List<Category>)request.result).Select(
                        c => new GetCategoryDto
                        {
                            CategoryId = c.CategoryId,
                            CategoryName = c.CategoryName,
                            date = c.date
                        }).ToList();


                    if (getCategoryDtos.Count < 0)
                    {
                        result.success = false;
                        result.text = "No categories found.";
                    } else
                    {
                        result.date = getCategoryDtos;
                        result.text = "Categories retrieved successfully.";

                    }

                    return result;

                }
                else
                {
                    result.success = false;
                    result.text = "Failed to retrieve categories.";

                }
            } catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while retrieving categories: {ex.Message}";
               _logger.LogError($"Error while retrieving categories: {ex.Message}");
            
            }


            return result;
        }

        public async Task<CategoryResponse> GetById(int id)
        {
            CategoryResponse result = new CategoryResponse();  

            if(id <= 0)
            {
                result.success = false;
                result.text = "Invalid category ID.";
                return result;
            }   

            try
            {
                _logger.LogInformation($"Starting to retrieve category with ID: {id}..."); 

                var request = await _categorierepository.GetById(id);

                if(request.status)
                {
                    result.date = request.result;
                    result.text = "Category retrieved successfully!";
                } else
                {
                    result.success = false;
                    result.text= "Category not found.";

                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while retrieving category with ID {id}: {ex.Message}";
                _logger.LogError($"Error while retrieving category: {ex.Message}");
            }

            return result;
        }

        public async Task<CategoryResponse> Update(UpdateCategoryDto updatedDto)
        {
            CategoryResponse result = new CategoryResponse();


            try
            {
                var request = await _categorierepository.GetById(updatedDto.CategoryId);


                if (request.status || request is not null)
                {
                    Category categoryupdate = (Category)request.result;
                    categoryupdate.CategoryName = updatedDto.CategoryName;
                    categoryupdate.date = DateTime.Now;
                    categoryupdate.CategoryId = updatedDto.CategoryId;
                    var update = await _categorierepository.Update(categoryupdate);


                    if (categoryupdate is not null)
                    {
                        result.date = update;
                        result.text = "Category updated successfully!";
                    }
                    else
                    {
                        result.success = false;
                        result.text = "Failed to update category.";
                    }

                   }
                }
            catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while updating category: {ex.Message}";
                _logger.LogError($"Error while updating category: {ex.Message}");
            }   

            return result;
        }
    }
}
