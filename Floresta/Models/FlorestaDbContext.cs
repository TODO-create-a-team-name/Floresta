using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Floresta.Models
{
    public class FlorestaDbContext : IdentityDbContext<User>
    {
        public FlorestaDbContext(DbContextOptions<FlorestaDbContext> options)
            : base(options) {}

        public DbSet<Seedling> Seedlings { get; set; }
        public DbSet<Marker> Markers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<QuestionTopic> QuestionTopics { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                 .HasMany(q => q.Questions)
                 .WithOne(u => u.User)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasMany(p => p.Payments)
                .WithOne(u => u.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<QuestionTopic>()
                .HasMany(q => q.Questions)
                .WithOne(t => t.QuestionTopic)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
