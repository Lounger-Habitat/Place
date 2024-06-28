using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DaGeEnterTips : TipsBase
{
    public RectTransform topImgRect;
    public RectTransform textImageRect;
    public RectTransform userImage;
    
    private Vector2[] imagePos = new[]
    {
        new Vector2(-575,469),
        new Vector2(-747,300),
        new Vector2(-468,264),
        new Vector2(-540,59),
        new Vector2(-747,26),
        new Vector2(-555,160),
        new Vector2(-726,-249),
        new Vector2(-575,-439)
    };

    private Vector2[] imageClosePos = new[]
    {
        new Vector2(-2152,874),
        new Vector2(-2127,1155),
        new Vector2(-2246,-604),
        new Vector2(-2193,-324),
        new Vector2(-2600,-722),
        new Vector2(-2220,557),
        new Vector2(-2258,297),
        new Vector2(-2504,872)
    };
    public RectTransform[] singleImage;
    public RectTransform tenImage;
    public RectTransform threeImage;

    public GameObject BG;
    public override void MoveTipsPanel(bool isOn = true)
    {
        if (isOn)
        {
            BG.SetActive(true);
            AudioManager.Instance.PlayShoutDaGeEnter();
            topImgRect.DOAnchorPos(new Vector2(0,230), 4f).SetEase(Ease.Linear);
            userImage.DOAnchorPos(new Vector2(500,945), 4f).SetEase(Ease.Linear);
        }
        else
        {
            for (int i = 0; i < singleImage.Length; i++)
            {
                singleImage[i].anchoredPosition = imageClosePos[i];
            }

            tenImage.anchoredPosition = threeImage.anchoredPosition = new Vector2(-2400, 180);
            topImgRect.anchoredPosition = new Vector2(2600, 230);
            textImageRect.anchoredPosition = new Vector2(2600, 180);
            userImage.anchoredPosition = new Vector2(3100, 945);
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
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < singleImage.Length; i++)
        {
            singleImage[i].DOAnchorPos(imagePos[i], 0.5f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1f);
        threeImage.DOAnchorPos(new Vector2(0, 180), 0.8f);
        yield return new WaitForSeconds(0.6f);
        tenImage.DOAnchorPos(new Vector2(0, 180), 0.8f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1f);
        textImageRect.DOAnchorPos(new Vector2(103, 180), 2f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(4.5f);
        MoveTipsPanel(false);
    }
}
