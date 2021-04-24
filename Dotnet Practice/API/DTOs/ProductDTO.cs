using System.Net.Mime;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


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

        public IFormFile productImage {get; set;}
    }
}