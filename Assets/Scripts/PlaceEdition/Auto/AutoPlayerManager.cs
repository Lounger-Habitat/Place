using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AutoPlayerManager : MonoBehaviour
{
    public Sprite team1;
    public Sprite team2;
    public User team1Leader;
    public User team2Leader;

    // Start is called before the first frame update
    // void Start()
    // {
    //     StartCoroutine(AddAutoPlayer());
    // }
    private void Start()
    {
        PlaceCenter.Instance.satrtGameAction += OnStartGameCheckAutoStatus;
    }

    //开局时检查是否要自动生成队伍帮手
    public void OnStartGameCheckAutoStatus()
    {
        if (DataNoDeleteManager.Instance.addAutoPlayer)
        {
            StartCoroutine(AddAutoPlayer());
        }
    }
    
    IEnumerator AddAutoPlayer()
    {
        // 生成队伍负责人 两个人
        team1Leader = new User("困困-帮手");
        team2Leader = new User("泰美-帮手");

        // 设置 阵营
        team1Leader.Camp = 1;
        team2Leader.Camp = 2;

        // 人物 id
        team1Leader.Id = PlaceCenter.Instance.GenId();
        team2Leader.Id = PlaceCenter.Instance.GenId();

        //头像 
        team1Leader.userIcon = team1;
        team2Leader.userIcon = team2;

        yield return new WaitUntil(() => PlaceCenter.Instance.gameRuning == true);

        // 生成队伍负责人
        PlaceInstructionManager.Instance.DefaultRunChatCommand(team1Leader, "/a 1");
        PlaceInstructionManager.Instance.DefaultRunChatCommand(team2Leader, "/a 2");

        while (PlaceCenter.Instance.gameRuning) // 无限循环生成指令
        {
            // 随机等待一段时间 , 0.1 - 2s
            // float waitTime = Random.Range(30f, 60f);
            // yield return new WaitForSeconds(waitTime);

            if (Random.Range(1, 3) == 1)
            {
                if (team1Leader.instructionQueue.Count == 0) {
                    string drawIns = RandomGenDrawIns("team1");
                    PlaceInstructionManager.Instance.DefaultRunChatCommand(team1Leader, drawIns);
                }

            }
            else
            {
                if (team2Leader.instructionQueue.Count == 0) {
                    string drawIns = RandomGenDrawIns("team2");
                    PlaceInstructionManager.Instance.DefaultRunChatCommand(team2Leader, drawIns);
                }

            }

            float waitTime = Random.Range(5f, 10f);
            yield return new WaitForSeconds(waitTime);

        }

    }

    string RandomGenDrawIns(string camp)
    {
        string drawIns = "";
        if (camp == "team1")
        {
            drawIns = GenDrawIns(1);
        }
        else if (camp == "team2")
        {
            drawIns = GenDrawIns(2);
        }
        return drawIns;
    }

    public string GenDrawIns(int camp)
    {
        // 设置 图片 放缩 的 max 宽高 大小
        int max = 40;
        int height = PlaceBoardManager.Instance.height;
        int width = PlaceBoardManager.Instance.width;
        int x,y;
        if (camp == 1)
        {
            x = 0;
            y = height - 8;
        }
        else
        {
            x = width - max;
            y = height - 8;
        }
        string drawIns = $"/roll {x} {y} {max} {camp}";
        return drawIns;
    }
}
