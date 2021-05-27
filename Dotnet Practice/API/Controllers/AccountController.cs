using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentEmail.Core;
using FluentEmail.Smtp;
using System.Net.Mail;
using System.Text;
using System;
using System.Net;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        // Instantiate random number generator.  
        private readonly Random _random = new Random();  
        public AccountController(DataContext context)
        {
            _context = context;
        }

        // Generates a random number within a range.      
        public int RandomNumber(int min, int max)  
        {  
        return _random.Next(min, max);  
        }  
        public string RandomString(int size, bool lowerCase = false)  
        {  
        var builder = new StringBuilder(size);  
        
        // Unicode/ASCII Letters are divided into two blocks
        // (Letters 65–90 / 97–122):
        // The first group containing the uppercase letters and
        // the second group containing the lowercase.  

        // char is a single Unicode character  
        char offset = lowerCase ? 'a' : 'A';  
        const int lettersOffset = 26; // A...Z or a..z: length=26  
        
        for (var i = 0; i < size; i++)  
        {  
        var @char = (char)_random.Next(offset, offset + lettersOffset);  
        builder.Append(@char);  
        }  
        
        return lowerCase ? builder.ToString().ToLower() : builder.ToString();  
        }  

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDTO registerDTO){
            if(await UserExists(registerDTO.userName)){
                return BadRequest("Username is taken");
            }


            var user = new AppUser{
                userName = registerDTO.userName,
                Name = registerDTO.Name,
                Surname = registerDTO.Surname,
                Age = registerDTO.Age,
                Password = registerDTO.Password,
                Email = registerDTO.Email,
                UserType = registerDTO.UserType,
                linking_id = registerDTO.linking_id,
                Balance = 100
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        
  
        


        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDTO loginDTO){
            var user = await _context.Users.SingleOrDefaultAsync(x => x.userName == loginDTO.userName);
            if(user == null){
                return Unauthorized("Invalid Username");
            }
            if(loginDTO.Password != user.Password) return Unauthorized("Wrong password");
            
            // Send Email
            var sender =new SmtpSender(() => new SmtpClient("email-smtp.us-east-2.amazonaws.com", 587){
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,    
                Credentials = new NetworkCredential("AKIA3OZN4OEO7JODVWHV", "BJaigG9XlMHs0RhdXpE7OAoeRocssOts4aHp1Xh9kXoN") 
                //DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                //PickupDirectoryLocation = @"C:\Demos"
            });
            string rand_string = RandomString(8);
            var body_email = "This is your verification number " + rand_string + " . Please paste this text to the ecommerce website. ";

            Email.DefaultSender = sender;
            var email = await Email
                .From("baris.karaer@ozu.edu.tr")
                .To(user.Email,user.Name)
                .Subject("Your Login Code !")
                .Body(body_email)
                .SendAsync();

            user.MailCode = rand_string;
            await _context.SaveChangesAsync();
            
            return user;

            
        }


        [HttpPost("twofactor")]
        public async Task<ActionResult<bool>> TwoFactor(TwoFactorDTO TwoFactorDTO){
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == TwoFactorDTO.userId);
            if(user == null){
                return Unauthorized("Invalid Username");
            }
            if(TwoFactorDTO.MailCode != user.MailCode){
                return false;
            }
            return true;
        }


        private async Task<bool> UserExists(string username){
            return await _context.Users.AnyAsync(x => x.userName == username.ToLower());
        }



        [HttpPost("update/{id}")]
        public async Task<ActionResult<AppUser>> Update(int id, RegisterDTO registerDTO){
            if(!(await UserExists(registerDTO.Id))){
                return BadRequest("User does not exist");
            }

            var user = await _context.Users.FindAsync(id);

            user.userName = registerDTO.userName;
            user.Name = registerDTO.Name;
            user.Surname = registerDTO.Surname;
            user.Age = registerDTO.Age;
            user.Password = registerDTO.Password;
            user.Email = registerDTO.Email;
            user.UserType = registerDTO.UserType;

            await _context.SaveChangesAsync();
            return user;
        }
        private async Task<bool> UserExists(int id){
            return await _context.Users.AnyAsync(x => x.Id == id);
        }

    }
}