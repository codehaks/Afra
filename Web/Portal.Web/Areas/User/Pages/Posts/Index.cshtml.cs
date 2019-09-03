using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portal.Application.Posts;
using Portal.Application.Posts.Models;

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

        public void OnGet()
        {
            Posts= _postService.GetAll();
        }
    }
}
