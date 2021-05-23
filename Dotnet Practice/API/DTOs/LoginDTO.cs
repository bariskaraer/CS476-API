using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string userName  { get; set; } 
        [Required]
        public string Password { get; set; }
        public string UserType {get ;set;}
        public int linking_id {get ;set;}  
        public string MailCode{get;set;} 
    }
}