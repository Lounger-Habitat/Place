using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        OnNumberInputEnd(inputField.text);//开局手动调一下 防止修改人数后没有确定
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


    public TMP_InputField inputField;
    public void OnNumberInputEnd(string str)
    {
        int number = int.Parse(str);
        number = Mathf.Clamp(number, 10, 100);
        inputField.text = number.ToString();
        PlaceTeamManager.Instance.SetTeamNumber(number);
    }

}
