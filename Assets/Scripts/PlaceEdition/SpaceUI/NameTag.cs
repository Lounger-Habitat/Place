using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NameTag : MonoBehaviour
{
    public Transform target; // 角色的Transform
    public Vector3 offset = new Vector3(0, 2.0f, 0); // 名字标签的偏移量
    public string go_name; // 名字标签的文本
    private RectTransform rectTransform;

    TMP_Text tmp_text;

    public Image LackIcon;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // 获取RectTransform组件
        tmp_text = GetComponentInChildren<TMP_Text>(); // 获取TextMeshPro组件
        tmp_text.text = go_name;// 设置文本
        LackIcon.gameObject.SetActive(false);
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

            tmp_text.text = go_name;
        }
    }

    [ContextMenu("sparkle")]
    public void SparkleOn()
    {
        if (LackIcon.gameObject.activeSelf)
        {
            return;
        }
        LackIcon.gameObject.SetActive(true);
        LackIcon.DOFade(1f, 1f).SetLoops(-1, LoopType.Restart);
        LackIcon.transform.DOScale(Vector3.one * 1.2f, 1f).SetLoops(-1, LoopType.Restart);
    }

    public void SparkleOff()
    {
        if (!LackIcon.gameObject.activeSelf)
        {
            return;
        }
        LackIcon.DOKill();//关闭所有变化
        LackIcon.gameObject.SetActive(false);
    }
}
