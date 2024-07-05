using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GiftImageTips : TipsBase
{
    public RectTransform userRect;
    public RectTransform iconRect;
    public GameObject BG;

    public RectTransform lineImage, lineImage2;
    public override void MoveTipsPanel(bool isOn = true)
    {
        if (isOn)
        {
            BG.SetActive(true);
            AudioManager.Instance.PlayShout();
            userRect.DOLocalMove(new Vector3(-867, 623, 0), 1.2f);
            iconRect.DOLocalMove(new Vector3(0, 0, 0), 2f);
            iconRect.DOScale(new Vector3(1.3f, 1.3f, 1), 2.3f);
            lineImage.DORotate(new Vector3(0, 0, 180f), 1.5f).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
            lineImage2.DORotate(new Vector3(0, 0, -180f), 1.1f).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
            //播放音乐..
        }
        else
        {
            userRect.anchoredPosition = new Vector2(-3500f,623);
            iconRect.localScale = Vector3.one*0.4f;
            iconRect.anchoredPosition = new Vector2(890,-460);
            lineImage.gameObject.SetActive(true);
            lineImage2.gameObject.SetActive(true);
            lineImage.DOKill();
            lineImage2.DOKill();
            lineImage.localRotation = Quaternion.identity;
            lineImage2.localRotation = Quaternion.identity;
            BG.SetActive(false);
        }
    }

    public void MoveOut()
    {
        lineImage.gameObject.SetActive(false);
        lineImage2.gameObject.SetActive(false);
        iconRect.DOLocalMove(new Vector3(-1680, 2220, 0), 0.6f).OnComplete(() =>
        {
            userRect.DOLocalMove(new Vector3(-3500f, 623, 0), 1.2f).SetEase(Ease.InBack);
        });
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
        MoveOut();
        yield return new WaitForSeconds(2f);
        MoveTipsPanel(false);
    }
}
