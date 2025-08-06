
using StoryDates.cs.BussinessEntities;

namespace StoryBussinessLogic.Dto.OrderDetailsDto.cs
{
    public class GetOrderDetailsDto : BaseOrderDetailsDto
    {
        public bool IsActive { get; set; }
        public int orderDetailsId { get; set; }
        public Order Order { get; set; } = null!;
    }
}
