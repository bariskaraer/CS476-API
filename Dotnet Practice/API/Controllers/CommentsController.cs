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
    public class CommentsController : BaseApiController
    {
        private readonly DataContext _context;
        public CommentsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<ActionResult<Product>> Add(CommentDTO commentDTO){
            if(await CommentExists(commentDTO.UserID, commentDTO.ProductID)){
                return BadRequest("Cant post more than 1 comment and rating");
            }


            var comment = new Comment{
                UserID = commentDTO.UserID,
                UserName = commentDTO.UserName,
                ProductID = commentDTO.ProductID,
                CommentDescription = commentDTO.CommentDescription,
                Rating = commentDTO.Rating,
                ApprovedStatus = commentDTO.ApprovedStatus,
                AddedDate = commentDTO.AddedDate
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return Ok(comment);
        }

        private async Task<bool> CommentExists(string UserID, string ProductID){
            return await _context.Comments.AnyAsync(x => x.UserID == UserID && x.ProductID == ProductID );
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsRelatedToProducts(string id)
        {
            return await _context.Comments.Where(x => x.ProductID == id).ToListAsync();
        }   
        
    }
}