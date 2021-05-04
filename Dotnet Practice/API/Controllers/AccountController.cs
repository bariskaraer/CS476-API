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
                UserType = registerDTO.UserType,
                linking_id = registerDTO.linking_id
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



        //http://localhost:5000/api/products/update/1
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
            user.linking_id = registerDTO.linking_id;

            await _context.SaveChangesAsync();
            return user;
        }
        private async Task<bool> UserExists(int id){
            return await _context.Users.AnyAsync(x => x.Id == id);
        }

    }
}