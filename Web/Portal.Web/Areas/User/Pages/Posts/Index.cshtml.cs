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
                var allPostsId = _postService.GetAll().Select(x => x.Id);

                //var channel = new Channel("localhost:5007", SslCredentials.Insecure);
               var channel= GrpcChannel.ForAddress("https://localhost:5007");
                var client = new Liker.LikerClient(channel);

                var reply = await client.GetAllImagesAndLikesAsync(new Ids()
                {
                    ArrayPostid = { allPostsId }
                }, cancellationToken: ct);

                if (reply.ImageLikeList.Any())
                {
                    foreach (var like in reply.ImageLikeList)
                    {
                        var likedPost = allPosts.Where(p => p.Id == like.PostId).Select(l =>
                         {
                             l.TotalLikeCount = like.TotalCount;
                             return l;
                         });

                        Posts = allPosts.Union(likedPost).ToList();
                    }
                }
                else
                {
                    Posts = allPosts;
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
