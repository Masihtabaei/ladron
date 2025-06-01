using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class GatewayComponent : MonoBehaviour
{

    public Action<PrincipleDetectionResult> PrincipleDetected;
    public const string ENDPOINT_URL = "https://api.groq.com/openai/v1/chat/completions";

    [Header("API Key")]
    [SerializeField]
    private string _apiKey;

    public IEnumerator ForwardRequest(List<Message> messages, bool jsonRequired, Action<string> callback)
    {
        GroqRequest requestBody = null;
        if (jsonRequired)
        {
            requestBody = new GroqRequest
            {
                Model = "llama3-8b-8192",
                Messages = messages,
                Stream = false,
                Response_Format = new()

            };
        }
        else
        {
            requestBody = new GroqRequest
            {
                Model = "llama3-8b-8192",
                Messages = messages,
                Stream = false,

            };
        }


        string jsonBody = JsonConvert.SerializeObject(requestBody);

        using (UnityWebRequest request = new UnityWebRequest(ENDPOINT_URL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {_apiKey}");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                JObject parsed = JObject.Parse(response);
                string content = parsed["choices"]?[0]?["message"]?["content"]?.ToString();
                Debug.Log("LLM JSON result: " + content);
                callback?.Invoke(content);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response: " + request.downloadHandler.text);
            }
        }
    }

}

