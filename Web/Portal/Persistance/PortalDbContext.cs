using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portal.Domain.Core;
using Portal.Domain.Entities;
using Portal.Domain.Identity;
using Portal.Persistance.Configs;
using Portal.Persistance.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Portal.Persistance
{
    public class PortalDbContext : IdentityDbContext<ApplicationUser>
    {
        public PortalDbContext(DbContextOptions<PortalDbContext> options, IUserIdentityService userIdentityService)
                : base(options)
        {
            UserId = userIdentityService.GetUserId();
        }

        private readonly string UserId;

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

            foreach (var entry in ChangeTracker
                 .Entries()
                 .Where(e => e.Entity is IUserInfo && e.State == EntityState.Added)
                 .Select(e => e.Entity as IUserInfo))
            {

                entry.UserId = UserId;

            }

            return base.SaveChanges();
        }

        public override  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker
                 .Entries()
                 .Where(e => e.Entity is IUserInfo && e.State == EntityState.Added)
                 .Select(e => e.Entity as IUserInfo))
            {

                entry.UserId = UserId;

            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PostConfig());
            base.OnModelCreating(builder);
        }
    }
}
