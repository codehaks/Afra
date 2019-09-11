using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Server.Ggpc.Liker
{
    public class LikerService : Liker.LikerBase
    {
        private readonly ILogger<LikerService> _logger;
        public LikerService(ILogger<LikerService> logger)
        {
            _logger = logger;
        }

        //public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        //{
        //    return Task.FromResult(new HelloReply
        //    {
        //        Message = "Hello " + request.Name
        //    });
        //}
    }
}
