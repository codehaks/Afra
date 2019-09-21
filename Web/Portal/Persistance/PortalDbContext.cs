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
        private readonly IHttpContextAccessor _ca;
        private readonly IUserIdentityService _userIdentityService;

        public PortalDbContext(DbContextOptions<PortalDbContext> options, IUserIdentityService userIdentityService, IHttpContextAccessor contextAccessor)
                : base(options)
        {
            _ca = contextAccessor;
            _userIdentityService = userIdentityService;
        }

        //private readonly string UserId;

        public DbSet<Post> Posts { get; set; }

        public override int SaveChanges()
        {
            var identity = (ClaimsIdentity)_ca.HttpContext.User.Identity;
            var userId = _ca.HttpContext?
               .User
               .Claims
                .SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
                .Value;

            var userId2 = _userIdentityService.GetUserId();

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
            var identity = (ClaimsIdentity)_ca.HttpContext.User.Identity;
            var userId = _ca.HttpContext?
               .User
               .Claims
                .SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
                .Value;

            var userId2 = _userIdentityService.GetUserId();

            //foreach (var entry in ChangeTracker
            //     .Entries()
            //     .Where(e => e.Entity is IUserInfo && e.State == EntityState.Added)
            //     .Select(e => e.Entity as IUserInfo))
            //{

            //    entry.UserId = UserId;

            //}
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PostConfig());
            base.OnModelCreating(builder);
        }
    }
}
