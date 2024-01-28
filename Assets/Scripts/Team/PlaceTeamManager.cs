using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTeamManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlaceTeamManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        CreateTeamArea();
    }

    [Header("队伍区域预制件")]
    public GameObject teamArea1;
    public GameObject teamArea2;
    public GameObject teamArea3;
    public GameObject teamArea4;

    [Header("队伍区域元信息")]
    [Header("队伍区域管理")]
    public  List<PlaceTeamAreaManager> teamAreas = new List<PlaceTeamAreaManager>();

    void Start() 
    {
        // CreateTeamArea();
    }
    // 创建队伍区域
    public void CreateTeamArea()
    {
        PlaceTeamAreaManager tam1 = teamArea1.GetComponent<PlaceTeamAreaManager>();
        PlaceTeamAreaManager tam2 = teamArea2.GetComponent<PlaceTeamAreaManager>();
        PlaceTeamAreaManager tam3 = teamArea3.GetComponent<PlaceTeamAreaManager>();
        PlaceTeamAreaManager tam4 = teamArea4.GetComponent<PlaceTeamAreaManager>();
        teamAreas.Add(tam1);
        teamAreas.Add(tam2);
        teamAreas.Add(tam3);
        teamAreas.Add(tam4);
    }

}
