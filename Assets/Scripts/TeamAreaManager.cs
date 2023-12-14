using System.Collections.Generic;
using UnityEngine;

public class TeamAreaManager : MonoBehaviour
{
    // 队伍区域当前容纳的人数
    public int currentTeamNumberCount = 0;

    private Team teaminfo;

    // prefab of character
    public GameObject characterPrefab;

    // 在队伍区域里创建角色
    public void CreateCharacterInTeamArea()
    {
        // 检查队伍区域是否已满
        if (currentTeamNumberCount < teaminfo.MaxTeamNumber)
        {
            // 创建角色
            Vector3 spawnPosition = GetRandomPositionInArea();
            Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
            currentTeamNumberCount += 1;
            // 可以在这里设置角色的其他属性，比如所属队伍等
        }
        else
        {
            Debug.Log("队伍区域已满");
        }

    }
    private Vector3 GetRandomPositionInArea()
    {
        float x = Random.Range(-5f, 5f); // 10x10区域内的随机x坐标
        float z = Random.Range(-5f, 5f); // 10x10区域内的随机z坐标
        return transform.position + new Vector3(x, 0, z);
    }

    public void setTeamInfo(Team team)
    {
        teaminfo = team;
    }

    public Team getTeamInfo()
    {
        return teaminfo;
    }
}