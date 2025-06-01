using Newtonsoft.Json;
using System.Collections.Generic;

public class GroqRequest
{
    [JsonProperty("model")]
    public string Model { get; set; }

    [JsonProperty("messages")]
    public List<Message> Messages { get; set; }

    [JsonProperty("stream")]
    public bool Stream { get; set; }

    [JsonProperty("response_format")]
    public ResponseFormat Response_Format { get; set; }
}