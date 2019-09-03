using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Application.Posts.Models
{
    public class PostAddModel
    {
        public string Body { get; set; }
        public byte[] Content { get; set; }
        public string UserId { get; set; }
        public string MimeType { get; set; }
    }
}
