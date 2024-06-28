using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlaceSettingPanel : MonoBehaviour
{
    public GameObject placeSettingUI;
    // Start is called before the first frame update
    void Start()
    {
        CheckAutoPlay(GameSettingManager.Instance.isAutoPlay,autoBtnImage,handle);
        CheckAutoPlay(GameSettingManager.Instance.addAutoPlayer,autoPlayerBtnImage,handleAutoPlayer);
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
    
    //自动开始游戏相关

    public Image autoBtnImage;
    public RectTransform handle;
    
    public Image autoPlayerBtnImage;
    public RectTransform handleAutoPlayer;
    public void OnClickAutoBtn()
    {
        if (GameSettingManager.Instance.isAutoPlay)
        {
            GameSettingManager.Instance.isAutoPlay = false;
        }
        else
        {
            GameSettingManager.Instance.isAutoPlay = true;
        }

        CheckAutoPlay(GameSettingManager.Instance.isAutoPlay,autoBtnImage,handle);
    }

    public void OnClickAutoPlayerBtn()
    {
        if (GameSettingManager.Instance.addAutoPlayer)
        {
            GameSettingManager.Instance.addAutoPlayer = false;
        }
        else
        {
            GameSettingManager.Instance.addAutoPlayer = true;
        }

        CheckAutoPlay(GameSettingManager.Instance.addAutoPlayer,autoPlayerBtnImage,handleAutoPlayer);
    }

    public void CheckAutoPlay(bool status, Image img, RectTransform rect)
    {
        if (!status)
        {

            rect.DOAnchorPosX(-50f, 0.4f).OnComplete(() =>
            {
                img.color = new Color(0.55f, 0.55f, 0.55f);
            });
        }
        else
        {
            rect.DOAnchorPosX(50f, 0.4f).OnComplete(() =>
            {
                img.color = new Color(1, 0.625f, 0.278f);
            });
        }
    }
}
