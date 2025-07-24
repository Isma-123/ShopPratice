

using StoryDates.cs.BaseDate;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoryDates.cs.BussinessEntities
{
    [Table("", Schema = "")]
    public sealed class User : BaseEntitie
    {
        // entities representive

        [Required]
        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(30)]
        public string FullName { get; set; } = null!;
        [Required]
        [MaxLength(30)]
        public string Email { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string PasswordHash { get; set; } = null!;
        [Required]
        [MaxLength(30)]
        public string Rol { get; set; } = "Client";

        // Orm  
        [Required]    
        public int OrderId { get; set; } 
        public ICollection<Order>? Orders {  get; set; } = new List<Order>();


    }
}
