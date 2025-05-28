using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;


public class GatewayComponent : MonoBehaviour
{
    [Header("APiKey")]
    public string apiKey;

    public TextMeshProUGUI response;
    public void Prompt(string message)
    {
        StartCoroutine(ForwardRequest(message));
    }

    IEnumerator ForwardRequest(string message)
    {
        string url = "https://api.groq.com/openai/v1/chat/completions";

        string jsonBody = $@"
        {{
            ""model"": ""llama3-70b-8192"",
            ""messages"": [
                {{
                    ""role"": ""user"",
                    ""content"": ""{message}""
                }}
            ]
        }}";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                JObject parsed = JObject.Parse(response);
                string content = parsed["choices"]?[0]?["message"]?["content"]?.ToString();
                Debug.Log("Response to your request: " + content);
                this.response.text = content;
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response: " + request.downloadHandler.text);
            }
        }
    }
}
