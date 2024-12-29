using Microsoft.EntityFrameworkCore;
using TheMemoryGameBackend.Models;

namespace TheMemoryGameBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } // User
        public DbSet<Score> Scores { get; set; } // Score

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置 Score 与 User 的一对多关系
            modelBuilder.Entity<Score>()
                .HasOne(s => s.User) // 一个 Score 关联一个 User
                .WithMany(u => u.Scores) // 一个 User 可以有多个 Score
                .HasForeignKey(s => s.UserId) // 外键是 Score 表中的 UserId
                .OnDelete(DeleteBehavior.Cascade); // 当 User 被删除时，自动删除关联的 Scores
        }
    }
}