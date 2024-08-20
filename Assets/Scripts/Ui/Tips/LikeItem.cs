using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class LikeItem : MonoBehaviour
{
    public TMP_Text likeCountText;
    public TMP_Text inkCountText;
    public Image userIcon;
    public CanvasGroup cGroup;
    // Start is called before the first frame update
    void Start()
    {
        DOTween.To(() => cGroup.alpha, value => cGroup.alpha = value, 0, 1.5f).SetEase(Ease.InExpo);
        Invoke("AutoDelete",1.7f);
    }

    private void AutoDelete()
    {
        Destroy(gameObject);
    }

    public void SetUserData(TipsItem tips)
    {
        userIcon.sprite = tips.icon;
        likeCountText.text = $"x {tips.likeCount}";
        inkCountText.text = $"+ {tips.likeCount*2}";
    }
}
