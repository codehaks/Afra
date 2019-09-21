using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Persistance;

namespace Portal.Web.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly PortalDbContext _db;

        public CommentController(PortalDbContext db)
        {
            _db = db;
        }
        [Route("api/comment")]
        public IActionResult Index()
        {
            _db.SaveChanges();
            return View();
        }
    }
}