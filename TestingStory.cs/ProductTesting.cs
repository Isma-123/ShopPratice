using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StoryDataBase.cs.Context;
using StoryDataBase.cs.Intefaces;
using StoryDataBase.cs.Repository.cs;
using StoryDates.cs.BussinessEntities;

namespace TestingStory.cs
{
    public class ProductTesting
    {
        private readonly IProductDates _productdates;
        private readonly StoryContext _context;

        public ProductTesting()
        {
            var date = new DbContextOptionsBuilder<StoryContext>()
                .UseInMemoryDatabase(databaseName: "DbStory")
                .Options;

            _context = new StoryContext(date);   
            var Moq = new Mock<ILogger<ProductRepository>>();
           _productdates = new  ProductRepository(_context, Moq.Object);
        }


        [Fact]
        

        public async Task ShowNull_AddingTheProductoOnTheSystem()
        {
            Product? product = null;


            var request = await _productdates.Add(product!);
            var text = "Product not found.";


            Assert.False(request.status);
            Assert.Equal(text, request.message);
            Assert.Null(request.result);

        }

        [Fact] 

        public async Task ShowErrorOrNull_GettingTheProductId()
        {
            Product product = new Product()
            {
                ProductId = 0,

            };

            var request = await _productdates.GetById(product.ProductId);   
            string text = "The Id Product Number must be 0 or greater than 0!";

            Assert.False(request.status);
            Assert.Equal(text, request.message);
            Assert.IsNotType<Product>(request.result);

        }

        [Fact]

        public async Task ShowErroOrNull_UpdatingTheProductOnTheSystem()
        {
            Product product = new Product()
            {
                ProductId = 3,
                Name = "Pedro pascar",
                Description = null!,
                ImageUrl = null!,
                IsActive = true,
                date = DateTime.Today,

            };


            var request = await _productdates.Update(product);
            var text = "Product name cannot be empty.";


            Assert.False(request.status);
            Assert.Equal(text, request.message);    
            Assert.IsNotAssignableFrom<Product>(request.result);    
            Assert.IsNotType<Product>(request.result);

        }


        [Fact]

        public async Task ShowTrue_IfTheProductExistOntheSystem()
        {
            Product product = new Product()
            {
                ProductId = 3,
            }; 

            var request = await _productdates.Exist(s => s.ProductId == product.ProductId);

            Assert.False(request);

        }
    }
}
