using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return Page();
        }
    }
}
