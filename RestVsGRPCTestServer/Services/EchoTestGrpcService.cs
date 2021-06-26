using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Grpc.Core;

namespace RestVsGRPCTestServer.Services
{
    // =========================================
    // This is a Service exposed via GRPC
    // =========================================
    public class EchoTestGrpcService : EchoTester.EchoTesterBase
    {
        private readonly ILogger<EchoTestGrpcService> _logger;

        // Constructor
        public EchoTestGrpcService(ILogger<EchoTestGrpcService> logger)
        {
            _logger = logger;
        }

        // =========================================
        // Just echo back the request payload
        // =========================================
        public override Task<EchoResponseDTO> Echo(EchoRequestDTO request, ServerCallContext context)
        {
            var echoResponse = new EchoResponseDTO();
            echoResponse.Books.AddRange(request.Books);

            return Task.FromResult(echoResponse);
        }
    }
}
