using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        
        [Required]
        public string UserID { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string ProductID { get; set; }

        [Required]
        public string CommentDescription { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public int ApprovedStatus { get; set; }

        [Required]
        public string AddedDate{get;set;}
    }
}