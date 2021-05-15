using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cabother.Organizer.Api.ViewModels
{
    public class ErrorViewModel
    {
        [JsonPropertyName("status")]
        public int? Status { get; set; }
        
        [JsonPropertyName("userMessage")]
        public string UserMessage { get; set; }
        
        [JsonPropertyName("detail")]
        public string Detail { get; set; }
        
        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
        
        [JsonExtensionData]
        public Dictionary<string, object> Extensions { get; set; } =
            new Dictionary<string, object>(StringComparer.Ordinal);
    }
}