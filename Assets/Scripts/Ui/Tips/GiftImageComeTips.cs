using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GiftImageComeTips : TipsBase
{
    public RectTransform userRect;
    public RectTransform imageTop;
    public RectTransform imageBg;
    public GameObject BG;
    
    public override void MoveTipsPanel(bool isOn = true)
    {
        if (isOn)
        {
            BG.SetActive(true);
            userRect.DOLocalMove(new Vector3(-867, 623, 0), 0.8f);
            //DOTween.To(() => imageTop.rect.width, wh => imageTop.rect.Set(0, 0, wh, wh), 2000, 3.5f);
            imageTop.DOScale(Vector3.one*23, 3f).SetEase(Ease.Linear);
        }
        else
        {
            userRect.anchoredPosition = new Vector2(-3500f,623);
            imageTop.localScale = Vector3.one;
            BG.SetActive(false);
        }
    }
    
    [ContextMenu("gogogo")]
    public void TestMenu()
    {
        StartCoroutine(showTips());
    }

    IEnumerator showTips()
    {
        MoveTipsPanel();
        yield return new WaitForSeconds(3.5f);
        userRect.DOLocalMoveX(-3000, 1f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(1.1f);
        MoveTipsPanel(false);
    }
}
