using Microsoft.EntityFrameworkCore;
using TheMemoryGameBackend.Models;

namespace TheMemoryGameBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } // 用户表
    }
}