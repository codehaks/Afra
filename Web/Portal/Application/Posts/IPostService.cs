using Portal.Application.Posts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application.Posts
{
    public interface IPostService
    {
        public Task Create(PostAddModel model);
        List<PostViewInfo> GetAll();
    }
}
