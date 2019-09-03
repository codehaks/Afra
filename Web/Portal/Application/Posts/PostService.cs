using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Application.Posts.Models;
using Portal.Domain.Entities;
using Portal.Persistance;

namespace Portal.Application.Posts
{
    public class PostService : IPostService
    {
        private readonly PortalDbContext _db;

        public PostService(PortalDbContext db)
        {
            _db = db;
        }
        public async Task<int> Create(PostAddModel model)
        {
            var post = new Post
            {
                Body = model.Body,
                UserId = model.UserId,
                TimeCreated = DateTime.Now
            };

            var result=_db.Posts.Add(post);
            await _db.SaveChangesAsync();
            return result.Entity.Id;
        }

        public List<PostViewInfo> GetAll()
        {
            return _db.Posts.Select(p => new PostViewInfo
            {
                Id = p.Id,
                Body = p.Body,
                UserId = p.UserId,
                TimeCreated = p.TimeCreated
            }).ToList();
        }
    }
}
