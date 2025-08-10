

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StoryBussinessLogic.Dto.OrderDto.cs
{
    public class BaseOrderDto
    {
   
       
        public DateTime date { get; set; }
        public decimal Total { get; set; }
        public int UserId { get; set; }     
        public int OrderDetailsId { get; set; }
    }
}
