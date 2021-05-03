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
    public class SalesController : BaseApiController
    {
        private readonly DataContext _context;
        public SalesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sales>>> GetSales()
        {
            return await _context.Sales.ToListAsync();
        }


        // api/sales/3
        [HttpGet("{id}")]
        public async Task<ActionResult<Sales>> GetSale(int id)
        {
            return await _context.Sales.FindAsync(id);
        }



        [HttpPost("add")]
        public async Task<ActionResult<Sales>> Add(SalesDTO salesDTO){
            if(!(await ProductsExists(salesDTO.productId))){
                return BadRequest("Product does not exist");
            }

            if(!(await UserExists(salesDTO.userId))){
                return BadRequest("User does not exist");
            }
            
            var productIdStr = salesDTO.productId.ToString();
            var sale = new Sales{
                productId = productIdStr,
                userId = salesDTO.userId,
                price = salesDTO.price,
                amount = salesDTO.amount
            };

            var product = await _context.Products.FindAsync(salesDTO.productId);
            if(salesDTO.amount <= product.quantity){
                var new_amount = product.quantity - salesDTO.amount;
                product.quantity = new_amount;
            }else{
                return BadRequest("The input amount cannot be larger than the quantity of product");
            }

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        private async Task<bool> ProductsExists(int id){
            return await _context.Products.AnyAsync(x => x.Id == id);
        }

        private async Task<bool> UserExists(int id){
            return await _context.Users.AnyAsync(x => x.Id == id);
        }

        [HttpGet("getByProduct/{id}")]
        public IQueryable<Sales> GetByProductSales(int id)
        {
            return  _context.Sales.Where(x => x.productId == id.ToString());
        }

        [HttpGet("getByUser/{id}")]
        public IQueryable<Sales> GetByUserSales(int id)
        {
            return  _context.Sales.Where( x => x.userId == id);
        }



        
    }
}