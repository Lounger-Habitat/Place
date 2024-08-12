using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class EndUI : MonoBehaviour
{
    public Transform[] rankItem;
    public Image displayImage;
    public void Init()
    {
        (transform as RectTransform).anchoredPosition = new Vector2(0, 0);
        OnSaveGifBengin();
        //获取排行数据，将当前前三名玩家展示出来
        var userList = PlaceCenter.Instance.users.Values.ToList();
        userList.Sort((a,b)=>b.score.CompareTo(a.score));//降序排列贡献值，取前三位
        for (int i = 0; i < rankItem[0].childCount; i++)
        {
            var item = rankItem[0].GetChild(i);
            if (i >= userList.Count)
            {
	            item.gameObject.SetActive(false);
	            continue;
            }//获取当前排行物体，分别赋值
            item.Find("Name").GetComponent<TMP_Text>().text = userList[i].Name;
            item.Find("Score").GetComponent<TMP_Text>().text = $"{userList[i].score}";
            item.Find("Picture").Find("InnerFrame").Find("Image").GetComponent<Image>().sprite = userList[i].userIcon;
        }
        userList.Sort((a,b)=>b.drawTimes.CompareTo(a.drawTimes));//降序排行颜料数 
        for (int i = 0; i < rankItem[1].childCount; i++)
        {
	        var item = rankItem[1].GetChild(i);//获取当前排行物体，分别赋值
	        if (i >= userList.Count)
	        {
		        item.gameObject.SetActive(false);
		        continue;
	        }
            item.Find("Name").GetComponent<TMP_Text>().text = userList[i].Name;
            item.Find("Score").GetComponent<TMP_Text>().text = $"{userList[i].drawTimes}";
            item.Find("Picture").Find("InnerFrame").Find("Image").GetComponent<Image>().sprite = userList[i].userIcon;
        }

        SetUniqueID();
    }

    public void OnClickNextBtn()
    {
        StopAllCoroutines();
        switch (GameSettingManager.Instance.Mode)
        {
            case GameMode.Create:
                SceneManager.LoadScene("1920-1080Scene");
                break;
            case GameMode.Graffiti:
                SceneManager.LoadScene("1920-1080Scene Mode2");
                break;
            case GameMode.Competition:
                SceneManager.LoadScene("1920-1080Scene Mode3");
                break;
        }
    }


    public Transform loadingTransform;
    public TMP_Text loadingText;
    public void OnSaveGifBengin()
    {
        loadingTransform.parent.gameObject.SetActive(true);
        loadingTransform.DORotate(new Vector3(0,0,-360),2f).SetLoops(-1, LoopType.Restart);
    }

    public void OnSaveGifLoading(int progress)
    {
        loadingText.text = $"正在加载...{progress}%";
    }
    public void OnSaveGifOk()
    {
        loadingTransform.parent.gameObject.SetActive(false);
        loadingTransform.DOKill();
        CheckAutoPlay();//加载完图片后再进行自动倒计时，防止图片没加载完 跳过了
    }
    
    //自动开始下一句相关

    public TMP_Text nextBtnText;
    private void CheckAutoPlay()
    {
        if (GameSettingManager.Instance.isAutoPlay)//如果是自动开始，就开始倒计时，并且给按钮上增加倒计时
        {
            StartCoroutine(AutoPlayTime());
        }
    }

    IEnumerator AutoPlayTime()
    {
        int secends = 60;
        while (secends>0)
        {
            nextBtnText.text = $"开始下一局({secends}s)";
            yield return new WaitForSeconds(1);
            secends--;
        }
        nextBtnText.text = $"开始下一局";
        switch (GameSettingManager.Instance.Mode)
        {
            case GameMode.Create:
                SceneManager.LoadScene("1920-1080Scene");
                break;
            case GameMode.Graffiti:
                SceneManager.LoadScene("1920-1080Scene Mode2");
                break;
            case GameMode.Competition:
                SceneManager.LoadScene("1920-1080Scene Mode3");
                break;
        }
    }
    
    public TMP_Text UniqueID_text1,UniqueID_text2;

    void SetUniqueID()
    {
        if (GameSettingManager.Instance.Mode == GameMode.Competition)
        {
            UniqueID_text1.text = PlaceTeamBoardManager.UniqueId;
            UniqueID_text2.text = PlaceTeamBoardManager.UniqueId;
        }
        
    }

    public GameObject team1, team2;
    public void SetWinTeam(int id)
    {
        if (id == 1)
        {
            team1.SetActive(true);
            team2.SetActive(false);
        }
        else
        {
            team1.SetActive(false);
            team2.SetActive(true);
        }
    }
}
