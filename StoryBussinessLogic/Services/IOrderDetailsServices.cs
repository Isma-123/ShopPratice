

using StoryBussinessLogic.Base;
using StoryBussinessLogic.Dto.OrderDetailsDto.cs;
using StoryBussinessLogic.Response.OrderDetailsResponse.cs;
using StoryDates.cs.BussinessEntities;

namespace StoryBussinessLogic.Services
{
    public interface IOrderDetailsServices : IBaseServices<OrderDetailsResponse, SaveOrderDetailsDto, GetOrderDetailsDto, UpdateOrderDetailsDto>

    {
        
    }
}
