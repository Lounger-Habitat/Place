using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OtherEnterTips : TipsBase
{
    public GameObject BG;
    public RectTransform userImage;
    public RectTransform lightImage;
    public override void MoveTipsPanel(bool isOn = true)
    {
        if (isOn)
        {
            BG.SetActive(true);
            userImage.DOAnchorPos(new Vector2(0, 527), 2f).SetEase(Ease.Linear);
            lightImage.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);

        }
        else
        {
            userImage.anchoredPosition = new Vector2(2600, 527);
            lightImage.DOKill();
            lightImage.localRotation = Quaternion.identity;
            BG.SetActive(false);
        }
    }
    
    [ContextMenu("gogogo")]
    public override void TestMenu()
    {
        StartCoroutine(showTips());
    }

    IEnumerator showTips()
    {
        MoveTipsPanel();
        yield return new WaitForSeconds(5f);
        userImage.DOMoveX(-2600, 1f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(1.1f);
        MoveTipsPanel(false);
    }
}
