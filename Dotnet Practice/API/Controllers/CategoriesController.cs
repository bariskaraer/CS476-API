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
    public class CategoriesController : BaseApiController
    {
        private readonly DataContext _context;
        public CategoriesController(DataContext context)
        {
            _context = context;
        }


        [HttpPost("add")]
        public async Task<ActionResult<Product>> Add(CategoryDTO categoryDTO){
            if(await CategoryExists(categoryDTO.categoryName)){
                return BadRequest("Category Name is taken");
            }


            var category = new Category{
                categoryName = categoryDTO.categoryName
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        private async Task<bool> CategoryExists(string categoryName){
            return await _context.Categories.AnyAsync(x => x.categoryName == categoryName);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        // api/categories/3
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategories(int id)
        {
            return await _context.Products.Where(x => x.Category == Convert.ToString(id)).ToListAsync();
        }

    }
}