using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers
{
    public class CartsController : BaseApiController
    {
        private readonly DataContext _context;
        public CartsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carts>>> GetCarts()
        {
            return await _context.Carts.ToListAsync();
        }

        // api/sales/3
        [HttpGet("{id}")]
        public async Task<ActionResult<Carts>> GetCart(int id)
        {
            return await _context.Carts.FindAsync(id);
        }

        [HttpPost("add")]
        public async Task<ActionResult<Carts>> Add(CartsDTO cartDTO){
            if(!(await UserExists(cartDTO.userId))){
                return BadRequest("User does not exist");
            }

            if(!(await ProductExists(cartDTO.product))){
                return BadRequest("Product does not exist");
            }
            
            var cart = new Carts{
                userId = cartDTO.userId,
                product = cartDTO.product
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }





        //http://localhost:5000/api/carts/delete
        [HttpPost("delete/{id}")]
        public async Task<ActionResult<Carts>> Delete(int id,CartsDTO cartDTO){

        if(!(await UserExists(cartDTO.userId))){
            return BadRequest("User does not exist");
        }

        if(!(await ProductExists(cartDTO.product))){
            return BadRequest("Product does not exist");
        }


        Carts cart = new Carts { Id = id };
        _context.Carts.Attach(cart);
        _context.Entry(cart).State = EntityState.Deleted; 
        await _context.SaveChangesAsync();
        return Ok();
        }

        private  Task<bool> UserExists(int id){
            return  _context.Users.AnyAsync(x => x.Id == id);
        }

        private  Task<bool> ProductExists(int id){
            return  _context.Products.AnyAsync(x => x.Id == id);
        }



        [HttpGet("getByProduct/{id}")]
        public IQueryable<Carts> GetByProductCarts(int id)
        {
            return  _context.Carts.Where(x => x.product == id);
        }

        [HttpGet("getByUser/{id}")]
        public IQueryable<Carts> GetByUserCarts(int id)
        {
            return  _context.Carts.Where( x => x.userId == id);
        }
    }
}