using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtFrame : MonoBehaviour
{

    // 定义开始和结束颜色
    public Color startColor = Color.red;
    public Color endColor = Color.blue;

    // 颜色变化的周期
    public float duration = 3.0f;

    Image image;

    // 内部时间跟踪器
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        // 计算颜色插值
        float t = Mathf.PingPong(time / duration, 1.0f);
        Color currentColor = Color.Lerp(startColor, endColor, t);

        // 获取当前 GameObject 的 Renderer 组件
        image.color = currentColor;

    }
}
