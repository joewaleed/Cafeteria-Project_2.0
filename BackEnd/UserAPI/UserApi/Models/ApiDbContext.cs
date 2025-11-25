using Microsoft.EntityFrameworkCore;

namespace UserApi.Models;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions options):base(options){
        
    }
    
    public DbSet<Users>  users { get; set; }
}