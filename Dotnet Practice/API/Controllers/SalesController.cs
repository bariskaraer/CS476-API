using System;
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

            var total_price = salesDTO.amount * salesDTO.price;
            var prod_id = salesDTO.productId;

            var producttt = await _context.Products.SingleOrDefaultAsync(x => x.Id == prod_id);
            var user_id = producttt.userId;
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == user_id);
            user.Balance = user.Balance + total_price;
            

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

        [HttpGet("getSales/{id}")]
        public ActionResult<IQueryable<Sales>> GetLinkedSales(int id){
            var linked_id = _context.Users.Find(id).linking_id;
            if(linked_id == 0){
                return BadRequest("No Existing linked user, this user may be a customer or a product manager, please call this method with a sales manager id");
            }
            var linkedIdOfSalesManager = _context.Users.Find(linked_id);
            var string_link = linkedIdOfSalesManager.Id;
            IEnumerable<Sales> sales = _context.Sales;
            IEnumerable<Product> products = _context.Products;
            var result = new List<Sales>();
            foreach (var sale in sales)
            {
                foreach (var product in products)
                {
                    if(product.userId == string_link && sale.productId == product.Id.ToString()){
                        result.Add(sale);
                    }
                }
            }
            return Ok(result);
        }

        
    }
}