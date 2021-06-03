using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        [Required]
        public int NotificationId  { get; set; } 
        [Required]
        public int UserId  { get; set; } 
        [Required]
        public int Seen { get; set; }
    }
}