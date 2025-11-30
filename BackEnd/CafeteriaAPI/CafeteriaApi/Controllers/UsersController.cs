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
        
        [HttpGet("{Email}")]
        public async Task<ActionResult<User>> GetUserByEmail(string Email){
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            if(user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User newUser)
        {
            if (newUser == null) return BadRequest(new { message = "User cannot be null." });

            if (!newUser.Email.Contains("@"))
                return UnprocessableEntity(new { message = "Email must Contain '@' in it" });
            
            if (string.IsNullOrWhiteSpace(newUser.Email) || newUser.Email.Length > 30)
                return UnprocessableEntity(new { message = "Email must be valid and max 30 characters." });

            if (string.IsNullOrWhiteSpace(newUser.Name) || newUser.Name.Length > 20 || newUser.Name.Length < 3)
                return UnprocessableEntity(new{ message = "Name must be between 3 and 20 characters."});

            if (string.IsNullOrWhiteSpace(newUser.Password) || newUser.Password.Length < 8 ||
                newUser.Password.Length > 20)
                return UnprocessableEntity(new{ message = "Password must be between 8 and 20 characters."});
            
            newUser.Email = newUser.Email.ToLowerInvariant();
            
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == newUser.Email);

            if (existingUser != null)
                return Conflict(new { message = "Email already exists." });
            
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            newUser.Password = hashedPassword;
            
            _context.Users.Add(newUser);

            try
            {
                await _context.SaveChangesAsync();
                newUser.Password = "";
                return CreatedAtAction(nameof(GetUserByEmail), new { Email = newUser.Email },newUser);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("DbUpdateException: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);

                throw;
            }
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
