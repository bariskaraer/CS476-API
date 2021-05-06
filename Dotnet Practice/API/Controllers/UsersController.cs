using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // api/users/3
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }


        // its gonna give the linked product manager id, if the parameter id is a customer or a product manager then its gonna give a bad request error
        [HttpGet("getLink/{id}")]
        public ActionResult<IQueryable<AppUser>> GetLinkedUser(int id)
        {
            var linked_id = _context.Users.Find(id).linking_id;
            if(linked_id == 0){
                return BadRequest("No Existing linked user, this user may be a customer or a product manager, please call this method with a sales manager id");
            }
            return Ok(_context.Users.Where( x => x.Id == linked_id));
        }

    }
}