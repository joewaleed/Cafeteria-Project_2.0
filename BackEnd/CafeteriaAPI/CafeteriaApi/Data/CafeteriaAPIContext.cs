using CafeteriaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaApi.Data;

public class CafeteriaAPIContext:DbContext{
    public CafeteriaAPIContext(DbContextOptions<CafeteriaAPIContext> options) : base(options){}
    
    public DbSet<User> Users { get; set; }
}