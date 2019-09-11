using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Ggpc.Liker.Data;

namespace Server.Ggpc.Liker
{
    public class LikerService : Liker.LikerBase
    {
        private readonly ILogger<LikerService> _logger;
        private readonly LikeDbContext _likeDbContext;

        public LikerService(ILogger<LikerService> logger , LikeDbContext likeDbContext)
        {
            _logger = logger;
            _likeDbContext = likeDbContext;
        }

        //public override async Task<TotalLikesReply> GetLikes(PostIdRequest request, ServerCallContext context)
        //{
        //  var totalLikes= await  _likeDbContext.Likes.FindAsync(request.PostId);
        //  var response = new TotalLikesReply()
        //  {
            
        //  };
        //}


    }
}
