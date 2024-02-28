using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using DG.Tweening;
public class TipsPanel : MonoBehaviour
{
    RectTransform rectTransform;
    public void Init()
    {
        rectTransform = transform as RectTransform;
        rectTransform.anchoredPosition = new Vector2(-260, -467);
        tipsQueue.Clear();
    }

    private void SetData(TipsItem tips)
    {
        transform.GetChild(1).GetComponent<TMP_Text>().text = tips.userName;
        transform.GetChild(2).GetComponent<TMP_Text>().text = tips.text;
    }

    private Queue<TipsItem> tipsQueue = new Queue<TipsItem>();

    public void AddTips(TipsItem tip)
    {
        lock (tipsQueue)
        {   //首先入列
            tipsQueue.Enqueue(tip);
        }
        //检查是否在进行弹出提示，如果在进行弹出提示就不用管了
        if (!isShowTips)
        {
            //否则就要启动弹出动画
            StartCoroutine(ShowTipsAni());
        }
        
    }

    WaitForSeconds wait = new WaitForSeconds(4.6f);
    private bool isShowTips = false;
    IEnumerator ShowTipsAni()
    {
        //将标志位置为true
        isShowTips = true;
        //检查队列中是否还有元素
        while (tipsQueue.Any())
        {
            lock (tipsQueue)
            {
                var data = tipsQueue.Dequeue();
                SetData(data);
            }
            MoveTipsPanel();
            yield return wait;
        }
        isShowTips = false;
        MoveTipsPanel(false);
    }

    private void MoveTipsPanel(bool isShow = true){
        if (isShow)//如果是打开移动到打开位置
        {
            rectTransform.DOAnchorPosX(60, 1f);
        }
        else
        {
            rectTransform.DOAnchorPosX(-230, 1f);
        }
    }
}

public class TipsItem
{
    public string userName;
    public string text;
    public Sprite icon;
}
