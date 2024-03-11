using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MessageTips : TipsBase
{
    public override void SetData(TipsItem tips)
    {
        transform.GetChild(1).GetComponent<TMP_Text>().text = tips.userName;
        transform.GetChild(2).GetComponent<TMP_Text>().text = tips.text;
    }

    public override void MoveTipsPanel(bool isOn = true)
    {
        if (isOn)//如果是打开移动到打开位置
        {
            (transform as RectTransform).DOAnchorPosX(320, 1f);
        }
        else
        {
            (transform as RectTransform).DOAnchorPosX(-30, 0.7f);
        }
    }
}
