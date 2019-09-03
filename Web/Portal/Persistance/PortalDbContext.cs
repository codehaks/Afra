using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portal.Domain.Core;
using Portal.Domain.Entities;
using Portal.Domain.Identity;
using Portal.Persistance.Configs;
using System;
using System.Linq;

namespace Portal.Persistance
{
    public class PortalDbContext : IdentityDbContext<ApplicationUser>
    {
        public PortalDbContext(DbContextOptions<PortalDbContext> options)
                : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }

        public override int SaveChanges()
        {

            foreach (var entry in ChangeTracker
                 .Entries()
                 .Where(e => e.Entity is ITimeCreated && e.State == EntityState.Added)
                 .Select(e => e.Entity as ITimeCreated))
            {
                if (entry.TimeCreated <= DateTime.MinValue)
                {
                    entry.TimeCreated = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PostConfig());
            base.OnModelCreating(builder);
        }
    }
}
