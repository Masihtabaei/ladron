using Newtonsoft.Json;

public class PrincipleDetectionResult
{

    [JsonProperty("reply")]
    public string reply { get; set; }

    [JsonProperty("trustDifference")]
    public float trustDifference { get; set; }

    [JsonProperty("principle")]
    public float principle { get; set; }

    [JsonProperty("joke")]
    public float joke { get; set; }
}