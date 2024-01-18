using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TipsPanel : MonoBehaviour
{
    RectTransform rectTransform;
    public void Init()
    {
        rectTransform = transform as RectTransform;
        rectTransform.anchoredPosition = new Vector2(-230, -467);
        tipsQueue.Clear();
    }

    public void SetData(TipsItem tips)
    {
        transform.GetChild(1).GetComponent<TMP_Text>().text = tips.userName;
        transform.GetChild(2).GetComponent<TMP_Text>().text = tips.text;
    }

    private Queue<TipsItem> tipsQueue = new Queue<TipsItem>();

    public void AddTips(TipsItem tip)
    {
        //首先入列
        tipsQueue.Enqueue(tip);
        //检查是否在进行弹出提示，如果在进行弹出提示就不用管了
        if (!isShowTips)
        {
            //否则就要启动弹出动画
        }
    }

    WaitForSeconds wait = new WaitForSeconds(2.6f);
    private bool isShowTips = false;
    IEnumerator ShowTipsAni()
    {
        //将标志位置为true
        isShowTips = true;
        //检查队列中是否还有元素
        while (tipsQueue.Any())
        {
            var data = tipsQueue.Dequeue();
            SetData(data);
            yield return wait;
        }
        isShowTips = false;
    }

    private void MoveTipsPanel(bool isShow){
        
    }
}

public class TipsItem
{
    public string userName;
    public string text;
    public Sprite icon;
}
