
using StoryDates.cs.BussinessEntities;
using System.ComponentModel.DataAnnotations;

namespace StoryBussinessLogic.Dto.UserDto.cs
{
    public abstract class BaseUserDto
    {

       
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;    
        public string PasswordHash { get; set; } = null!; 
        public string Rol { get; set; } = "Client";  
        public int OrderId { get; set; }
        public DateTime date { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Order>? Orders { get; set; } = new List<Order>();

    }
}
