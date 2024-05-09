using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpTips : TipsBase
{
    public float openPos,closePos;

    public TMP_Text userNameText,tipsText;

    public Image userIcon;
    
    public GameObject[] starts;
    public float openRotationZ;
    public override void SetData(TipsItem tips)
    {
        for (int i = 0; i < starts.Length; i++)
        {
            starts[i].SetActive(false);
        }
        int level = int.Parse(tips.value);
        int start = level / 10;
        for (int i = 0; i < start; i++)
        {
            starts[i].SetActive(true);
        }
        userNameText.text = tips.userName;
        tipsText.text = tips.text;
        userIcon.sprite = tips.icon;
        
    }

    public override void MoveTipsPanel(bool isOn = true)
    {
        
        if (isOn)
        {
            (transform as RectTransform).DOAnchorPosX(openPos, 0.7f);
            (transform as RectTransform).DOLocalRotate(new Vector3(0,0,openRotationZ), 3.5f);
        }
        else
        {
            (transform as RectTransform).DOAnchorPosX(closePos, 0.6f);
            (transform as RectTransform).DOLocalRotate(new Vector3(0,0,0), 0.8f);
        }
    }
}
