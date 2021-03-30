using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDTO registerDTO){
            if(await UserExists(registerDTO.userName)){
                return BadRequest("Username is taken");
            }


            var user = new AppUser{
                userName = registerDTO.userName.ToLower(),
                Name = registerDTO.Name,
                Surname = registerDTO.Surname,
                Age = registerDTO.Age,
                Password = registerDTO.Password,
                Email = registerDTO.Email,
                UserType = registerDTO.UserType
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
            return user;
        }


        private async Task<bool> UserExists(string username){
            return await _context.Users.AnyAsync(x => x.userName == username.ToLower());
        }


    }
}