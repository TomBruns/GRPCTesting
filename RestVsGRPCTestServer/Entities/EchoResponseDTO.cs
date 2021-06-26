using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestVsGRPCTestServer.Entities
{
    // This is an entity class used by the HTTP client
    public class EchoResponseDTO
    {
        // note: we are using the entity defined in the GRPC proto file
        [JsonPropertyName(@"books")]
        public List<BookDTO> Books { get; set; }
    }
}
