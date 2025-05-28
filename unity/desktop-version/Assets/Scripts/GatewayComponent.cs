using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.EventSystems;
using System;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class PrincipleDetectionResult
{
    public string PrincipleName;
    public float ConfidenceScore;
    public float ClevernessScore;
    public float EvilScore;
}
public class ResponseFormat
{
    [JsonProperty("type")]
    public string type = "json_object";
}

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

public class ChatRequest
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


public class GatewayComponent : MonoBehaviour
{
    [Header("APiKey")]
    public string apiKey;

    public TextMeshProUGUI response;
    //Reference to the inputfield 
    public TMP_InputField userInput;

    public Action<PrincipleDetectionResult> PrincipleDetected;







    public void Prompt(string message)
    {

        if (message == null || message == string.Empty)
            return;

        StartCoroutine(DetectPrinciple(message));


        this.response.text = "Ladron: " + message + "\n\n";
        StartCoroutine(ForwardRequest(message));

        userInput.text = string.Empty;

        userInput.ActivateInputField();
        if (EventSystem.current.alreadySelecting)
        {
            EventSystem.current.SetSelectedGameObject(userInput.gameObject, null);
        }

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
                this.response.text += "Noctula: " + content;
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response: " + request.downloadHandler.text);
            }
        }
    }


    IEnumerator DetectPrinciple(string playerInput)
    {
        //onResult+=
        string url = "https://api.groq.com/openai/v1/chat/completions";
        string systemPrompt = File.ReadAllText("Assets/Prompts/PrincipleDetectionPrompt.txt");

        List<Message> messageBuilder = new List<Message>
        {
            new Message("system", systemPrompt),
            new Message("user", playerInput)
        };

        var requestBody = new ChatRequest
        {
            Model = "llama3-8b-8192",
            Messages = messageBuilder,
            Stream = false,
            Response_Format = new()
        };

        string jsonBody = JsonConvert.SerializeObject(requestBody);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
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
                Debug.Log("LLM JSON result: " + content);

                try
                {
                    PrincipleDetectionResult result = JsonUtility.FromJson<PrincipleDetectionResult>(content);
                    PrincipleDetected?.Invoke(result);
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to parse LLM response: " + e.Message);
                }
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response: " + request.downloadHandler.text);
            }
        }
    }







}

