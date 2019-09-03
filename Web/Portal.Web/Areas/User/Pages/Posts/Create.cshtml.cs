using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portal.Application.Posts;
using Portal.Web.Common.Extentions;
using Servers.Vega;

namespace Portal.Web.Areas.User.Pages.Posts
{
    public class CreateModel : PageModel
    {
        private readonly IPostService _postService;
        public CreateModel(IPostService postService)
        {
            _postService = postService;
        }
     
        [BindProperty]
        public IFormFile PhotoFile { get; set; }

        [BindProperty]
        public string Body { get; set; }

        public async Task<IActionResult> OnPost()
        {
            using var ms1 = new MemoryStream();
            await PhotoFile.CopyToAsync(ms1);
            ms1.Position = 0;

            var model = new Application.Posts.Models.PostAddModel
            {
                Body = this.Body,
                UserId = ""
            };

            var postId=await _postService.Create(model);

            #region Upload to Vega

            var channel = new Grpc.Core.Channel("localhost:5005", SslCredentials.Insecure);
            var client = new Servers.Vega.FileService.FileServiceClient(channel);
            
            var result2 = client.UploadFile(new UploadRequest
            {
                Content = ByteString.CopyFrom(ms1.ToArray()),
                ContentType = PhotoFile.ContentType,
                Name = PhotoFile.FileName,
                PostId=postId,
                UserId = User.GetUserId()
            });
            #endregion

            return RedirectToPage("/index");
        }
    }
}