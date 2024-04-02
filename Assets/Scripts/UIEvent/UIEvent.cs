using System.Collections.Generic;
using UnityEngine;

public class UIEvent
{
    // 用户更新事件
    public delegate void UserUpdateEventHandler(User user);
    public static event UserUpdateEventHandler OnUserUpdateEvent;

    // 队伍更新事件
    public delegate void TeamUpdateEventHandler(Team team);
    public static event TeamUpdateEventHandler OnTeamUpdateEvent;

    // Rank 更新事件
    public delegate void RankUpdateEventHandler(List<User> userList);
    public static event RankUpdateEventHandler OnRankUpdateEvent;
    
    public delegate void TeamAreaUpdateEventHandler(PlaceTeamAreaManager teamArea);
    public static event TeamAreaUpdateEventHandler OnTeamAreaUpdateEvent;


    public static void OnTeamUIUpdate(Team team) {
        if (OnTeamUpdateEvent != null)
        {        
            OnTeamUpdateEvent(team);
        }
    }

    public static void OnRankUIUpdate(List<User> userList) {
        if (OnRankUpdateEvent != null)
        {        
            OnRankUpdateEvent(userList);
        }
    }
    public static void OnUserUIUpdate(User user) {
        if (OnUserUpdateEvent != null)
        {        
            OnUserUpdateEvent(user);
        }
    }
    
    public static void OnTeamAreaUIUpdate(PlaceTeamAreaManager team)
    {
        OnTeamAreaUpdateEvent?.Invoke(team);
    }
}