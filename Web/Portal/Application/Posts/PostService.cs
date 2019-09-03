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
        public async Task Create(PostAddModel model)
        {
            var post = new Post
            {
                Body = model.Body,
                Content = model.Content,
                UserId = model.UserId,
                MimeType = model.MimeType,
                TimeCreated = DateTime.Now
            };

            _db.Posts.Add(post);
            await _db.SaveChangesAsync();
        }

        public List<PostViewInfo> GetAll()
        {
            return _db.Posts.Select(p => new PostViewInfo
            {
                Id = p.Id,
                Body = p.Body,
                Content = p.Content,
                MimeType = p.MimeType,
                UserId = p.UserId,
                TimeCreated = p.TimeCreated
            }).ToList();
        }
    }
}
