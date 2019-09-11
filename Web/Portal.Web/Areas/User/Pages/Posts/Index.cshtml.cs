using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portal.Application.Posts;
using Portal.Application.Posts.Models;
using Server.Ggpc.Liker;

namespace Portal.Web.Areas.User.Pages.Posts
{

    public class IndexModel : PageModel
    {
        private readonly IPostService _postService;

        public IndexModel(IPostService postService)
        {
            _postService = postService;
        }
        
        
        public List<PostViewInfo> Posts { get; set; }


        public async Task OnGetAsync(CancellationToken ct)
        {
            try
            {
                var allPosts = _postService.GetAll();
                var allPostsId = _postService.GetAll().Select(x=>x.Id).ToList();
                var channel = new Channel("localhost:5006", SslCredentials.Insecure);
                var client = new Liker.LikerClient(channel);
              
                var reply = await client.GetAllImagesAndLikesAsync(new Ids()
                {
                   ArrayPostid = { allPostsId}
                },cancellationToken:ct);

                for (int i = 0; i < allPosts.Count; i++)
                {
                    //Posts.Select(x=>x.TotalLikeCount= reply.AllImagesLikes[i].TotalCount)
                    Posts.Where(x => x.Id == reply.AllImagesLikes[i].PostId)
                        .Select(x => x.TotalLikeCount = reply.AllImagesLikes[i].TotalCount);

                }

                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
                

        }
    }

}
