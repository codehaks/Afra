using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portal.Application.Posts;

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
                Content = ms1.ToArray(),
                MimeType = PhotoFile.ContentType,
                UserId = ""
            };

            await _postService.Create(model);

            return RedirectToPage("/index");
        }
    }
}