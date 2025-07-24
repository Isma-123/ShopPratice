

using Microsoft.Extensions.Logging;
using StoryDataBase.cs.Base;
using StoryDataBase.cs.Context;
using StoryDataBase.cs.Intefaces;
using StoryDates.cs.BussinessEntities;


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
    }
}
