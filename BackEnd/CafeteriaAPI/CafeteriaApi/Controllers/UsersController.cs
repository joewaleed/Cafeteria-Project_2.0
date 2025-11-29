using CafeteriaApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CafeteriaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CafeteriaAPIContext _context;
        public UsersController(CafeteriaAPIContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllInfo(){
            
            return Ok(await  _context.Users.ToListAsync());
        }
        
        [HttpGet("{UserID}")]
        public async Task<ActionResult<User>> GetUserInfo(int UserID){
            var user = await _context.Users.FindAsync(UserID);
            if(user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User Newuser){
           
            if (Newuser == null) return BadRequest();
           _context.Users.Add(Newuser);
           await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserInfo), new { userID = Newuser.UserID }, Newuser);
        }
        
        [HttpPut("{UserID}")]
        public async Task<IActionResult> UpdateUser(int UserID,User Updateduser){
            var user = await _context.Users.FindAsync(UserID);
            if (Updateduser == null) return NotFound();
            user.UserID = Updateduser.UserID;
            user.Email = Updateduser.Email;
            user.Name = Updateduser.Name;
            user.Password = Updateduser.Password;
            
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{UserID}")]
        public async Task<IActionResult> DeleteUser(int UserID){
            var user = await _context.Users.FindAsync(UserID);
            if (user == null) return NotFound();
            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
    }
}
