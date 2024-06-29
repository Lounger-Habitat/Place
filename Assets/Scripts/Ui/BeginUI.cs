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
        CheckAutoPlay();
        // GameSettingManager.Instance.mode = GameMode.Competition;
        UpdateUi();
    }

    public void OnClickBeginBtn()
    {
        StopAllCoroutines();
        OnNumberInputEnd(inputField.text);//开局手动调一下 防止修改人数后没有确定
        //通知游戏开始了
        PlaceCenter.Instance.StartGame();
        // PlaceCenter.Instance.RecordImage();
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
        GameSettingManager.Instance.maxNumber = number;//记录
        PlaceTeamManager.Instance.SetTeamNumber(number);
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
        GameSettingManager.Instance.mode = GameSettingManager.Instance.mode + 1;
        UpdateUi();
    }

    private void UpdateUi()
    {
        switch (GameSettingManager.Instance.mode) {
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
}
