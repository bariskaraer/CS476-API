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

        // http://localhost:5000/api/carts/update/9/13/400
        // update/{productID}/{userID}/{quantity}
        [HttpPost("update/{productID}/{userID}/{quantity}")]
        public async Task<ActionResult<Carts>> Update(int productID, int userID,int quantity){
            
            var cart= await _context.Carts.FirstOrDefaultAsync(x => x.product == productID && x.userId == userID);

            cart.quantity = quantity;
            await _context.SaveChangesAsync();
            return Ok();
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
                product = cartDTO.product,
                quantity = cartDTO.quantity
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }





        //http://localhost:5000/api/carts/delete
        [HttpPost("delete")]
        public async Task<ActionResult<Carts>> Delete(CartsDTO cartDTO){

        if(!(await UserExists(cartDTO.userId))){
            return BadRequest("User does not exist");
        }

        if(!(await ProductExists(cartDTO.product))){
            return BadRequest("Product does not exist");
        }


        var new_id = _context.Carts.Where(x => x.userId == cartDTO.userId && x.product == cartDTO.product).First();
        //Carts cart = new Carts { Id = new_id.Id, userId=new_id.userId,product = new_id.product };
        //_context.Carts.Attach(cart);
        //_context.Entry(cart).State = EntityState.Deleted; 
        _context.Carts.Remove(new_id);
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