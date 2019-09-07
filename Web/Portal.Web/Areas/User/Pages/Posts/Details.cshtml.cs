using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portal.Domain.Entities;
using Portal.Persistance;

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

        public async Task<IActionResult> OnGet(int postId)
        {
            PostViewModel = await _db.Posts.FindAsync(postId);

            var channel = new Grpc.Core.Channel("localhost:5005", SslCredentials.Insecure);
            var client = new Servers.Vega.FileService.FileServiceClient(channel);

            var result=await client.DownloadFileAsync(new Servers.Vega.DownloadRequest
            {
                PostId = PostViewModel.Id
            });

            return Page();
        }
    }
}
