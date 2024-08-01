using System.Collections;
using System.Collections.Generic;
using OpenBLive.Runtime.Data;
using TMPro;
using UnityEngine;

public class TestUICanvas : MonoBehaviour
{
    public TMP_Text tipsText;
    
    private string playerName = "好奇小松鼠";
    
    public void DefIns(string cmd)
    {
        cmd = cmd.Trim();
        if (PlaceCenter.Instance.users.ContainsKey(playerName))
        {
            tipsText.gameObject.SetActive(false);
            Dm dm = MakeDm(playerName, cmd);
            PlaceInstructionManager.Instance.DefaultDanmuCommand(dm);
        }
        else
        {
            if (cmd.Contains("a"))
            {
                tipsText.gameObject.SetActive(false);
                Dm dm = MakeDm(playerName, cmd);
                PlaceInstructionManager.Instance.DefaultDanmuCommand(dm);
                
                Dm dm2 = MakeDm("快乐小老虎", "/a 1");
                PlaceInstructionManager.Instance.DefaultDanmuCommand(dm2);
                return;
            }
            Debug.LogError("用户不存在");
            tipsText.gameObject.SetActive(true);
        }
    }

    public void DefGift(float giftValue)
    {
        if (!PlaceCenter.Instance.users.ContainsKey(playerName))
        {
            Debug.LogError("用户不存在");
            tipsText.gameObject.SetActive(true);
            return;
        }
        tipsText.gameObject.SetActive(false);
        User u = PlaceCenter.Instance.users[playerName];
        PlaceCenter.Instance.GainPower(u.Name, giftValue);
    }
    
    public void DoLike()
    {
        // 用户存在
        if (!PlaceCenter.Instance.users.ContainsKey(playerName))
        {
            Debug.LogError("用户不存在");
            tipsText.gameObject.SetActive(true);
            return;
        }
        tipsText.gameObject.SetActive(false);
        User u = PlaceCenter.Instance.users[playerName];
        int count = 10;
        PlaceCenter.Instance.GainLikePower(u, count);
    }
    public Dm MakeDm(string name, string ins)
    {
        Dm dm = new Dm();
        dm.userName = name;
        dm.userFace = "https://unsplash.com/photos/mou0S7ViElQ/download?ixid=M3wxMjA3fDB8MXxzZWFyY2h8M3x8Y2FydG9vbnxlbnwwfHx8fDE3MTg3Nzg5MzZ8MA&force=true&w=640";
        
        dm.msg = ins;
        return dm;
    }
}
