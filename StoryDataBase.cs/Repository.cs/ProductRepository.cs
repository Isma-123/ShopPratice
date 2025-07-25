

using Microsoft.Extensions.Logging;
using StoryDataBase.cs.Base;
using StoryDataBase.cs.Context;
using StoryDataBase.cs.Intefaces;
using StoryDates.cs.BussinessEntities;
using StoryDates.cs.Repository;
using System.Data.Entity;

namespace StoryDataBase.cs.Repository.cs
{
    public class ProductRepository :
        BaseRepository<Product>,
        IProductDates
    {
        private readonly StoryContext _storycontext;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(StoryContext storyContext,
            ILogger<ProductRepository> _logger) : base(storyContext)
        {
            _storycontext = storyContext;
            this._logger = _logger;
        }

        public override async Task<SucefullyResult> Add(Product entity)
        {
            SucefullyResult result = new SucefullyResult();

            if (entity == null)
            {
                result.status = false;
                result.message = "Product not found.";
                return result;
            }

            // Validaciones
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                result.status = false;
                result.message = "The product name is required.";
                return result;
            }

            if (entity.Price <= 0)
            {
                result.status = false;
                result.message = "The product price must be greater than zero.";
                return result;
            }

            if (entity.Stock < 0)
            {
                result.status = false;
                result.message = "The product stock cannot be negative.";
                return result;
            }

            if (string.IsNullOrWhiteSpace(entity.Description))
            {
                result.status = false;
                result.message = "The product description is required.";
                return result;
            }

            if (string.IsNullOrWhiteSpace(entity.ImageUrl))
            {
                result.status = false;
                result.message = "The product image URL is required.";
                return result;
            }



            if (await base.Exist(s => s.ProductId.Equals(entity.ProductId)))
            {

                result.status = false;
                result.message = " there is already a id on the system!";
                return result;
            }

            try
            {
                await base.Add(entity);
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error trying to add the product: {ex.Message}";
                _logger.LogError($"Error type: {ex.Message}");

            }

            return result;
        }

        public async override Task<SucefullyResult> GetById(int id)
        {
            SucefullyResult result = new SucefullyResult();

            if (id <= 0)
            {
                result.status = false;
                result.message = "The Id Product Number must be 0 or greater than 0!";
                return result;
            }

            try
            {
                var date = await (from products in _storycontext.Products
                                  join i in _storycontext.ProductCategories
                                  on products.ProductId equals i.ProductId
                                  join category in _storycontext.Category
                                  on i.CategoryId equals category.CategoryId
                                  where products.ProductId == id
                                  group new { products, category } by new
                                  {

                                      ProductId = products.ProductId,
                                      Name = products.Name,
                                      Stock = products.Stock,
                                      ImagenUrl = products.ImageUrl,
                                      Isactive = products.IsActive,
                                      Date = products.date,
                                      Price = products.Price,
                                      Description = products.Description,
                                  } into a
                                  select new
                                  {
                                      a.Key.Stock,
                                      a.Key.Price,
                                      a.Key.Description,
                                      a.Key.Date,
                                      a.Key.Name,
                                      a.Key.ImagenUrl,
                                      a.Key.Isactive,
                                      a.Key.ProductId,
                                      Category = a.Select(s => s.category.CategoryId).Distinct().ToList(),
                                  }
                                  ).AsNoTracking()
                                  .FirstOrDefaultAsync()
                                  .ConfigureAwait(false);



                result.message = $"Id {id} found it on the system";

            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error trying to getting the product: {ex.Message}";
                _logger.LogError($"Error type: {ex.Message}");

            }

            return result;
        }

        public async override Task<SucefullyResult> GetAll()
        {
            SucefullyResult result = new SucefullyResult();


            try
            {


                var date = await (from products in _storycontext.Products //  logica mucho a mucho
                                  join i in _storycontext.ProductCategories
                                  on products.ProductId equals i.ProductId
                                  join c in _storycontext.Category
                                  on i.CategoryId equals c.CategoryId
                                  orderby products.date descending
                                  where products.IsActive == true
                                  group new { products, c } by new
                                  {
                                      products.ProductId,
                                      products.Name,
                                      products.Description,
                                      products.Price,
                                      products.IsActive,
                                      products.date,
                                      products.Stock,
                                      products.ImageUrl
                                  } into g
                                  select new
                                  {
                                      g.Key.ProductId,
                                      g.Key.Name,
                                      g.Key.Description,
                                      g.Key.Price,
                                      g.Key.IsActive,
                                      g.Key.date,
                                      g.Key.ImageUrl,
                                      g.Key.Stock,
                                      Category = g.Select(x => x.c.CategoryId).ToList(),

                                  }
                                  ).AsNoTracking()
                                  .ToListAsync();


                result.message = "List of products is already disclosed!";
                result.result = result;
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error trying to disclosing the products: {ex.Message}";
                _logger.LogError($"Error type: {ex.Message}");


            }
            return result;
        }

        public async override Task<SucefullyResult> Update(Product entity)
        {
            SucefullyResult result = new SucefullyResult();


            if (entity == null)
            {
                result.status = false;
                result.message = "Product not found.";
                return result;
            }

            if (string.IsNullOrWhiteSpace(entity.Name) || string.IsNullOrEmpty(entity.ImageUrl) || string.IsNullOrEmpty(entity.Description))
            {
                result.status = false;
                result.message = "Product name cannot be empty.";
                return result;
            }

            if (entity.Price < 0)
            {
                result.status = false;
                result.message = "Price must be a positive value.";
                return result;
            }

            if (entity.Stock < 0)
            {
                result.status = false;
                result.message = "Stock cannot be negative.";
                return result;
            }
            try
            {
                Product? response = await _storycontext.Products.FindAsync(entity.ProductId);

                if(response != null)
                {
                    Product productupdate = new Product();
                    productupdate.ProductId = entity.ProductId;
                    productupdate.Name = entity.Name;
                    productupdate.ImageUrl = entity.ImageUrl;     
                    productupdate.date = DateTime.Now;
                    productupdate.Description   = entity.Description;   
                    productupdate.Price = entity.Price;
                    productupdate.IsActive = entity.IsActive;
                    productupdate.Stock = entity.Stock;

                    var date = await base.Update(productupdate);
                    result.result = date;
                    result.message = "Product updated sucefully!";
                } else
                {
                    result.status = false;
                    result.message = "error we cannot find the product on the system!";
                }
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error updating the product : {ex.Message}";
                _logger.LogError($"Error type: {ex.Message}");
            }
            return result;
        }
    }
}
