using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using DG.Tweening;
using UnityEngine.UI;

public class GiftTips : TipsBase
{

    public RectTransform dataRectTransform;
    public RectTransform iconRectTransform;

    public Image iconImage;
    
    public Sprite[] sprites;//保存一些礼物ICON，护盾、画画、龙卷风、雷电......
    
    public float openPos,closePos;

    private bool needRotation =false;
   public override void SetData(TipsItem tips)
    {
        transform.Find("Background").Find("Text_Name").GetComponent<TMP_Text>().text = tips.userName;
        transform.Find("Background").Find("ListFrame05_Light_Green").Find("Text").GetComponent<TMP_Text>().text = tips.text;
        iconImage.transform.Find("Image").gameObject.SetActive(false);
        needRotation = false;
        switch (tips.text)
        {
            case "急速神行":
                iconImage.sprite = sprites[4];
                break;
            case "颜料爆发":
                iconImage.sprite = sprites[0];
                needRotation = true;
                break;
            case "风之束缚":
                iconImage.sprite = sprites[5];
                break;
            case "绝对防御":
                iconImage.sprite = sprites[3];
                iconImage.transform.Find("Image").gameObject.SetActive(true);
                iconImage.transform.Find("Image").GetComponent<Image>().sprite =sprites[2];
                break;
            case "颜料核弹":
                iconImage.sprite = sprites[0];
                needRotation = true;
                break;
            case "掌控雷电":
                iconImage.sprite = sprites[1];
                break;
        }
    }

    public override void MoveTipsPanel(bool isOn = true)
    {
        if (panelType is TipsType.giftDrawPanel or TipsType.giftDrawPanelRight)
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
                if(needRotation)iconRectTransform.DORotate(new Vector3(0, 0, 360 * 2f), 2.4f,RotateMode.FastBeyond360).SetEase(Ease.OutCubic);
                iconRectTransform.DOScale(new Vector3(3, 3, 3), 1.5f).SetEase(Ease.OutElastic).OnComplete(() =>
                {
                    iconRectTransform.DOScale(Vector3.one, 1.5f).SetEase(Ease.OutExpo);
                });
            });
        }
        else
        {
            (transform as RectTransform).DOAnchorPosX(closePos, 0.5f);
        }
    }
}
