using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Ggpc.Liker.Data
{
    public class LikeModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public bool IsLiked { get; set; }
    }
}
