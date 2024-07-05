using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GiftImageBaiTuoTips : TipsBase
{   
    public RectTransform userRect;
    public RectTransform imageRight;
    public RectTransform imageLeft;
    public RectTransform effectImage;
    public GameObject BG;
    public override void MoveTipsPanel(bool isOn = true)
    {
        if (isOn)
        {
            BG.SetActive(true);
            userRect.DOLocalMove(new Vector3(-867, 623, 0), 0.8f);
            imageRight.DOLocalMoveX(0, 1.2f);
            imageLeft.DOLocalMoveX(0, 1.2f);
        }
        else
        {
            userRect.anchoredPosition = new Vector2(-3500f,623);
            imageRight.anchoredPosition = new Vector2(2600, 100);
            imageLeft.anchoredPosition = new Vector2(-2600, 100);
            effectImage.anchoredPosition = new Vector2(-470, -100);
            effectImage.localScale = Vector3.zero;
            BG.SetActive(false);
        }
    }


    public void MoveOut()
    {
        imageRight.DOLocalMoveX(-2600, 0.8f);
        imageLeft.DOLocalMoveX(-2600, 0.8f);
        effectImage.DOLocalMoveX(-3060, 0.8f);
    }
    
    [ContextMenu("gogogo")]
    public override void TestMenu()
    {
        StartCoroutine(showTips());
    }

    IEnumerator showTips()
    {
        MoveTipsPanel();
        yield return new WaitForSeconds(1.1f);
        effectImage.DOScale(Vector3.one, 0.35f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(3.0f);
        MoveOut();
        yield return new WaitForSeconds(1.0f);
        userRect.DOLocalMoveX(-3000, 1f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(1.1f);
        MoveTipsPanel(false);
    }
}
