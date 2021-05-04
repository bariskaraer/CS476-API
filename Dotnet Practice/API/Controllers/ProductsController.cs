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
                userId = productDTO.userId,
                quantity = productDTO.quantity,
                brand = productDTO.brand
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
            foreach(Product product_ in await _context.Products.ToListAsync()){
                var id = product_.Id;
                var totalRate= _context.Comments.Where( x => x.ProductID == id.ToString()).Select(x=> x.Rating).Sum();
                var totalComment= _context.Comments.Where(x => x.ProductID == id.ToString()).Count();
                var product = await _context.Products.FindAsync(id);
                double calculation = ((double)totalRate)/(double)totalComment;
                if(totalComment==0){
                    calculation = 0;
                }
                product.Rating=calculation;
                await _context.SaveChangesAsync();
            }

            return await _context.Products.ToListAsync();

           
        }

        // api/products/3
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            
            var totalRate= _context.Comments.Where( x => x.ProductID == id.ToString()).Select(x=> x.Rating).Sum();
            var totalComment= _context.Comments.Where(x => x.ProductID == id.ToString()).Count();
            var product = await _context.Products.FindAsync(id);
            double calculation = ((double)totalRate)/(double)totalComment;
            if(totalComment==0){
                calculation = 0;
            }
            product.Rating=calculation;
            await _context.SaveChangesAsync();
            return product;
        }
         
        //http://localhost:5000/api/products/update/1
        [HttpPost("update/{id}")]
        public async Task<ActionResult<Product>> Update(int id, ProductDTO productDTO){
            if(!(await ProductExists(productDTO.Id))){
                return BadRequest("Product does not exist");
            }

            if(await ProductUserIdCheck(productDTO.userId)){
                return BadRequest("The user should be a product manager to add a product");
            }

            var product = await _context.Products.FindAsync(id);

            product.productName = productDTO.productName.ToLower();
            product.Price = productDTO.Price;
            product.Description = productDTO.Description;
            product.Category = productDTO.Category;
            product.userId = productDTO.userId;
            product.quantity = productDTO.quantity;
            product.brand = productDTO.brand;
            
            await _context.SaveChangesAsync();
            return product;
        }
        private async Task<bool> ProductExists(int productId){
            return await _context.Products.AnyAsync(x => x.Id == productId);
        }

        //http://localhost:5000/api/products/delete/1
        [HttpPost("delete/{id}")]
            public async Task<ActionResult<bool>> Delete(int id,ProductDeleteDTO productDeleteDTO){
            if(!(await ProductExists(id))){
                return BadRequest("Product does not exist");
            }

            if(await ProductUserIdCheck(productDeleteDTO.Id)){
                return BadRequest("The user should be a product manager to delete a product");
            }

            Product product = new Product { Id = id };
            _context.Products.Attach(product);
            _context.Entry(product).State = EntityState.Deleted; 
            await _context.SaveChangesAsync();
            return true;
        }


        [HttpGet("getByPM/{id}")]
        public IQueryable<Product> GetByProductManager(int id)
        {
            return  _context.Products.Where( x => x.userId == id);
        }
    }
}