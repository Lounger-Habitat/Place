using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
public class DiffusionManager : MonoBehaviour
{
    public static DiffusionManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Texture2D cacheTexture;
    private byte[] cachePng;

    public delegate void ImageLoaded(Texture2D texture,string finishReason, long seed,int x,int y);
    public event ImageLoaded OnImageLoaded;

    public int samples = 1;


    private readonly string apiURL = "https://api.stability.ai/v1/generation/{engine_id}/text-to-image";
    private readonly string engineID = "stable-diffusion-xl-1024-v1-0"; // 替换为具体的engineID
  
    [SerializeField] private string requestBody;

    private readonly string apiKey = "YOUR_API_KEY";
    public void GenerateImage(int x ,int y ,string p)
    {
        string apiUrl = apiURL.Replace("{engine_id}", engineID);
        Debug.Log("生成图片 : " + p);
        SDRequestData requestBodyData = new SDRequestData
        {
            cfg_scale = 7,
            clip_guidance_preset = "FAST_BLUE",
            height = 1024,
            width = 1024,
            samples = samples,
            steps = 30,
            text_prompts = new[]
            {
                new TextPrompt { text = p, weight = 1.0f }
            }
        };
        requestBody = JsonUtility.ToJson(requestBodyData, true);
        StartCoroutine(PostRequest(apiUrl, requestBody,x,y));
    }
    private Texture2D ConvertBytesToTexture2D(byte[] imageBytes)
    {
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(imageBytes))
        {
            return texture;
        }
        else
        {
            Debug.LogError("Could not load texture from byte array!");
            return null;
        }
    }

    IEnumerator PostRequest(string url, string json,int x , int y)
    {
        var uri = url;
        var request = new UnityWebRequest(uri, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);
        request.SetRequestHeader("Accept", "application/json");

        Debug.Log($"Sending request to {url} with body: {json}");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Error Detail: " + request.downloadHandler.text); // 详细的错误信息通常包含在这里
        }
        else
        {
            HandleResponse(request.downloadHandler.text,x,y);
        }
    }

    private string GetHeaderValue(UnityWebRequest request, string headerName)
    {
        string value;
        if (request.GetResponseHeaders().TryGetValue(headerName, out value))
        {
            return value;
        }
        else
        {
            Debug.LogError("Header not found: " + headerName);
            return null;
        }
    }

    private Texture2D Base64ToTexture2D(string base64)
    {
        byte[] imageBytes = Convert.FromBase64String(base64);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(imageBytes))
        {
            return texture;
        }
        else
        {
            Debug.LogError("Could not load texture from base64!");
            return null;
        }
    }

    void HandleResponse(string jsonResponse,int x,int y)
    {
        JObject responseObj = JObject.Parse(jsonResponse);
        JArray artifactsArray = (JArray)responseObj["artifacts"];

        if (artifactsArray != null && artifactsArray.Count > 0)
        {
            if (samples == 1) {
                foreach (JObject artifact in artifactsArray)
                {
                    string base64Image = artifact["base64"].ToString();
                    string finishReason = artifact["finishReason"].ToString();
                    long seed = (long)artifact["seed"];
                    StartCoroutine(LoadImage(base64Image, finishReason, seed,x,y));
                }
            }else{
                foreach (JArray artifactGroup in artifactsArray)
                {
                    foreach (JObject artifact in artifactGroup)
                    {
                        string base64Image = artifact["base64"].ToString();
                        string finishReason = artifact["finishReason"].ToString();
                        long seed = (long)artifact["seed"];
                        StartCoroutine(LoadImage(base64Image, finishReason, seed,x,y));
                    }
                }

            }

        }
        else
        {
            Debug.LogError("No artifacts found in the response");
        }
    }

    IEnumerator LoadImage(string imageData,string finishReason, long seed,int x,int y)
    {
        byte[] imageBytes = Convert.FromBase64String(imageData);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(imageBytes))
        {
            Debug.Log("Image Loaded Successfully");
            OnImageLoaded?.Invoke(texture,finishReason,seed,x,y);
        }
        else
        {
            Debug.LogError("Failed to load image");
        }
        string filePath = $"Assets/Images/sd_{DateTime.Now.ToString("yyyyMMddHHmmss")}.png";
        System.IO.File.WriteAllBytes(filePath, imageBytes);

        yield return null;
    }
}




[Serializable]
public class SDRequestData
{
    public int cfg_scale;
    public string clip_guidance_preset;
    public int height;
    public int width;
    public int samples;
    public int steps;
    public TextPrompt[] text_prompts;
}

[Serializable]
public class TextPrompt
{
    public string text;
    public float weight;
}

[Serializable]
public class ResponseData
{
    public Artifact[][] artifacts;
}

[Serializable]
public class Artifact
{
    public string base64;
    public string finishReason;
    public int seed;
}