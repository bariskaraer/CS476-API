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
    public class CampaignController:BaseApiController
    {
        private readonly DataContext _context;
        public CampaignController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<ActionResult<Campaign>> Add(CampaignDto campaignDto){
            if(!(await UserOrProductExists(campaignDto.userId, campaignDto.productId))){
                return BadRequest("User or Product doesnt exist");
            }


            var campaign = new Campaign{
                userId = campaignDto.userId,
                productId = campaignDto.productId,
                startDate = campaignDto.startDate,
                endDate = campaignDto.endDate,
                description = campaignDto.description,
                percentage = campaignDto.percentage
            };
            _context.Campaigns.Add(campaign);
            await _context.SaveChangesAsync();

            // campaign.Id
            var all_users = _context.Users.Where(x => x.UserType == "Customer");
            foreach(var user in all_users){
                var notification = new Notification{
                NotificationId = campaign.Id,
                UserId = user.Id,
                Seen = 0
            };
                _context.Notifications.Add(notification);
            }
            await _context.SaveChangesAsync();
            return Ok(campaign);
        }

        private async Task<bool> UserOrProductExists(int UserID, int ProductID){
            var existsUser = await _context.Users.AnyAsync(x => x.Id == UserID);
            var existsProduct = await _context.Products.AnyAsync(x => x.Id == ProductID);
            return existsUser && existsProduct;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Campaign>>> GetCampaigns()
        {
            return await _context.Campaigns.ToListAsync();
        }

        [HttpGet("byProduct/{id}")]
        public async Task<ActionResult<IEnumerable<Campaign>>> GetCampaignsRelatedToProducts(int id)
        {
            return await _context.Campaigns.Where(x => x.productId == id).ToListAsync();
        }
    }
}