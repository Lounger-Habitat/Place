using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : MonoBehaviour
{
    private Transform rankItem;

    public void Init()
    {
        rankItem = transform.GetChild(0);
        //rankItem.gameObject.SetActive(false);
        SetRankUI(new List<User>());
    }

    //弃用
    // public void SetData(List<RankItem> rankItems)
    // {
    //     //先删除
    //     var count = useList.Count;
    //     for (int i = 1; i <= count; i++)
    //     {
    //         Destroy(useList[count - i].gameObject);
    //     }

    //     useList.Clear();
    //     //再生成
    //     foreach (var item in rankItems)
    //     {
    //         var rankTransform = Instantiate(rankItem, transform);
    //         rankTransform.gameObject.SetActive(true);
    //         rankTransform.Find("Name").GetComponent<TMP_Text>().text = item.userName;
    //         rankTransform.Find("Data").GetComponent<TMP_Text>().text = $"贡献等级:{item.rankData}";
    //         if (item.iconTexture != null)
    //         {
    //             Sprite sp = Sprite.Create(item.iconTexture,
    //                 new Rect(0, 0, item.iconTexture.width, item.iconTexture.height), new Vector2(.5f, .5f));
    //             rankTransform.Find("Icon").GetComponent<Image>().sprite = sp;
    //         }

    //         useList.Add(rankTransform);
    //     }

    //     //刷新UI布局
    //     LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    // }

    public void SetRankUI(List<User> userList)
    {
        //不限制传入数量，数量不够会自动补充
        for (int i = 0; i < 8; i++) //只显示前8名，前三名有奖牌，显示后边
        {
            rankItem = transform.GetChild(i);
            if (userList.Count <= i)
            {
                //没有数据了 需要自动填充
                rankItem.Find("Name").GetComponent<TMP_Text>().text = "虚位以待";
                rankItem.Find("Data").GetComponent<TMP_Text>().text = $"贡献:";
                //rankItem.Find("UserIcon").GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;
            }
            else
            {
                var item = userList[i];
                rankItem.Find("Name").GetComponent<TMP_Text>().text = item.Name;
                rankItem.Find("Data").GetComponent<TMP_Text>().text = $"贡献:{item.score}";
                rankItem.Find("UserIcon").GetChild(0).GetChild(0).GetComponent<Image>().sprite = item.userIcon;//TODO:需要对接user头像，目前没有，所以我的遮罩就没了
            }
        }
    }

    public void UpdateRankUI(List<User> userList){
        SetRankUI(userList);
    }
}
