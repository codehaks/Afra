using Portal.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Domain.Entities
{
    public class Post: ITimeCreated, IUserInfo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Body { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}
