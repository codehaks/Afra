using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace Server.Ggpc.Liker.Data
{
    public class LikeModel:BaseEntity
    {
        public int PostId { get; set; }
        public string UserId { get; set; }
        public bool IsLiked { get; set; }
    }
}
