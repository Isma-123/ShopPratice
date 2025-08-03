

using StoryBussinessLogic.Base;
using StoryBussinessLogic.Dto.ProductDto.cs;
using StoryBussinessLogic.Response.ProductResponse.cs;

namespace StoryBussinessLogic.Services
{
    public interface IProductServices : IBaseServices<ProductResponse, SaveProductDto, GetallProductDto, UpdateProductDto>
    { 
        
    }
}
