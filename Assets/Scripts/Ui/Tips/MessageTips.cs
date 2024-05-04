using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class MessageTips : TipsBase
{

    public float waitTime=2.0f;
    
    private bool isShowTips;

    private TipsPanel parentPanel;

    public float closePos, openPos;

    public bool isLeft;

    public Image userIcon;
    public void Init(TipsPanel parent)
    {
        parentPanel = parent;
    }
    public override void SetData(TipsItem tips)
    {
        transform.Find("UserName").GetComponent<TMP_Text>().text = tips.userName;
        transform.Find("Text").GetComponent<TMP_Text>().text = tips.text;
        userIcon.sprite = tips.icon;
    }

    public override void MoveTipsPanel(bool isOn = true)
    {
        if (isOn)//如果是打开移动到打开位置
        {
            (transform as RectTransform).DOAnchorPosX(openPos, 1f);
        }
        else
        {
            (transform as RectTransform).DOAnchorPosX(closePos, 0.7f);
        }
    }
    
    private TipsItem nowData;
    IEnumerator ShowTipsAni()
    {
        //将标志位置为true
        isShowTips = true;
        //检查队列中是否还有元素
        if (isLeft)
        {
            while (parentPanel.tipsMessageQueue.Any())
            {
                lock (parentPanel.tipsMessageQueue)
                {
                    nowData = parentPanel.tipsMessageQueue.Dequeue();
                }

                var panel = this;
                panel.SetData(nowData);
                panel.MoveTipsPanel();
                yield return new WaitForSeconds(waitTime);
                panel.MoveTipsPanel(false);
                yield return new WaitForSeconds(0.8f);
            }
        }
        else
        {
            while (parentPanel.tipsMessageQueueright.Any())
            {
                lock (parentPanel.tipsMessageQueueright)
                {
                    nowData = parentPanel.tipsMessageQueueright.Dequeue();
                }

                var panel = this;
                panel.SetData(nowData);
                panel.MoveTipsPanel();
                yield return new WaitForSeconds(waitTime);
                panel.MoveTipsPanel(false);
                yield return new WaitForSeconds(0.8f);
            }
        }
       

        isShowTips = false;
    }

    public bool ShowTips()
    {
        if (isShowTips)
        {
            return false;
        }

        StopAllCoroutines();
        StartCoroutine(ShowTipsAni());
        return true;

    }
}
