using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Unity.Sentis.Layers;

public class IconNameTag : MonoBehaviour
{
    public Transform target; // 角色的Transform
    public Vector3 offset = new Vector3(0, 2.0f, 0); // 名字标签的偏移量
    public string go_Name;
    public Sprite go_Icon;
    public User user;
    private RectTransform rectTransform;
    public TMP_Text name_text;
    public TMP_Text title_text;
    string title = "";
    public Image icon;

    RectTransform maskRect;



    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // 获取RectTransform组件
        // name_text = GetComponentInChildren<TMP_Text>(); // 获取TextMeshPro组件
        // title_text = transform.GetChild(1).GetComponent<TMP_Text>();
        maskRect = transform.GetChild(0).GetComponent<RectTransform>();
        // icon = transform.GetChild(0).GetChild(0).GetComponent<Image>(); // 获取TextMeshPro组件
        if (user.Camp == 1)
        {
            name_text.color = new Color32(174, 255, 255, 255);
        }
        else
        {
            name_text.color = new Color32(88, 22, 222, 255);
        }

        name_text.text = go_Name; // 设置文本
        icon.sprite = go_Icon;
        StartCoroutine(UpdateName());
    }

    // 每秒检测更新
    IEnumerator UpdateName()
    {
        while (true)
        {
            if (target != null)
            {
                switch (user.level)
                {
                    case < 10:
                        title = "新手"; // 白
                        title_text.color = new Color32(255, 255, 255, 255);
                        break;
                    case < 20:
                        title = "学徒"; // 绿
                        title_text.color = new Color32(0, 255, 0, 255);
                        break;
                    case < 30:
                        title = "画师"; // 蓝
                        title_text.color = new Color32(0, 0, 255, 255);
                        break;
                    case < 50:
                        title = "画家"; // 紫
                        title_text.color = new Color32(255, 0, 255, 255);
                        break;
                    case < 100:
                        title = "画圣"; // 橙
                        title_text.color = new Color32(255, 165, 0, 255);
                        break;
                    default:
                        title = "画尊"; // 红
                        title_text.color = new Color32(255, 0, 0, 255);
                        break;
                }
                title_text.text = title;
                name_text.text = $"{user.Name}"; // 设置文本
                icon.sprite = user.userIcon;
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void UpdateIconRect(float sizeDelta)
    {
        float magicNum = 1.2f; // 角色身高
        float powerScale = sizeDelta; // 1 + sizeDelta;
        //maskrect width height
        // maskRect.sizeDelta = new Vector2(1 + sizeDelta, 1 + sizeDelta);
        offset = new Vector3(0, 2.5f + magicNum * powerScale + (2.7f * powerScale / 2), 0);
        icon.rectTransform.parent.localScale = new Vector3(1 + powerScale, 1 + powerScale, 1);
        title_text.rectTransform.localScale = new Vector3(1 + powerScale / 2, 1 + powerScale / 2, 1);
        title_text.rectTransform.localPosition = new Vector3(0, 0.1f - powerScale/2, 0);
        name_text.rectTransform.localScale = new Vector3(1 + powerScale / 2, 1 + powerScale / 2, 1);
        name_text.rectTransform.localPosition = new Vector3(0, -0.8f - powerScale, 0);
        // rectTransform.localScale = new Vector3(1 + powerScale, 1 + powerScale, 1);
    }

    void Update()
    {
        if (target != null)
        {
            // 更新UI元素的世界位置
            rectTransform.position = target.position + offset;

            // 可选：使UI元素始终朝向摄像机
            // rectTransform.LookAt(Camera.main.transform);
            // rectTransform.rotation = Camera.main.transform.rotation;

        }
    }
}
