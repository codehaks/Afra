using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Servers.Vega.Data;

namespace Servers.Vega
{
    public class VegaService : FileService.FileServiceBase
    {
        private readonly ILogger<VegaService> _logger;
        private readonly VegaDbContext _db;

        public VegaService(ILogger<VegaService> logger, VegaDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public override async Task<UploadResponse> UploadFile(UploadRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"File Received: {request.Name} | Size: {request.Content.Length} ");

            _db.Docs.Add(new Doc
            {
                Content = request.Content.ToArray(),
                ContentType = request.ContentType,
                PostId = request.PostId,
                FileName = request.Name,
                UserId = request.UserId
            });

            await _db.SaveChangesAsync();

            // Check if it was successful

            var response = new UploadResponse
            {
                Status = FileUploadStatus.Success,
                Message = $"File upload is done => {request.Name} | " + request.Content.Length
            };
            return response;
        }


    }
}
