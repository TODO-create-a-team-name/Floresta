﻿
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Floresta.Models
{
    public class FlorestaDbContext : IdentityDbContext<User>
    {
        public FlorestaDbContext(DbContextOptions<FlorestaDbContext> options)
            : base(options) { }

    }
}
