using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BeginUI : MonoBehaviour
{
    //public bool isMode2 = false;

    //开始时调用，先把整个UI移到屏幕中间
    public GameObject placeSettingUI;

    public Toggle shuToggle;
    public Toggle hengToggle;
    void Start()
    {
        if (shuToggle == null)return;
        if (GameSettingManager.Instance.displayRatio == GameDisplayRatio.R9_16)
        {
            //SetShu();
            shuToggle.isOn = true;
            hengToggle.isOn = false;
        }

        if (GameSettingManager.Instance.displayRatio == GameDisplayRatio.R16_9)
        {
            //SetHeng();
            hengToggle.isOn = true;
            shuToggle.isOn = false;
        }
    }

    public void Init()
    {
        //将模式选择单独放到外部场景。
        // if (isMode2) //如果是mode2，表示当前场景是竞赛场景，不需要开始UI，直接开始游戏即可
        // {
        //     inputField.text = GameSettingManager.Instance.maxNumber.ToString();
        //     PlaceTeamManager.Instance.SetTeamNumber(GameSettingManager.Instance.maxNumber);
        //     cdp.ChangeTime(GameSettingManager.Instance.playTime); //设定相关游戏数据
        //
        //     OnClickBeginBtn(); //直接开始游戏
        // }
        // else
        // {
            (transform as RectTransform).anchoredPosition = new Vector2(0, 0);
            // gameObject.SetActive(true);
            CheckAutoPlay();
            // GameSettingManager.Instance.mode = GameMode.Competition;
            UpdateUi();
        //}
    }

    public void OnClickBeginBtn()
    {
        // if (GameSettingManager.Instance.Mode == GameMode.Graffiti && !isMode2)
        // {
        //     //如果是涂鸦模式 需要切换到竞速场景中
        //     SceneManager.LoadScene("1920-1080Scene Mode2");
        //     return;
        // }
        // if (GameSettingManager.Instance.Mode == GameMode.Competition && !isMode2)
        // {
        //     //如果是竞速模式 需要切换到竞速场景中
        //     SceneManager.LoadScene("1920-1080Scene Mode3");
        //     return;
        // }

        StopAllCoroutines();
        OnNumberInputEnd(inputField.text); //开局手动调一下 防止修改人数后没有确定
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

    private int playerNumber = 25;
    
    public void OnNumberInputEnd(string str)
    {
        int number = int.Parse(str);
        number = Mathf.Clamp(number, 10, 100);
        inputField.text = number.ToString();
        GameSettingManager.Instance.maxNumber = number; //记录
        PlaceTeamManager.Instance.SetTeamNumber(number);
    }

    public void OnNumberBtnClick(TMP_Text tmpText)
    {
        playerNumber += 25;
        if (playerNumber>100)
        {
            playerNumber = 25;
        }
        tmpText.text = playerNumber.ToString();
        GameSettingManager.Instance.maxNumber = playerNumber; //记录
        PlaceTeamManager.Instance.SetTeamNumber(playerNumber);
    }

    //自动游戏相关

    public CountdownPanel cdp;
    public TMP_Text timeText;
    public TMP_Text beginText;

    public void CheckAutoPlay()
    {
        if (GameSettingManager.Instance.isAutoPlay)
        {
            //如果是自动开启，就把时间与人数设定为上次设定，并且开始倒计时
            inputField.text = GameSettingManager.Instance.maxNumber.ToString();
            PlaceTeamManager.Instance.SetTeamNumber(GameSettingManager.Instance.maxNumber);

            cdp.ChangeTime(GameSettingManager.Instance.playTime);
            timeText.text = GameSettingManager.Instance.playTime.ToString();
            StartCoroutine(AutoPlayTime());
        }
    }

    IEnumerator AutoPlayTime()
    {
        int secends = 60;
        while (secends > 0)
        {
            if (placeSettingUI.activeSelf)
            {
                yield return new WaitForSeconds(1);
                continue;
            }

            beginText.text = $"开始({secends}s)";
            yield return new WaitForSeconds(1);
            secends--;
        }

        beginText.text = $"开始(0s)";
        OnClickBeginBtn();
    }

    public TMP_Text modeText;
    public GameObject createIcon;
    public GameObject graffitiIcon;
    public GameObject competitionIcon;

    public void OnClickModeBtn()
    {
        GameSettingManager.Instance.Mode = GameSettingManager.Instance.Mode + 1;
        UpdateUi();
    }

    private void UpdateUi()
    {
        switch (GameSettingManager.Instance.Mode)
        {
            case GameMode.Create:
                modeText.text = "创作模式";
                createIcon.SetActive(true);
                graffitiIcon.SetActive(false);
                competitionIcon.SetActive(false);
                break;
            case GameMode.Graffiti:
                modeText.text = "涂鸦模式";
                createIcon.SetActive(false);
                graffitiIcon.SetActive(true);
                competitionIcon.SetActive(false);
                break;
            case GameMode.Competition:
                modeText.text = "竞赛模式";
                createIcon.SetActive(false);
                graffitiIcon.SetActive(false);
                competitionIcon.SetActive(true);
                break;
        }
    }
    
    public void SelectGameModel(int gameMode)
    {   //选择后直接进入对应游戏模式场景
        GameSettingManager.Instance.Mode =(GameMode)gameMode;
        if (GameSettingManager.Instance.displayRatio == GameDisplayRatio.R9_16)
        {
            gameMode += 3;
        }
        SceneManager.LoadSceneAsync(gameMode);
    }

    public void GoToFirstScene()
    {
        SceneManager.LoadSceneAsync(0);
    }
    //分辨率下拉框
    public void OnShuToggleValueChange(bool isOn)
    {
        if (isOn && GameSettingManager.Instance.displayRatio != GameDisplayRatio.R9_16)
        {
            SetShu();
        }
    }

    public void SetShu()
    {
        int aim_height = 1920;
        int aim_width = 1080;

        int bestMatchIndex = 0;
        int smallestDifference = int.MaxValue;
        Resolution[] res = Screen.resolutions;
        for (int i = 0; i < res.Length; i++)
        {
            Resolution resolution = res[i];
            int currentDifference = Math.Abs(resolution.height - aim_height);
            // 找到更接近目标分辨率的分辨率
            if (currentDifference < smallestDifference)
            {
                smallestDifference = currentDifference;
                bestMatchIndex = i;
            }
        }
        int height = res[bestMatchIndex].height;
        if (height < aim_height)
        {
            aim_height = height;
            aim_width = height / 16 * 9; 
        }

            
        Screen.SetResolution(aim_width,aim_height,false);
        GameSettingManager.Instance.displayRatio = GameDisplayRatio.R9_16;

    }

    public void OnHengToggleValueChange(bool isOn)
    {
        if (isOn && GameSettingManager.Instance.displayRatio == GameDisplayRatio.R9_16)
        {
            SetHeng();
        }
    }

    public void SetHeng()
    {
        int aim_height = 1080;
        int aim_width = 1920;
        
        int bestMatchIndex = 0;
        int smallestDifference = int.MaxValue;
        Resolution[] res = Screen.resolutions;
        for (int i = 0; i < res.Length; i++)
        {
            Resolution resolution = res[i];
            int currentDifference = Math.Abs(resolution.width - aim_width);
            // 找到更接近目标分辨率的分辨率
            if (currentDifference < smallestDifference)
            {
                smallestDifference = currentDifference;
                bestMatchIndex = i;
            }
        }
        int width = res[bestMatchIndex].width;
        if (width < aim_width)
        { 
            aim_width = width;
            aim_height = width / 16 * 9;
        }
        Screen.SetResolution(aim_width,aim_height,false);
        GameSettingManager.Instance.displayRatio = GameDisplayRatio.R16_9;
        
    }
}