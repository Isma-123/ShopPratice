

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StoryBussinessLogic.Dto.OrderDetailsDto.cs
{
    public abstract class BaseOrderDetailsDto
    {
        public DateTime date { get; set; }
        public bool IsActive { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
