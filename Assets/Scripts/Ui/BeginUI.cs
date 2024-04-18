using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginUI : MonoBehaviour
{
    //开始时调用，先把整个UI移到屏幕中间
    public void Init()
    {
        (transform as RectTransform).anchoredPosition = new Vector2(0, 0);
    }
    
    public void OnClickBeginBtn()
    {
        //通知游戏开始了
        PlaceCenter.Instance.StartGame();
        PlaceCenter.Instance.RecordImage();
        //把自己整个移出显示区域
        (transform as RectTransform).anchoredPosition = new Vector2(3000, 0);
    }

    public void OnCLickReBtn()
    {
        
    }
}
