using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTO
    {
        public int Id { get; set; }
        [Required]
        public string userName  { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Age {get; set;}
        [Required]
        public string Password { get; set; }
        public string Email {get ;set;}

        public string UserType {get ;set;}
    }
}