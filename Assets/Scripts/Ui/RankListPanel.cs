using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankListPanel : MonoBehaviour
{
    private Transform rankItem;
    private List<Transform> useList = new List<Transform>();
    private List<Transform> poolList = new List<Transform>();
    public void Init()
    {
        rankItem = transform.GetChild(0);
        //rankItem.gameObject.SetActive(false);
    }

    public void SetData(List<RankItem> rankItems)
    {
        //先删除
        var count = useList.Count;
        for (int i = 1; i <= count; i++)
        {
            Destroy(useList[count - i].gameObject);
        }
        useList.Clear();
        //再生成
        foreach (var item in rankItems)
        {
            var rankTransform = Instantiate(rankItem, transform);
            rankTransform.gameObject.SetActive(true);
            rankTransform.Find("Name").GetComponent<TMP_Text>().text = item.userName;
            rankTransform.Find("Data").GetComponent<TMP_Text>().text = $"贡献等级:{item.rankData}";
            if (item.iconTexture != null)
            {
                Sprite sp = Sprite.Create(item.iconTexture, new Rect(0, 0, item.iconTexture.width, item.iconTexture.height), new Vector2(.5f, .5f));
                rankTransform.Find("Icon").GetComponent<Image>().sprite = sp;
            }
            useList.Add(rankTransform);
        }
        //刷新UI布局
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
}

public struct RankItem
{
    /// <summary>
    /// 玩家名称
    /// </summary>
    public string userName;
    /// <summary>
    /// 分数
    /// </summary>
    public string rankData;
    /// <summary>
    /// 前方图标
    /// </summary>
    public Texture2D iconTexture;
}
