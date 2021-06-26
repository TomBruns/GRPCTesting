using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestVsGRPCTestServer.Entities
{
    public class BookDTOxx
    {
        [JsonPropertyName(@"title")]
        public string Title { get; set; }
        [JsonPropertyName(@"author")]
        public string Author { get; set; }
    }
}
