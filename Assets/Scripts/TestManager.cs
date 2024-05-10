using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using OpenBLive.Runtime.Data;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public string ins;
    public string playerName;
    // Start is called before the first frame update

    public string gift = "1";

    public Vector3 position;
    public Vector3 rotation;

    private float minInterval = 0.1f; // 最小时间间隔
    private float maxInterval = 2.0f; // 最大时间间隔

    public List<Texture2D> loadedTextures;
    #if UNITY_EDITOR
    public string directoryPath = "Assets/Images";
    # else
    public string directoryPath = $"{Application.persistentDataPath}/Images";
    # endif
    public int index = 0;
    public Color[] pixelsImage;
    public static TestManager Instance { get; private set; }


    public int teamCount = 5;



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
    }

    void Start()
    {
        loadedTextures = PlaceCenter.Instance.LoadResources(directoryPath);
        pixelsImage = PlaceCenter.Instance.ImageFitBoardProcessor(loadedTextures, index).GetPixels();
    }

    // Update is called once per frame
    void Update()
    {
        // 按下/，执行指令  接受 指令
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            ins = ins.Trim();

            if (PlaceCenter.Instance.users.ContainsKey(playerName))
            {
                Dm dm = MakeDm(playerName, ins);
                PlaceInstructionManager.Instance.DefaultDanmuCommand(dm);
            }
            else
            {
                Dm dm = MakeDm(playerName, ins);
                PlaceInstructionManager.Instance.DefaultDanmuCommand(dm);
            }
        }
        // 按下‘，执行指令  接受 指令
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ins = ins.Trim();
            string[] parts = ins.Split(' ');


            if (parts.Length == 3)
            {
                string c, name;
                float power;
                c = parts[0]; // /d
                name = parts[1]; // name
                power = float.Parse(parts[2]); // y
                Debug.Log($"{c} {name} {power}");
                PlaceCenter.Instance.GainPower(name, power);
            }
            else if (parts.Length == 4)
            {
                string c, name;
                long count;
                c = parts[0]; // /d
                name = parts[1]; // name
                User u = PlaceCenter.Instance.users[name];
                count = long.Parse(parts[3]); // count
                Debug.Log($"{c} {name} {count}");
                PlaceCenter.Instance.GainLikePower(u, count);
            }
        }
        // 按下,，执行指令  接受 指令
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            PlaceInstructionManager.Instance.DefaultGiftCommand(playerName, gift);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            string[] users = PlaceCenter.Instance.users.Keys.ToArray();
            // 遍历所有用户
            foreach (var user in users)
            {
                PlaceInstructionManager.Instance.DefaultGiftCommand(user, gift);
            }

        }
        // 按下.，执行指令  测试 指令
        if (Input.GetKeyDown(KeyCode.Period))
        {
            // 生成角色
            // GenPlayer();
            GenBiliPlayer();
            // 不定时 随机生成指令
            StartCoroutine(GenerateRandomCommand());
            // StartCoroutine(GenerateRandomCommand());
            // StartCoroutine(GenerateRandomCommand());
            // StartCoroutine(GenerateRandomCommand());

        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            string coolIns = RandomGenCoolIns();
            User u = PlaceCenter.Instance.users[playerName];
            PlaceInstructionManager.Instance.DefaultRunChatCommand(u, coolIns);
        }

    }

    public void CameraView()
    {
        Camera.main.transform.position = position;
        Camera.main.transform.rotation = Quaternion.Euler(rotation);
    }

    public Dm MakeDm(string name, string ins)
    {
        Dm dm = new Dm();
        dm.userName = name;
        dm.userFace = "https://unsplash.com/photos/EGJVpJr_r3w/download?ixid=M3wxMjA3fDB8MXxhbGx8MTR8fHx8fHwyfHwxNzEzMjUyNjAyfA&force=true&w=640";
        dm.msg = ins;
        return dm;
    }

    public void GenPlayer()
    {
        // 添加player
        string[] cx1 = { "cx1", "/a 1" };
        string[] cx2 = { "cx2", "/a 1" };
        string[] cx3 = { "cx3", "/a 1" };
        string[] cx4 = { "cx4", "/a 1" };

        string[] gt1 = { "gt1", "/a 2" };
        string[] gt2 = { "gt2", "/a 2" };
        string[] gt3 = { "gt3", "/a 2" };
        string[] gt4 = { "gt4", "/a 2" };

        string[] by1 = { "by1", "/a 3" };
        string[] by2 = { "by2", "/a 3" };
        string[] by3 = { "by3", "/a 3" };
        string[] by4 = { "by4", "/a 3" };

        string[] hy1 = { "hy1", "/a 4" };
        string[] hy2 = { "hy2", "/a 4" };
        string[] hy3 = { "hy3", "/a 4" };
        string[] hy4 = { "hy4", "/a 4" };

        var combinedListLinq = new[] { cx1, cx2, cx3, cx4, gt1, gt2, gt3, gt4, by1, by2, by3, by4, hy1, hy2, hy3, hy4 }.SelectMany(a => a).ToList();
        StartCoroutine(RepeatFunctionCall(combinedListLinq));

        //执行指令
    }

    public void GenBiliPlayer()
    {
        Dictionary<string, string> myDictionary = new Dictionary<string, string>();

        for (int i = 0; i < teamCount; i++)
        {
            string cxname = "cx" + (i + 1);
            string cxins = "蓝";
            string gtname = "gt" + (i + 1);
            string gtins = "绿";
            myDictionary.Add(cxname, cxins);
            myDictionary.Add(gtname, gtins);
        }

        // 制作 dm
        List<Dm> dms = myDictionary.Keys.ToList().Select(k => MakeDm(k, myDictionary[k])).ToList();

        // 执行指令
        StartCoroutine(RepeatDmCall(dms));



    }

    IEnumerator RepeatFunctionCall(List<string> combinedListLinq)
    {
        for (int i = 0; i < combinedListLinq.Count; i = i + 2) // 循环
        {
            string uname = combinedListLinq[i];
            string ins = combinedListLinq[i + 1];
            User u = new User(uname);
            u.Camp = int.Parse(Regex.Match(ins, @"\d+").Value);
            u.Id = PlaceCenter.Instance.GenId();
            PlaceInstructionManager.Instance.DefaultRunChatCommand(u, ins); // 调用你的函数
            yield return new WaitForSeconds(1f); // 等待1秒
        }
    }

    IEnumerator RepeatDmCall(List<Dm> dms)
    {
        foreach (var dm in dms)
        {// 循环
            // string uname = dm.userName;
            // string ins = dm.msg;
            // User u = new User(uname);
            // u.Camp = int.Parse(Regex.Match(ins, @"\d+").Value);
            PlaceInstructionManager.Instance.DefaultDanmuCommand(dm); // 调用你的函数
            yield return new WaitForSeconds(1f); // 等待1秒
        }
    }

    public string RandomGenDrawIns()
    {
        int height = PlaceBoardManager.Instance.height;
        int width = PlaceBoardManager.Instance.width;

        string drawIns = "";

        // 0-1 random
        float rand = Random.Range(0f, 1f);
        if (rand < 1.1f)
        {
            // 生成 画点指令
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            // 随机生成颜色
            int r, g, b;
            (r, g, b) = RandomGetPoint(x, y);

            drawIns = "/d " + x + " " + y + " " + r + " " + g + " " + b;
        }
        else if (rand < 0.8f)
        {
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

        }
        else
        {
            // 生成 画线指令
            int count = Random.Range(0, 20);
            char[] gifts = { '1', '2', '3', '4', '6', '7', '8', '9' };
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

    string RandomGenGiftIns()
    {

        // string[] gifts = { "0.1", "1", "1.9", "5.2", "9.9", "19.9", "29.9", "52", "66.6", "88.8", "99.9", "120"};
        string[] gifts = { "0.1", "1", "1.9", "5.2", "9.9" };
        int grand = Random.Range(0, gifts.Length);
        string giftIns = gifts[grand];

        return giftIns;

    }

    string RandomGenCoolIns()
    {
        int height = PlaceBoardManager.Instance.height;
        int width = PlaceBoardManager.Instance.width;

        string coolIns = "";

        // 0-1 random
        float rand = Random.Range(0f, 1f);

        // 生成 画点指令
        int x = Random.Range(0, width - 50);
        int y = Random.Range(0, height - 50);

        coolIns = "/r " + x + " " + y;
        return coolIns;
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
        if (rand < 0.95f)
        {
            return RandomGenDrawIns();
        }
        else
        {
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
        User u = PlaceCenter.Instance.users[user];



        float rand = Random.Range(0f, 1f);
        if (rand < 0.95f)
        {
            string drawIns = RandomGenDrawIns();
            // Debug.Log($"{u.Name} 执行 ({drawIns}) 指令");
            PlaceInstructionManager.Instance.DefaultRunChatCommand(u, drawIns);
        }
        else if (rand < 1f)
        {
            string giftIns = RandomGenGiftIns();
            // Debug.Log($"{u.Name} 赠送 ({giftIns}) 颜料");
            PlaceInstructionManager.Instance.DefaultGiftCommand(user, giftIns);
        }
    }

    (int, int, int) RandomGetPoint(int x, int y)
    {
        int i = y * PlaceBoardManager.Instance.width + x;
        Color32 c = pixelsImage[i];
        return (c.r, c.g, c.b);
    }




    public void DoLike()
    {
        // 用户存在
        if (!PlaceCenter.Instance.users.ContainsKey(playerName))
        {
            Debug.LogError("用户不存在");
            return;
        }
        User u = PlaceCenter.Instance.users[playerName];
        int count = Random.Range(1, 10);
        PlaceCenter.Instance.GainLikePower(u, count);
    }

    public void SendGift(float gift)
    {
        // 用户存在
        if (!PlaceCenter.Instance.users.ContainsKey(playerName))
        {
            Debug.LogError("用户不存在");
            return;
        }
        User u = PlaceCenter.Instance.users[playerName];
        PlaceCenter.Instance.GainPower(u.Name, gift);
    }
}


/*
    一人充钱，全队享受赐福
    初始赠予100颜料
    1. 人员加入队伍
    2. 人员执行绘画指令
*/

/*
    准备一些user数据，符合dm结构

    开始游戏

    结束游戏

    评选统计

*/