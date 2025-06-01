using Newtonsoft.Json;

public class Message
{
    [JsonProperty("role")]
    public string Role { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }

    public Message(string role, string content)
    {
        Role = role;
        Content = content;
    }
}