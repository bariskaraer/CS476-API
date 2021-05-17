using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CartsDTO
    {
        
        public int Id { get; set; }
        [Required]
        public int userId { get; set; }
        [Required]
        public int product { get; set; }
        public int quantity { get; set; }
    }
}