using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portal.Domain.Core;
using Portal.Domain.Entities;
using Portal.Domain.Identity;
using Portal.Persistance.Configs;
using Portal.Persistance.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Portal.Persistance
{
    public class PortalDbContext : IdentityDbContext<ApplicationUser>
    {

        private readonly IUserIdentityService _userIdentityService;

        public PortalDbContext(DbContextOptions<PortalDbContext> options, IUserIdentityService userIdentityService)
                : base(options)
        {
            _userIdentityService = userIdentityService;
        }

        public DbSet<Post> Posts { get; set; }

        public override int SaveChanges()
        {
            var userId = _userIdentityService.GetUserId();

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

            //foreach (var entry in ChangeTracker
            //     .Entries()
            //     .Where(e => e.Entity is IUserInfo && e.State == EntityState.Added)
            //     .Select(e => e.Entity as IUserInfo))
            //{

            //    entry.UserId = UserId;

            //}

            return base.SaveChanges();
        }

        public override  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _userIdentityService.GetUserId();

            foreach (var entry in ChangeTracker
                 .Entries()
                 .Where(e => e.Entity is IUserInfo && e.State == EntityState.Added)
                 .Select(e => e.Entity as IUserInfo))
            {

                entry.UserId = userId;

            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var userId = _userIdentityService.GetUserId();
            builder.ApplyConfiguration(new PostConfig());
            builder.Entity<Post>().HasQueryFilter(p => p.UserId == userId);
            base.OnModelCreating(builder);
        }
    }
}
