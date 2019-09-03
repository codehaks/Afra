using Microsoft.EntityFrameworkCore;
using Servers.Vega.Data;
using System;
using System.IO;
using System.Linq;

namespace Servers.Vega.Data
{
    public class VegaDbContext : DbContext
    {
        public VegaDbContext(DbContextOptions<VegaDbContext> options)
                : base(options)
        {
        }

        public DbSet<Doc> Docs { get; set; }

      
    }
}
