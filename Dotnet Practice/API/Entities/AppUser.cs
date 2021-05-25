using System;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string userName  { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Age {get; set;}
        public string Password { get; set; }

        public string Email {get ;set;}

        public string UserType {get ;set;}

        public int linking_id {get ;set;}

        public string MailCode{get;set;}

        public int Balance{get;set;}
    }
}



//salesManager has to select a productmanager when registering
//when registering for a product manager the user dont have to choose a sales manager (dont have to implement anything for this)
