
using StoryBussinessLogic.Base;
using StoryBussinessLogic.Dto.OrderDto.cs;
using StoryBussinessLogic.Response.OrderResponse.cs;

namespace StoryBussinessLogic.Services
{
    public interface IOrderServices : IBaseServices<OrderResponse, SaveOrderDto, UpdateOrderDto, GetOrderDto>
    {
    }
}
