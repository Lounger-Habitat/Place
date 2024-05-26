using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class TextTips : TipsBase
{
  public TMP_Text itemPrefabs;
  public Transform  parent;
  public override void SetData(TipsItem tips)
  {
      var t = Instantiate(itemPrefabs, parent);
      t.text = tips.text;
      t.DOFade(0, 2.3f).SetEase(Ease.InExpo).OnComplete(() => Destroy(t.gameObject));
      LayoutRebuilder.ForceRebuildLayoutImmediate(t.transform.parent as RectTransform);
  }

  public override void MoveTipsPanel(bool isOn = true)
  {
    //只有提示 无其他作用，不需要移动，此方法暂不使用
  }
}
