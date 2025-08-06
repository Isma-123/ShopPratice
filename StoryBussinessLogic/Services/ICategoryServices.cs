

using StoryBussinessLogic.Base;
using StoryBussinessLogic.Dto.CategoryDto.cs;
using StoryBussinessLogic.Response.CategoryResponse.cs;

namespace StoryBussinessLogic.Services
{
    public interface ICategoryServices : IBaseServices<CategoryResponse, SaveCategoryDto, GetCategoryDto, UpdateCategoryDto> 

    {
    }
}
