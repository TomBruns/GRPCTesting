using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using rest = RestVsGRPCTestServer.Entities;

namespace RestVsGRPCTestServer.Controllers
{
    // =========================================
    // This is a standard REST WebAPI controller
    // =========================================
    [Route("api/[controller]")]
    [ApiController]
    public class EchoTestController : Controller
    {
        private readonly ILogger<EchoTestController> _logger;

        /// <summary>
        /// EchoTestController constructor
        /// </summary>
        public EchoTestController(ILogger<EchoTestController> logger)
        {
            _logger = logger;
        }

        // =========================================
        // Just echo back the request payload
        // =========================================
        [HttpPost("echo", Name = "Echo")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EchoResponseDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<rest.EchoResponseDTO>> Echo([FromBody] rest.EchoRequestDTO request)
        {
            var echoResponse = new rest.EchoResponseDTO();
            echoResponse.Books = new List<BookDTO>();
            echoResponse.Books.AddRange(request.Books);

            return Ok(echoResponse);
        }
    }
}
