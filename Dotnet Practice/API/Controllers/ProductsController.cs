using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly DataContext _context;
        public ProductsController(DataContext context)
        {
            _context = context;
        }


        [HttpPost("add")]
        public async Task<ActionResult<Product>> Add(ProductDTO productDTO){
            if(await ProductExists(productDTO.productName)){
                return BadRequest("Product Name is taken");
            }

            if(await ProductUserIdCheck(productDTO.userId)){
                return BadRequest("The user should be a product manager to add a product");
            }


            var product = new Product{
                productName = productDTO.productName.ToLower(),
                Price = productDTO.Price,
                Description = productDTO.Description,
                Category = productDTO.Category,
                userId = productDTO.userId
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        private async Task<bool> ProductUserIdCheck(int userId){
            var user = await _context.Users.FindAsync(userId);
            if(user.UserType == "Product Manager"){
                return false;
            }else{
                return true;
            }
        }


        private async Task<bool> ProductExists(string productName){
            return await _context.Products.AnyAsync(x => x.productName == productName.ToLower());
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // api/products/3
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _context.Products.FindAsync(id);
        }
    }
}