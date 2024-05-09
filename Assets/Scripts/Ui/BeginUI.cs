using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginUI : MonoBehaviour
{
    //开始时调用，先把整个UI移到屏幕中间
    public GameObject placeSettingUI;
    public void Init()
    {
        (transform as RectTransform).anchoredPosition = new Vector2(0, 0);
        // gameObject.SetActive(true);
    }
    
    public void OnClickBeginBtn()
    {
        //通知游戏开始了
        PlaceCenter.Instance.StartGame();
        PlaceCenter.Instance.RecordImage();
        //把自己整个移出显示区域
        (transform as RectTransform).anchoredPosition = new Vector2(9000, 0);
        // gameObject.SetActive(false);
    }

    public void OnClickSettingBtn()
    {
        // 显示 ui 界面
        // 关闭setting
        placeSettingUI.SetActive(true);
    }
}
