using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [Required]
        public string productName  { get; set; } 
        [Required]
        public int Price { get; set; }
        public string Description { get; set; }

        [Required]
        public string Category {get; set;}
        
        [Required]
        public int userId {get; set;}

    }
}