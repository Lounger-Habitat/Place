using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LikeTips : TipsBase
{
    public float openPos,closePos;

    public TMP_Text userNameText,tipsText;

    public Image userIcon;
    //点赞面板
    public override void SetData(TipsItem tips)
    {
        userNameText.text = tips.userName;
        tipsText.text = tips.text;
        userIcon.sprite = tips.icon;
    }

    public override void MoveTipsPanel(bool isOn = true)
    {
        
        if (isOn)
        {
            (transform as RectTransform).DOAnchorPosX(openPos, 0.7f);
            //(transform as RectTransform).DOLocalRotate(new Vector3(0,0,8.5f), 3.5f);
        }
        else
        {
            (transform as RectTransform).DOAnchorPosX(closePos, 0.6f);
            //(transform as RectTransform).DOLocalRotate(new Vector3(0,0,0), 0.8f);
        }
    }
}
