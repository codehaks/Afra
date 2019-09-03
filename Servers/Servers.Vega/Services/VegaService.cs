using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Servers.Vega
{
    public class VegaService : FileService.FileServiceBase
    {
        private readonly ILogger<VegaService> _logger;
        public VegaService(ILogger<VegaService> logger)
        {
            _logger = logger;
        }

        public override Task<UploadResponse> UploadFile(UploadRequest request, ServerCallContext context)
        {
            _logger.LogInformation("File Received ", request.Content.Length);
            var response = new UploadResponse { Status = FileUploadStatus.Success, Message = $"File upload is done => {request.Name} | " + request.Content.Length };
            return Task.FromResult(response);
        }

       
    }
}
