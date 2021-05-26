using API.Data;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentEmail.Core;
using FluentEmail.Smtp;
using System.Net.Mail;
using System.Text;
using System;

namespace API.Controllers
{
    public class FinanceController: BaseApiController
    {
        private readonly DataContext _context;
        public FinanceController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("pay")]
        public async Task<ActionResult<AppUser>> Payment(FinanceDto financeDTO){
            if(!(await UserExists(financeDTO.customerID))){
                return BadRequest("User does not exist");
            }
            var user = await _context.Users.FindAsync(financeDTO.customerID);
            var newBalance = user.Balance - financeDTO.fee;
            if(newBalance < 0){
                return BadRequest("The fee is too much. There is not enough balance on this users wallet.");
            }
            user.Balance = newBalance;
            await _context.SaveChangesAsync();
            return user;
        }


        [HttpPost("addMoney")]
        public async Task<ActionResult<AppUser>> AddMoney(FinanceDto financeDTO){
            if(!(await UserExists(financeDTO.customerID))){
                return BadRequest("User does not exist");
            }
            var user = await _context.Users.FindAsync(financeDTO.customerID);
            var newBalance = user.Balance + financeDTO.fee;
            user.Balance = newBalance;
            await _context.SaveChangesAsync();
            return user;
        }

        private async Task<bool> UserExists(int id){
            return await _context.Users.AnyAsync(x => x.Id == id);
        }
        
    }
}