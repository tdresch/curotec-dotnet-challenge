using Curotec.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Curotec.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
    }
}