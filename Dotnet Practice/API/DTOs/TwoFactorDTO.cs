using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class TwoFactorDTO
    {
        [Required]
        public int userId  { get; set; } 
        [Required]
        public string MailCode { get; set; }
    }
}