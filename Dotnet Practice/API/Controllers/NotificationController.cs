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
    public class NotificationController: BaseApiController
    {
        private readonly DataContext _context;
        public NotificationController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
        {
            return await _context.Notifications.ToListAsync();
        }

        [HttpGet("getByUser/{id}")]
        public IQueryable<Notification> GetByUserNotification(int id)
        {
            return  _context.Notifications.Where( x => x.UserId == id);
        }

        [HttpGet("getByUserNotSeen/{id}")]
        public IQueryable<Notification> GetByUserNotificationNotSeen(int id)
        {
            return  _context.Notifications.Where( x => x.UserId == id && x.Seen == 0);
        }

        private async Task<bool> UserExists(int id){
            return await _context.Users.AnyAsync(x => x.Id == id);
        }


        [HttpPost("add")]
        public async Task<ActionResult<Notification>> Add(NotificationDto notDto){
            if(await NotificationExists(notDto.NotificationId, notDto.UserId)){
                return BadRequest("Cant post more than 1 notification for user and notification id");
            }


            var notification = new Notification{
                NotificationId = notDto.NotificationId,
                UserId = notDto.UserId,
                Seen = notDto.Seen
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return Ok(notification);
        }

        [HttpPost("change")]
        public async Task<ActionResult<Notification>> ChangeStatus(NotificationDto notDto){
            var notif = await _context.Notifications.FirstOrDefaultAsync(x => x.NotificationId == notDto.NotificationId && x.UserId == notDto.UserId);
            notif.Seen = notDto.Seen;
            await _context.SaveChangesAsync();
            return Ok();
        }

        private async Task<bool> NotificationExists(int NotificationId, int UserId){
            return await _context.Notifications.AnyAsync(x => x.NotificationId == NotificationId && x.UserId == UserId );
        }
    }
}