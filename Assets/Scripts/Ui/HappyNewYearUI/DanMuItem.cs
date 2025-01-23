using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DanMuItem : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text tMP_Text;

    public void Initialize(User user,string text)
    {
        iconImage.sprite = user.userIcon;
        tMP_Text.text = text;
        (transform as RectTransform).DOAnchorPosX(1400, 5f).SetEase(Ease.Linear).OnComplete(()=>Destroy(gameObject));
    }
}
