using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class ProductDeleteDTO
    {
        [Required]
        public int Id { get; set; }
        
    }
}