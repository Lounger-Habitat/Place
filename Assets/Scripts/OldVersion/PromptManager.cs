// using System.Collections;
// using System.Text;
// using UnityEngine;
// using UnityEngine.Networking;

// public class PromptManager : MonoBehaviour
// {
//     public static PromptManager Instance { get; private set; }
//     public DanmakuManager danmakuManager;

//     void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             // DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     private readonly string apiKey = "YOUR_API_KEY";
//     private readonly string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent";



//     public void GenerateInstruction(string p)
//     {
//         LMRequestData requestData = new LMRequestData
//         {
//             contents = new LMRequestData.Content[]
//             {
//                 new LMRequestData.Content
//                 {
//                     parts = new LMRequestData.Content.Part[]
//                     {
//                         new LMRequestData.Content.Part
//                         {
//                             text = p
//                         }
//                     }
//                 }
//             }
//         };

//         string json = JsonUtility.ToJson(requestData);
//         StartCoroutine(PostRequest(apiUrl, json));
//     }

//     IEnumerator PostRequest(string url, string json)
//     {
//         var uri = $"{url}?key={apiKey}";
//         var request = new UnityWebRequest(uri, "POST");
//         byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
//         request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//         request.downloadHandler = new DownloadHandlerBuffer();
//         request.SetRequestHeader("Content-Type", "application/json");

//         yield return request.SendWebRequest();

//         if (request.result != UnityWebRequest.Result.Success)
//         {
//             Debug.Log(request.error);
//         }
//         else
//         {
//             string resjson = request.downloadHandler.text;
//             ApiResponse apiResponse = JsonUtility.FromJson<ApiResponse>(resjson);

//             // 例如，提取第一个候选项的第一个部分的文本
//             if (apiResponse.candidates != null && apiResponse.candidates.Length > 0)
//             {
//                 Candidate firstCandidate = apiResponse.candidates[0];
//                 if (firstCandidate.content != null && firstCandidate.content.parts != null && firstCandidate.content.parts.Length > 0)
//                 {
//                     string text = firstCandidate.content.parts[0].text;
//                     Debug.Log("Response: " + text);
//                     danmakuManager.AddNewDanmaku("Response: " + text);
//                 }
//             }
//         }
//     }
// }




// [System.Serializable]
// public class ApiResponse
// {
//     public Candidate[] candidates;
//     public PromptFeedback promptFeedback;
// }

// [System.Serializable]
// public class Candidate
// {
//     public Content content;
//     public string finishReason;
//     public int index;
//     public SafetyRating[] safetyRatings;
// }

// [System.Serializable]
// public class Content
// {
//     public Part[] parts;
//     public string role;
// }

// [System.Serializable]
// public class Part
// {
//     public string text;
// }

// [System.Serializable]
// public class SafetyRating
// {
//     public string category;
//     public string probability;
// }

// [System.Serializable]
// public class PromptFeedback
// {
//     public SafetyRating[] safetyRatings;
// }


// [System.Serializable]
// public class LMRequestData
// {
//     public Content[] contents;

//     [System.Serializable]
//     public class Content
//     {
//         public Part[] parts;

//         [System.Serializable]
//         public class Part
//         {
//             public string text;
//         }
//     }
// }
