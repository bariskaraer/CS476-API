using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required]
        public string categoryName { get; set; }

    }
}