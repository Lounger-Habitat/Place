using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class TipsPanel : MonoBehaviour
{
    //应该把每个Tips单独写成一个类，目前先这么写
    public RectTransform messageTipsPanel;
    public RectTransform giftTipsPanel;

    private Dictionary<TipsType, TipsBase> tipsPanels = new Dictionary<TipsType, TipsBase>();

    private MessageTips tipsleft, tipsright;
#if UNITY_EDITOR
    private void Start()
    {
        Init();
    }
#endif
    public void Init()
    {
        //rectTransform = transform as RectTransform;
        //messageTipsPanel.anchoredPosition = new Vector2(-260, -467);
        tipsQueue.Clear();
        //初始化所有提示面板
        var tips = GetComponentsInChildren<TipsBase>();
        foreach (var item in tips)
        {
            tipsPanels[item.panelType] = item;
        }

        tipsleft = transform.Find("Tips").GetComponent<MessageTips>();
        tipsright = transform.Find("TipsRight").GetComponent<MessageTips>();
        tipsleft.Init(this);
        tipsright.Init(this);
    }

    private void SetData(TipsItem tips)
    {
        switch (tips.tipsType)
        {
            case TipsType.messagePanel:
                messageTipsPanel.GetChild(1).GetComponent<TMP_Text>().text = tips.userName;
                messageTipsPanel.GetChild(2).GetComponent<TMP_Text>().text = tips.text;
                break;
            case TipsType.giftAttackPanel:
                giftTipsPanel.Find("Background").Find("Text_Name").GetComponent<TMP_Text>().text = tips.userName;
                giftTipsPanel.Find("Bottom").Find("ListFrame05_Light_Green").Find("Text").GetComponent<TMP_Text>()
                    .text = tips.userName;
                break;
        }
    }

    private Queue<TipsItem> tipsQueue = new Queue<TipsItem>();
    private Queue<TipsItem> tipsQueueright = new Queue<TipsItem>();
    public Queue<TipsItem> tipsMessageQueue = new Queue<TipsItem>();
    public Queue<TipsItem> tipsMessageQueueright = new Queue<TipsItem>();

    public void AddTips(TipsItem tip)
    {
        if (tip.tipsType == TipsType.messagePanel)
        {
            if (tip.isLeft)
            {
                lock (tipsMessageQueue)
                {
                    //首先入列
                    tipsMessageQueue.Enqueue(tip);
                }
            }
            else
            {
                lock (tipsMessageQueueright)
                {
                    //首先入列
                    tipsMessageQueueright.Enqueue(tip);
                }
            }
            

            tipsleft.ShowTips();
            tipsright.ShowTips();
        }
        else
        {
            if (tip.isLeft)
            {
                lock (tipsQueue)
                {
                    //首先入列
                    tipsQueue.Enqueue(tip);
                }

                //检查是否在进行弹出提示，如果在进行弹出提示就不用管了
                if (!isShowTips)
                {
                    //否则就要启动弹出动画
                    StartCoroutine(ShowTipsAni());
                }
            }
            else
            {
                lock (tipsQueueright)
                {
                    //首先入列
                    tipsQueueright.Enqueue(tip);
                }

                //检查是否在进行弹出提示，如果在进行弹出提示就不用管了
                if (!isShowTipsright)
                {
                    //否则就要启动弹出动画
                    StartCoroutine(ShowTipsAniRight());
                }
            }
            
        }
    }

    WaitForSeconds wait = new(4.6f);
    private bool isShowTips = false;
    private bool isShowTipsright = false;
    private TipsItem nowData;

    IEnumerator ShowTipsAni()
    {
        //将标志位置为true
        isShowTips = true;
        //检查队列中是否还有元素
        while (tipsQueue.Any())
        {
            lock (tipsQueue)
            {
                nowData = tipsQueue.Dequeue();
            }

            var panel = tipsPanels[nowData.tipsType];
            panel.SetData(nowData);
            panel.MoveTipsPanel();
            yield return wait;
            panel.MoveTipsPanel(false);
            yield return new WaitForSeconds(0.8f);
        }

        isShowTips = false;
    }
    
    IEnumerator ShowTipsAniRight()
    {
        //将标志位置为true
        isShowTipsright = true;
        //检查队列中是否还有元素
        while (tipsQueueright.Any())
        {
            lock (tipsQueueright)
            {
                nowData = tipsQueueright.Dequeue();
            }

            var panel = tipsPanels[nowData.tipsType];
            panel.SetData(nowData);
            panel.MoveTipsPanel();
            yield return wait;
            panel.MoveTipsPanel(false);
            yield return new WaitForSeconds(0.8f);
        }

        isShowTipsright = false;
    }
    
    [ContextMenu("message")]
    public void Test()
    {
        AddTips(new TipsItem()
        {
            userName = "全力以赴",
            text = "Add Team",
            tipsType = TipsType.messagePanel
        });
        AddTips(new TipsItem()
        {
            userName = "全力以赴1",
            text = "Add Team1",
            tipsType = TipsType.messagePanel
        });
        AddTips(new TipsItem()
        {
            userName = "全力以赴2222",
            text = "Add Team1",
            tipsType = TipsType.messagePanel
        });
        AddTips(new TipsItem()
        {
            userName = "全力以赴333",
            text = "Add Team22",
            tipsType = TipsType.messagePanel
        });
        AddTips(new TipsItem()
        {
            userName = "全力以赴444",
            text = "Add Team33",
            tipsType = TipsType.messagePanel
        });
    }

    [ContextMenu("Gift1")]
    public void Test1()
    {
        AddTips(new TipsItem()
        {
            userName = "金钱帝国",
            text = "gift hhhhhhh",
            tipsType = TipsType.giftAttackPanel
        });
    }

    [ContextMenu("Gift2")]
    public void Test2()
    {
        AddTips(new TipsItem()
        {
            userName = "金钱帝国",
            text = "gift giftDefensePanel",
            tipsType = TipsType.giftDefensePanel
        });
    }

    [ContextMenu("Gift3")]
    public void Test3()
    {
        AddTips(new TipsItem()
        {
            userName = "金钱帝国",
            text = "颜料增加",
            tipsType = TipsType.giftDrawPanel
        });
    }
}

public class TipsItem
{
    //我这里边还少一个东西应该
    public string userName;
    public string text;
    public Sprite icon;
    public TipsType tipsType;
    public string value;
    public bool isLeft = true;
}

//这是提示的类型，
public enum TipsType
{
    messagePanel,
    giftAttackPanel,
    giftDefensePanel,
    giftDrawPanel,
    giftDrawPanelRight,
    likeTipsPanel,
    likeTipsPanelRight,
    levelUpPanel,
    levelUpPanelRight
}