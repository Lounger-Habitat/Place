using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using DG.Tweening;

public class GiftTips : TipsBase
{

    public RectTransform dataRectTransform;
    public RectTransform iconRectTransform;
    
   public override void SetData(TipsItem tips)
    {
        transform.Find("Background").Find("Text_Name").GetComponent<TMP_Text>().text = tips.userName;
        transform.Find("Background").Find("ListFrame05_Light_Green").Find("Text").GetComponent<TMP_Text>().text = tips.text;
    }

    public override void MoveTipsPanel(bool isOn = true)
    {
        if (panelType == TipsType.giftDrawPanel)
        {
            MoveTipsGiftDraw(isOn);
            return;
        }
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

    public void MoveTipsGiftDraw(bool isOn = true)
    {
        dataRectTransform.localScale = new Vector3(0.1f,0.1f,0.1f);
        iconRectTransform.localScale = new Vector3(1f, 1f, 1f);
        iconRectTransform.rotation = Quaternion.identity;
        if (isOn)
        {
            (transform as RectTransform).DOAnchorPosX(0, 0.5f).OnComplete(() =>
            {
                dataRectTransform.DOScale(new Vector3(0.8f, 0.8f, 1), 1f).SetEase(Ease.OutElastic);
                iconRectTransform.DORotate(new Vector3(0, 0, 360 * 2f), 2.4f,RotateMode.FastBeyond360).SetEase(Ease.OutCubic);
                iconRectTransform.DOScale(new Vector3(3, 3, 3), 1.5f).SetEase(Ease.OutElastic).OnComplete(() =>
                {
                    iconRectTransform.DOScale(Vector3.one, 1.5f).SetEase(Ease.OutExpo);
                });
            });
        }
        else
        {
            (transform as RectTransform).DOAnchorPosX(-950, 0.5f);
        }
    }
}
