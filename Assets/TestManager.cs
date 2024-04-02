using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public string ins;
    public string playerName;
    // Start is called before the first frame update

    public string gift="20";

    public Vector3 position;
    public Vector3 rotation;

    private float minInterval = 0.1f; // 最小时间间隔
    private float maxInterval = 2.0f; // 最大时间间隔

    // Update is called once per frame
    void Update()
    {
        // 按下/，执行指令  接受 指令
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            PlaceInstructionManager.DefaultRunChatCommand(playerName,ins);
            // ChatCommandManager.Instance.RunChatCommand("test",ins);
        }
        // 按下,，执行指令  接受 指令
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            PlaceInstructionManager.DefaultGiftCommand(playerName,gift);
        }
        // 按下.，执行指令  测试 指令
        if (Input.GetKeyDown(KeyCode.Period))
        {
            // 生成角色
            GenPlayer();
            // 不定时 随机生成指令
            StartCoroutine(GenerateRandomCommand());

        }
        
    }

    public void CameraView() {
        Camera.main.transform.position = position;
        Camera.main.transform.rotation = Quaternion.Euler(rotation);
    }

    public void GenPlayer(){
        // 添加player
        string[] cx1 = { "cx1", "111" };
        string[] cx2 = { "cx2", "111" };
        string[] cx3 = { "cx3", "111" };
        string[] cx4 = { "cx4", "111" };

        string[] gt1 = { "gt1", "222" };
        string[] gt2 = { "gt2", "222" };
        string[] gt3 = { "gt3", "222" };
        string[] gt4 = { "gt4", "222" };

        string[] by1 = { "by1", "333" };
        string[] by2 = { "by2", "333" };
        string[] by3 = { "by3", "333" };
        string[] by4 = { "by4", "333" };

        string[] hy1 = { "hy1", "444" };
        string[] hy2 = { "hy2", "444" };
        string[] hy3 = { "hy3", "444" };
        string[] hy4 = { "hy4", "444" };

        var combinedListLinq = new[] { cx1, cx2, cx3, cx4, gt1, gt2, gt3, gt4, by1, by2, by3, by4, hy1, hy2, hy3, hy4 }.SelectMany(a => a).ToList();
        
        StartCoroutine(RepeatFunctionCall(combinedListLinq));
        
        //执行指令
    }

    IEnumerator RepeatFunctionCall(List<string> combinedListLinq)
    {
        for (int i = 0; i < combinedListLinq.Count; i=i+2) // 循环
        {
            PlaceInstructionManager.DefaultRunChatCommand(combinedListLinq[i],combinedListLinq[i+1]); // 调用你的函数
            yield return new WaitForSeconds(1f); // 等待1秒
        }
    }

    public string RandomGenDrawIns() {
        int height = PlaceBoardManager.Instance.height;
        int width = PlaceBoardManager.Instance.width;

        string drawIns = "";

        // 0-1 random
        float rand = Random.Range(0f, 1f);
        if (rand<0.5f) {
            // 生成 画点指令
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            // 随机生成颜色
            int r = Random.Range(0, 255);
            int g = Random.Range(0, 255);
            int b = Random.Range(0, 255);

            drawIns = "/d " + x + " " + y + " " + r + " " + g + " " + b;
        }else if(rand<0.8f) {
            // 生成 画线指令
            int x1 = Random.Range(0, width);
            int y1 = Random.Range(0, height);
            int x2 = Random.Range(0, width);
            int y2 = Random.Range(0, height);
            
            // 随机生成颜色
            int r = Random.Range(0, 255);
            int g = Random.Range(0, 255);
            int b = Random.Range(0, 255);

            drawIns = "/l " + x1 + " " + y1 + " " + x2 + " " + y2 + " " + r + " " + g + " " + b;

        }else {
            // 生成 画线指令
            int count = Random.Range(0, 20);
            char[] gifts = { '1' , '2', '3', '4', '6', '7', '8', '9'};
            string s = "";
            for (int i = 0; i < count; i++)
            {
                int ci = Random.Range(0, gifts.Length);
                char c = gifts[ci];
                s = s + c;
            }

            // 随机生成颜色
            int r = Random.Range(0, 255);
            int g = Random.Range(0, 255);
            int b = Random.Range(0, 255);

            drawIns = "/m " + s + " " + r + " " + g + " " + b;
        }

        return drawIns;

    }

    public string RandomGenGiftIns() {

        string[] gifts = { "0.1", "1", "1.9", "5.2", "9.9", "19.9", "29.9", "52", "66.6", "88.8", "99.9", "120"};
        int grand = Random.Range(0, gifts.Length);
        string giftIns = gifts[grand];
    
        return giftIns;

    }

    IEnumerator GenerateRandomCommand()
    {
        while (PlaceCenter.Instance.gameRuning) // 无限循环生成指令
        {
            // 随机等待一段时间
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            ExecuteCommand();
        }
    }

    // 根据需要生成具体的指令
    string GenerateCommand()
    {
        float rand = Random.Range(0f, 1f);
        if (rand<0.95f) {
            return RandomGenDrawIns();
        }else {
            return RandomGenGiftIns();
        }
    }

    // 执行生成的指令
    void ExecuteCommand()
    {
        // 随机选择 用户
        // string[] users = { "cx1", "cx2", "cx3", "cx4", "gt1", "gt2", "gt3", "gt4", "by1", "by2", "by3", "by4", "hy1", "hy2", "hy3", "hy4" };
        string[] users = PlaceCenter.Instance.users.Keys.ToArray();
        int urand = Random.Range(0, users.Length);
        string user = users[urand];

        float rand = Random.Range(0f, 1f);
        if (rand<0.95f) {
            string drawIns = RandomGenDrawIns();
            Debug.Log($"{user} 执行 ({drawIns}) 指令");
            PlaceInstructionManager.DefaultRunChatCommand(user,drawIns);
        }else {
            string giftIns = RandomGenGiftIns();
            Debug.Log($"{user} 赠送 ({giftIns}) 颜料");
            PlaceInstructionManager.DefaultGiftCommand(user,giftIns);
        }
    }
}


/*
    一人充钱，全队享受赐福
    初始赠予100颜料
    1. 人员加入队伍
    2. 人员执行绘画指令
*/