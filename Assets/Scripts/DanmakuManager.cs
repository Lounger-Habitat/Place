using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class DanmakuManager : MonoBehaviour
{
    public RectTransform contentPanel; // Content 面板的 RectTransform
    public TMP_Text danmakuPrefab; // 弹幕预制件

    void Update() {
        // 按下h键发送弹幕
        if (Input.GetKeyDown(KeyCode.H))
        {
            AddNewDanmaku("user:xxx!");
        }

    }
    // 添加新弹幕到弹幕栏
    public void AddNewDanmaku(string message)
    {
        TMP_Text newDanmaku = Instantiate(danmakuPrefab, contentPanel);
        newDanmaku.text = message;
        StartCoroutine(ScrollToBottom());
    }

    // 协程来等待下一帧，确保滚动视图更新后再滚动到底部
    private IEnumerator ScrollToBottom()
    {
        // 等待下一帧，确保所有 UI 元素都已更新
        yield return new WaitForEndOfFrame();

        // 计算新的滚动位置
        var scrollRect = contentPanel.GetComponentInParent<ScrollRect>();
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }
}






// 滚动弹幕
// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections.Generic;
// using TMPro;
// using UnityEditor.Rendering.Universal;

// public class DanmakuManager : MonoBehaviour
// {
//     public TMP_Text danmakuTextPrefab; // 弹幕文本预制件
//     public RectTransform danmakuContainer; // 弹幕容器
//     public float scrollSpeed = 50f; // 弹幕滚动速度
//     public float messageSpacing = 20f; // 消息间距

//     private List<TMP_Text> danmakus = new List<TMP_Text>();
//     private Queue<string> messages = new Queue<string>();

//     void Update()
//     {
//         // 更新所有弹幕的位置
//         foreach (var danmaku in danmakus)
//         {
//             danmaku.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
//         }

//         // 移除超出容器的弹幕
//         RemoveOffscreenDanmakus();
//     }

//     string dm = "user:xxx!";

//     [ContextMenu("Add Test Message")]
//     public void AddMessage()
//     {
//         CreateDanmaku(dm);
//     }

//     void CreateDanmaku(string text)
//     {
//         TMP_Text newDanmaku = Instantiate(danmakuTextPrefab, danmakuContainer);
//         newDanmaku.text = text;
        
//         // 设置新弹幕的位置
//         float newYPosition = (danmakus.Count > 0) ? danmakus[danmakus.Count - 1].transform.localPosition.y - messageSpacing : 0;
//         newDanmaku.transform.localPosition = new Vector3(0, newYPosition, 0);

//         danmakus.Add(newDanmaku);
//     }

//     void RemoveOffscreenDanmakus()
//     {
//         for (int i = danmakus.Count - 1; i >= 0; i--)
//         {
//             if (danmakus[i].transform.localPosition.y > danmakuContainer.rect.height)
//             {
//                 Destroy(danmakus[i].gameObject);
//                 danmakus.RemoveAt(i);
//             }
//         }
//     }
// }
