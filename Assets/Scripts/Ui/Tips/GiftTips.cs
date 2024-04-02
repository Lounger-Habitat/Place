using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using DG.Tweening;

public class GiftTips : TipsBase
{
   public override void SetData(TipsItem tips)
    {
        transform.Find("Background").Find("Text_Name").GetComponent<TMP_Text>().text = tips.userName;
        transform.Find("Bottom").Find("ListFrame05_Light_Green").Find("Text").GetComponent<TMP_Text>().text = tips.text;
    }

    public override void MoveTipsPanel(bool isOn = true)
    {
        if (isOn)
        {
            (transform as RectTransform).DOAnchorPosX(540, 0.7f);
            (transform as RectTransform).DOLocalRotate(new Vector3(0,0,8.5f), 3.5f);
        }
        else
        {
            (transform as RectTransform).DOAnchorPosX(-900, 0.6f);
            (transform as RectTransform).DOLocalRotate(new Vector3(0,0,0), 0.8f);
        }
    }
}
