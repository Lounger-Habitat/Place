using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageBubbleTag : MonoBehaviour
{
    public Transform target; // 角色的Transform
    public Vector3 offset = new Vector3(0, 3.0f, 0); // 名字标签的偏移量
    public string message; // 名字标签的文本
    private RectTransform rectTransform;

    private float startTime;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // 获取RectTransform组件
        TMP_Text tmp_text = GetComponentInChildren<TMP_Text>(); // 获取TextMeshPro组件
        tmp_text.text = message; // 设置文本

        startTime = Time.time;
    }

    void Update()
    {
        if (target != null)
        {
            // 更新UI元素的世界位置
            rectTransform.position = target.position + offset;

            // 可选：使UI元素始终朝向摄像机
            // rectTransform.LookAt(Camera.main.transform);
            rectTransform.rotation = Camera.main.transform.rotation;
        }

        float elapsedTime = Time.time - startTime;
        
        if (elapsedTime > 3.0f)
        {
            Destroy(gameObject);
        }
    }
}
