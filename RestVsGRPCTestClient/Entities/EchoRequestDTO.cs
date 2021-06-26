using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestVsGRPCTestClient.Entities
{   
    // This is an entity class used by the HTTP client
    public class EchoRequestDTO
    {
        // note: we are using the entity defined in the GRPC proto file
        [JsonPropertyName(@"books")]
        public List<BookDTO> Books { get; set; }
    }
}
