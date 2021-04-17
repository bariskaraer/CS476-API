using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Controllers
{
    public class RatingController : BaseApiController
    {
        private readonly DataContext _context;
        public RatingController(DataContext context)
        {
         _context = context;

        }

        //http://localhost:5000/api/rating/productId/1
        [HttpGet("productId/{productId}")]
        public ActionResult<double> GetProductRatings(string productId)
        {
        var totalRate= _context.Comments.Where( x => x.ProductID == productId.ToString()).Select(x=> x.Rating).Sum();
        var totalComment=_context.Comments.Where(x => x.ProductID == productId.ToString()).Count();
           
        if(totalComment==0){
            return 0;
            }
        
        return ((double)totalRate)/(double)totalComment;
        }

    }
}