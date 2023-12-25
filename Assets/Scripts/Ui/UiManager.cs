using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    public static UiManager Instance;
    private RankListPanel rankList;
    private TeamListPanel teamList;
    // Start is called before the first frame update
    void Awake(){
        if (Instance!=null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        rankList  = GetComponentInChildren<RankListPanel>();
        teamList = GetComponentInChildren<TeamListPanel>();
        rankList.Init();
        teamList.Init();
    }

    public void SetRankData(List<RankItem> data){
        rankList.SetData(data);//设置数据
        //做一些事情，比如刷新UI
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
    
    public void SetTeamData(List<TeamItem> data){
        teamList.SetData(data);//
        //做一些事情，比如刷新UI
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
}
