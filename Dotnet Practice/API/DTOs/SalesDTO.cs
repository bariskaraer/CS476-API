using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class SalesDTO
    {
        [Required]
        public int productId  { get; set; } 
        [Required]
        public int userId { get; set; }
        [Required]
        public int price { get; set; }
        [Required]
        public int amount { get; set; }
    }
}