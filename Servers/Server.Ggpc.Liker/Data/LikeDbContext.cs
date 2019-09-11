using Microsoft.EntityFrameworkCore;

namespace Server.Ggpc.Liker.Data
{
    public class LikeDbContext:DbContext
    {
        public LikeDbContext(DbContextOptions<LikeDbContext>options):base(options)
        {
            
        }
        public DbSet<LikeModel>Likes { get; set; }
    }
}
