using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceSettingPanel : MonoBehaviour
{
    public GameObject placeSettingUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCloseSetting()
    {
        placeSettingUI.SetActive(false);
    }

    public void OnClickExitBtn()
    {
        if (PlaceCenter.Instance.gameRuning)
        {
            //游戏时 无法退出
            return;
        }
        //是否需要断开链接，检测当前是否在游戏，保存当前图片之类得
        Application.Quit();
    }

    public void PlaySound(int index)
    {
        AudioManager.Instance.PlaySound(index);
    }
    
    public void Slider_Audio(float value)
    {
        AudioManager.Instance.Slider_Audio(value);
    }
    
    public void Mute_Click()
    {
        AudioManager.Instance.Mute_Click();
    }
}
