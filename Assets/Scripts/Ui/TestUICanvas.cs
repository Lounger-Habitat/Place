using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenBLive.Runtime.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestUICanvas : MonoBehaviour
{
    public TMP_Text tipsText;
    
    private string playerName = "好奇小松鼠";

    string GenRandomName()
    {
        // 定义一些常见的名字前缀和后缀
        string[] prefixes = {
            "Aiden", "Bella", "Carter", "Dylan", "Ethan",
            "Fiona", "Grayson", "Harper", "Isla", "Jaxon"
        };

        string[] suffixes = {
            "-son", "-ette", "-man", "-ley", "-ton",
            "-field", "-berg", "-ston", "-worth", "-land"
        };

        string[] middlefixes = {
            "Rose", "James", "Liam", "Olivia", "Noah",
            "Mia", "Ella", "William", "Sophia", "Ava"
        };

        // 随机选择前缀和后缀
        string prefix = prefixes[Random.Range(0,prefixes.Length)];
        string suffix = suffixes[Random.Range(0,suffixes.Length)];
        string middlefix = middlefixes[Random.Range(0, middlefixes.Length)];

        // 组合成名字
        return prefix + middlefix +suffix;
    }
    

    public void DefIns(string cmd)
    {
        if (cmd.Contains("a"))
        {
            tipsText.gameObject.SetActive(false);
            playerName = GenRandomName();
            Dm dm = MakeDm(playerName, cmd);
            PlaceInstructionManager.Instance.DefaultDanmuCommand(dm);
            return;
        }

        playerName = PlaceCenter.Instance.users.Keys.ToArray()[Random.Range(0,PlaceCenter.Instance.users.Keys.Count())];
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
                playerName = GenRandomName();
                Dm dm = MakeDm(playerName, cmd);
                PlaceInstructionManager.Instance.DefaultDanmuCommand(dm);
                
                // Dm dm2 = MakeDm("快乐小老虎", "/a 1");
                // PlaceInstructionManager.Instance.DefaultDanmuCommand(dm2);
                return;
            }
            Debug.LogError("用户不存在");
            tipsText.gameObject.SetActive(true);
        }
    }

    public void DefGift(float giftValue)
    {
        playerName = PlaceCenter.Instance.users.Keys.ToArray()[Random.Range(0,PlaceCenter.Instance.users.Keys.Count())];
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
        playerName = PlaceCenter.Instance.users.Keys.ToArray()[Random.Range(0,PlaceCenter.Instance.users.Keys.Count())];
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

    public Button[] btns;
 #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            btns[0].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            btns[1].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            btns[2].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            btns[3].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            btns[4].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            btns[5].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            btns[6].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            btns[7].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            btns[8].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            btns[9].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            btns[10].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            btns[11].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            btns[12].onClick.Invoke();
        }
    }
 #endif
}
