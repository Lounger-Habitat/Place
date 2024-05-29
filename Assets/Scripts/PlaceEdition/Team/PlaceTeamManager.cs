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
        if (teamArea1 != null )
        {
            PlaceTeamAreaManager tam1 = teamArea1.GetComponent<PlaceTeamAreaManager>();
            teamAreas.Add(tam1);
        }

        if (teamArea2 != null )
        {
            PlaceTeamAreaManager tam2 = teamArea2.GetComponent<PlaceTeamAreaManager>();
            teamAreas.Add(tam2);
        }

        // if (teamArea3 != null )
        // {
        //     PlaceTeamAreaManager tam3 = teamArea3.GetComponent<PlaceTeamAreaManager>();
        //     teamAreas.Add(tam3);
        // }

        // if (teamArea4 != null )
        // {
        //     PlaceTeamAreaManager tam4 = teamArea4.GetComponent<PlaceTeamAreaManager>();
        //     teamAreas.Add(tam4);
        // }
    }

    public void Reset() {
        foreach (PlaceTeamAreaManager tam in teamAreas) {
            tam.Reset();
        }
    }

    public void SetTeamNumber(int number)
    {
        foreach (PlaceTeamAreaManager team in teamAreas)
        {
            team.SetTeamNumber(number);
        }

    }

}
