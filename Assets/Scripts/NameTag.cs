using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameTag : MonoBehaviour
{
    public Transform target; // 角色的Transform
    public Vector3 offset = new Vector3(0, 2.0f, 0); // 名字标签的偏移量
    public string name; // 名字标签的文本
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // 获取RectTransform组件
        TMP_Text tmp_text = GetComponentInChildren<TMP_Text>(); // 获取TextMeshPro组件
        tmp_text.text = name; // 设置文本
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
    }
}
