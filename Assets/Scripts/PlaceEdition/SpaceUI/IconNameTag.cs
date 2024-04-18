using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class IconNameTag : MonoBehaviour
{
    public Transform target; // 角色的Transform
    public Vector3 offset = new Vector3(0, 2.0f, 0); // 名字标签的偏移量
    public string userName;
    public Sprite userIcon;
    public User user;
    private RectTransform rectTransform;

    TMP_Text tmp_text;
    Image icon;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // 获取RectTransform组件
        tmp_text = GetComponentInChildren<TMP_Text>(); // 获取TextMeshPro组件
        icon = GetComponentInChildren<Image>(); // 获取TextMeshPro组件

        tmp_text.text = userName; // 设置文本
        icon.sprite = userIcon;
        StartCoroutine(UpdateName());
    }

    // 每秒检测更新
    IEnumerator UpdateName()
    {
        while (true)
        {
            if (target != null)
            {
                tmp_text.text = user.Name; // 设置文本
                icon.sprite = user.userIcon;
            }
            yield return new WaitForSeconds(1);
        }
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
