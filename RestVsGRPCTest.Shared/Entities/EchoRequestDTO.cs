using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestVsGRPCTestServer.Entities
{
    public class EchoRequestDTO
    {
        [JsonPropertyName(@"books")]
        public List<BookDTO> Books { get; set; }
    }
}
