using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    public Transform[] rankItem;
    public Image displayImage;
    public void Init()
    {
        (transform as RectTransform).anchoredPosition = new Vector2(0, 0);
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
        userList.Sort((a,b)=>b.carryingInkCount.CompareTo(a.carryingInkCount));//降序排行有效颜料数 TODO:当前carryingInkCount不是颜料参数
        for (int i = 0; i < rankItem[1].childCount; i++)
        {
	        var item = rankItem[1].GetChild(i);//获取当前排行物体，分别赋值
	        if (i >= userList.Count)
	        {
		        item.gameObject.SetActive(false);
		        continue;
	        }
            item.Find("Name").GetComponent<TMP_Text>().text = userList[i].Name;
            item.Find("Score").GetComponent<TMP_Text>().text = $"{userList[i].carryingInkCount}";
            item.Find("Picture").Find("InnerFrame").Find("Image").GetComponent<Image>().sprite = userList[i].userIcon;
        }
    }

    public void OnClickNextBtn()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowGIF(string path)
	{
		ProGifManager.Instance.m_OptimizeMemoryUsage = true;

		//Open the Pro GIF player to show the converted GIF
        Debug.Log($"path: {path}");
		ProGifManager.Instance.PlayGif(path, displayImage, (loadProgress)=>{
			if(loadProgress < 1f)
			{
				displayImage.SetNativeSize();
			}
		});

        displayImage.sprite = ProGifTexturesToGIF.Instance.GetSprite(0);
		displayImage.SetNativeSize();
	}
}
