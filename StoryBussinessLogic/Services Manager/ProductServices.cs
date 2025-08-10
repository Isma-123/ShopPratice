using Microsoft.Extensions.Logging;
using StoryBussinessLogic.Dto.CategoryDto.cs;
using StoryBussinessLogic.Dto.ProductDto.cs;
using StoryBussinessLogic.Response.ProductResponse.cs;
using StoryBussinessLogic.Services;
using StoryDataBase.cs.Repository.cs;
using StoryDates.cs.BussinessEntities;
using System.Linq.Expressions;


namespace StoryBussinessLogic.Services_Manager
{
    public class ProductServices : IProductServices
    {    


        private readonly ProductRepository _productRepository;
        private readonly ILogger<ProductServices> _logger;


        public ProductServices(ProductRepository productRepository, ILogger<ProductServices> logger)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _logger = logger;
        } 

        public async Task<ProductResponse> Add(SaveProductDto saveDto)
        {
            ProductResponse result = new ProductResponse();

            try
            {

                _logger.LogInformation("Starting to add product..."); 


                Product product = new Product();    
                product.Description= saveDto.Description;
                product.Price = saveDto.Price;
                product.Name = saveDto.Name;
                product.Stock = saveDto.Stock;
                product.ImageUrl = saveDto.ImageUrl;
                product.date = DateTime.Now;     


                var response = await _productRepository.Add(product);
                result.date = response; 

                result.text = "Product added successfully!";
            }
            catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while adding product: {ex.Message}";
               _logger.LogError($"Error while adding product: {ex.Message}");

                return result;

            }


            return result;
        }

 

            public async Task<ProductResponse> GetAll()
           {
            ProductResponse result = new ProductResponse();

            try
            {
                _logger.LogInformation("Starting to get all products...");

                var request = await _productRepository.GetAll();

                if (request.status)
                {
                    List<GetallProductDto> dtoproduct = ((List<Product>)request.result).Select(
                        s => new GetallProductDto
                        {
                            ProductId = s.ProductId,
                            Name = s.Name,
                            Description = s.Description,
                            Price = s.Price,
                            Stock = s.Stock,
                            ImageUrl = s.ImageUrl,
                            date = s.date,
                            IsActive = s.IsActive,
                            productCategoriesDto = (ICollection<ProductCategory>)s.productCategories.Select(pc => pc.CategoryId).ToList(),


                        }).ToList();

                    result.date = dtoproduct;
                    result.text = "Products retrieved successfully.";
                }
                else
                {
                    result.success = false;
                    result.text = "Failed to retrieve products.";
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while retrieving products: {ex.Message}";
                _logger.LogError($"Error while retrieving products: {ex.Message}");
            }

            return result;
        }

        public async Task<ProductResponse> GetById(int id)
        {
            ProductResponse result = new ProductResponse();

            if(id <= 0)
            {
                result.success = false;
                result.text = "Invalid product ID.";
                return result;
            }

            try
            {  

               _logger.LogInformation($"Starting to get product by ID: {id}");


                var request = await _productRepository.GetById(id);

                if(!request.status)
                {
                    result.success = false;
                    result.text = "Product not found.";
                    return result; 

                } else
                {

                    result.date = request;
                    result.text = "Product retrieved successfully.";
               

                }
            } catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while retrieving product by ID: {ex.Message}";
                _logger.LogError($"Error while retrieving product by ID: {ex.Message}");
                return result;
            }   

            return result;
        }

        public async Task<ProductResponse> Update(UpdateProductDto updatedDto)
        {
            ProductResponse result = new ProductResponse();


            try
            {
                _logger.LogInformation("Starting to update product...");   

                var  product = await _productRepository.GetById(updatedDto.ProductId); 

                if(!product.status || product is null)
                {
                    result.text = "Product not found."; 
                    result.success = false;
                    return result;
                }


                Product productupdate = (Product)product.result;
                productupdate.ProductId = updatedDto.ProductId;
                productupdate.Name = updatedDto.Name;
                productupdate.Description = updatedDto.Description;
                productupdate.Price = updatedDto.Price;
                productupdate.Stock = updatedDto.Stock;
                productupdate.date = DateTime.Now;  
                productupdate.IsActive = updatedDto.IsActive;   


                var request = await _productRepository.Update(productupdate);   
                 
                if(!request.status)
                {
                    result.success = false;
                    result.text = "Failed to update product.";
                    return result;
                }
                
                    result.date = request.result;
                    result.text = "Product updated successfully!";
                    
            }
            catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while updating product: {ex.Message}";
               _logger.LogError($"Error while updating product: {ex.Message}");

            }
                return result;
        }
    }
}
