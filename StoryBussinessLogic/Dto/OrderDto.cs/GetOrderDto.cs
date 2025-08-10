

using StoryDates.cs.BussinessEntities;

namespace StoryBussinessLogic.Dto.OrderDto.cs
{
    public class GetOrderDto : BaseOrderDto
    {
        public int OrderId { get; set; }
        public bool IsActive { get; set; }
        public ICollection<OrderDetails> DetailsDto { get; set; } = null!;
        public User UserDto { get; set; } = null!;


    }
}
