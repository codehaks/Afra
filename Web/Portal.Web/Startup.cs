using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Portal.Application.Posts;
using Portal.Domain.Identity;
using Portal.Persistance;
using Portal.Persistance.Identity;
using Portal.Web.Common;

namespace Portal.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUserClaimsPrincipalFactory<ApplicationUser>, MyClaimsPrincipalFactory>();
            services.AddDbContext<PortalDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });


            services.AddTransient<IEmailSender, EmailSender>();
            services.AddIdentity<ApplicationUser,IdentityRole>(
                options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<PortalDbContext>();

            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizeAreaFolder("User", "/");
                //options.Conventions.AuthorizeFolder("/users",);
                //options.Conventions.AuthorizeFolder("/admin", "RequireAdminRole");

            });
            services.AddScoped<IUserIdentityService, UserIdentityService>();
            services.AddMvc();
            services.AddHealthChecks()
                    .AddCheck<PortalWebHealthCheck>("portalweb_health_check");
            services.AddTransient<IPostService, PostService>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
