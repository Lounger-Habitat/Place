using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamListPanel : MonoBehaviour
{
    private Transform teamItem;
    private List<Transform> useList = new List<Transform>();
    private List<Transform> poolList = new List<Transform>();
    public void Init(){
        teamItem  = transform.GetChild(0);
        //teamItem.gameObject.SetActive(false);
    }

    public void SetData(List<TeamItem> rankItems){
        //先删除
        var count = useList.Count;
        for (int i = 1; i <= count; i++)
        {
           Destroy(useList[count-i].gameObject);
        }
        useList.Clear();
        //再生成
        foreach (var item in rankItems)
        {
            var teamTransform = Instantiate(teamItem,transform);
            teamTransform.gameObject.SetActive(true);
            teamTransform.Find("TeamName").GetComponent<TMP_Text>().text = item.teamName;
            teamTransform.Find("Data").GetComponent<TMP_Text>().text = $"当前人数:\n{item.teamNumber}";
            if (item.iconTexture!=null)
            {
                Sprite sp = Sprite.Create(item.iconTexture,new Rect(0,0,item.iconTexture.width,item.iconTexture.height),new Vector2(.5f,.5f));
                teamTransform.Find("Icon").GetComponent<Image>().sprite = sp;
            }
            useList.Add(teamTransform);
        }
        //刷新UI
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
}

public struct TeamItem{
    /// <summary>
    /// 团队名称
    /// </summary>
    public string teamName;
    /// <summary>
    /// 团队人数，目前这么写，可能是别的
    /// </summary>
    public string teamNumber;
    /// <summary>
    /// 团队图标
    /// </summary>
    public Texture2D iconTexture;
}