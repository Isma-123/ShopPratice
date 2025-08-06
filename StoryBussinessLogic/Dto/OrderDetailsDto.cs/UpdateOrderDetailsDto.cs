
namespace StoryBussinessLogic.Dto.OrderDetailsDto.cs
{
    public class UpdateOrderDetailsDto : BaseOrderDetailsDto   
    {
        public int orderDetailsId { get; set; }
        public bool IsActive { get; set; }
    }
}
