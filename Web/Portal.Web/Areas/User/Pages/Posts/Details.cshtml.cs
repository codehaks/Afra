using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portal.Domain.Entities;
using Portal.Persistance;
using Grpc.Net.Client;
using Portal.Web.Common.Extentions;
using Server.Ggpc.Liker;

namespace Portal.Web.Areas.User.Pages.Posts
{
    public class DetailsModel : PageModel
    {
        private readonly PortalDbContext _db;
        public DetailsModel(PortalDbContext db)
        {
            _db = db;
        }

        public Post PostViewModel { get; set; }
        public int Likes { get; set; }
        public bool IsLiked { get; set; }

        public async Task<IActionResult> OnGet(int postId)
        {
            PostViewModel = await _db.Posts.FindAsync(postId);

            var likeChannel = new Grpc.Core.Channel("localhost:5007", SslCredentials.Insecure);
            var likeClient = new Liker.LikerClient(likeChannel);
            var likeReply = await likeClient.GetImageLikesAsync(new PostIdRequest()
            {
                PostId = postId,
                UserId=User.GetUserId(),
                
            });

            Likes = likeReply.TotalCount;
            if (likeReply.UserId == User.GetUserId() && likeReply.Status==TotalLikesReply.Types.Status.Like)
            {
                IsLiked = true;
            }
            else
            {
                IsLiked = false;
            }
            return Page();
        }

        public async Task<IActionResult> OnGetImageAsync(int postId)
        {
            //var channel = GrpcChannel.ForAddress("http://localhost:5005");
            //var client = new Servers.Vega.FileService.FileServiceClient(channel);

            var channel = new Grpc.Core.Channel("localhost:5005", SslCredentials.Insecure);
            var client = new Servers.Vega.FileService.FileServiceClient(channel);

            var result = await client.DownloadFileAsync(new Servers.Vega.DownloadRequest
            {
                PostId = postId
            });

           
            return File(result.Content.ToArray(), result.ContentType);
        }

        public async Task<IActionResult> OnPostAsync(int postId)
        {
            var likeChannel = new Grpc.Core.Channel("localhost:5007", SslCredentials.Insecure);
            var likeClient = new Liker.LikerClient(likeChannel);

            var reply = await likeClient.AddImageLikeAsync(new PostIdRequest()
            {
                PostId = postId,
                UserId = User.GetUserId(),
                //Status = PostIdRequest.Types.Status.Like
            });

            IsLiked = true;

            return new RedirectToPageResult("",new{postId = postId});
        }
    }
}
