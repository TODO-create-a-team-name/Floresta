using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Floresta.Models
{
    public class FlorestaDbContext : IdentityDbContext<User>
    {
        public FlorestaDbContext(DbContextOptions<FlorestaDbContext> options)
            : base(options) { }

        public DbSet<Seedling> Seedlings { get; set; }
        public DbSet<Marker> Markers { get; set; }
        public DbSet<Question> Questions { get; set; }
    }
}
