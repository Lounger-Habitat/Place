using System.Collections.Generic;
using UnityEngine;
using OpenBLive.Runtime.Data;
using UnityEngine.Networking;
using System.Collections;
using System.Text.RegularExpressions;
using System;

public class PlaceInstructionManager : MonoBehaviour
{
    // 解析字符串，生成指令
    /*
    [创作绘画] 点线面
    起点 + 颜色
    /draw 50 50 255 255 255
    /d    50 50 255 255 255
    使用上一次默认值
    /draw 50 50
    /d    50 50

    大范围绘画
    
    起点 + 终点 + 颜色
    /line 50 50 100 100 255 255 255
    /l    50 50 100 100 255 255 255
    起点 + 大小 + 颜色
    /paint 50 50 50 50 255 255 255
    /p     50 50 50 50 255 255 255


    [生成绘画]

    [加入队伍]
    /add x
    /a   x
    111 1
    222 2
    333 3
    444 4

    [拿取颜料]
    /take 
    /t

    [防守反击]
    /defense
    /de
    
    */

    // 快捷指令

    // 带有 指令 符号/
    private static PlaceInstructionManager instance;
    public static PlaceInstructionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlaceInstructionManager>();
            }
            return instance;
        }
    }
    public void DefaultRunChatCommand(User user, string command)
    {
        string[] parts = command.Trim().Split(' ');
        switch (parts[0])
        {
            case "1":
            case "2":
            case "3":
            case "4":
            case "6":
            case "7":
            case "8":
            case "9":
                // if (!PlaceCenter.Instance.CheckUser(username))
                // {
                //     break;
                // }
                // User quickDrawUser = PlaceCenter.Instance.FindUser(username);
                if (parts.Length > 1)
                {
                    int x, y, r, g, b;
                    string c;
                    c = parts[0]; // /d
                    x = user.lastPoint.x; // x
                    y = user.lastPoint.y; // y
                    (x,y) = ComputeQuickIns(c[0],x,y);
                    r = int.Parse(parts[1]); // x
                    g = int.Parse(parts[2]); // y
                    b = int.Parse(parts[3]); // r
                    Instruction drawIns = new Instruction("/d", x, y, r: r, g: g, b: b);
                    if (!PlaceBoardManager.Instance.CheckIns(drawIns)){
                        Debug.Log("指令不合法");
                        break;
                    }
                    user.lastColor = new Color(r, g, b);
                    user.lastPoint = (x, y);
                    user.instructionQueue.Enqueue(drawIns);
                }
                else
                {
                    int x, y, r, g, b;
                    string c;
                    c = parts[0]; // /d
                    x = user.lastPoint.x; // x
                    y = user.lastPoint.y; // y
                    (x,y) = ComputeQuickIns(c[0],x,y);
                    Color color = user.lastColor;
                    r = (int)(color.r * 255); // r
                    g = (int)(color.g * 255); // g
                    b = (int)(color.b * 255); // b
                    Instruction drawIns = new Instruction("/d", x, y, r: r, g: g, b: b);
                    if (!PlaceBoardManager.Instance.CheckIns(drawIns)){
                        Debug.Log("指令不合法");
                        break;
                    }
                    user.lastPoint = (x, y);
                    user.instructionQueue.Enqueue(drawIns);
                }
                break;
            // case "111":
            // case "222":
            // case "333":
            // case "444":
            //     string ins = parts[0];
            //     string teamId = Regex.Replace(ins, @"(.)\1+", m => m.Groups[1].Value);
            //     PlaceCenter.Instance.JoinTeam(username, teamId);
            //  break;
            case "/add":
            case "/a":
                if (parts.Length >= 2)
                {
                    // 这里调用加入队伍的逻辑
                    PlaceCenter.Instance.JoinGame(user, parts[1]);
                }
                break;
            case "/say":
            case "/s":
                if (parts.Length >= 2)
                {
                    // 这里调用发送消息的逻辑
                    // SayMessage(string.Join(" ", parts, 1, parts.Length - 1));
                    Debug.Log(parts[1]);
                    string message = parts[1];
                    PlaceCenter.Instance.SayMessage(user, message);
                }
                break;
            // 可以在这里添加其他命令的处理
            case "/draw":
            case "/d":
                // if (!PlaceCenter.Instance.CheckUser(username))
                // {
                //     break;
                // }
                // User drawUser = PlaceCenter.Instance.FindUser(username);
                if (parts.Length >= 3)
                {
                    if (parts.Length == 3)
                    {
                        int x, y, r, g, b;
                        string c;
                        c = parts[0]; // /d
                        x = int.Parse(parts[1]); // x
                        y = int.Parse(parts[2]); // y
                        Color color = user.lastColor;
                        r = (int)(color.r * 255); // r
                        g = (int)(color.g * 255); // g
                        b = (int)(color.b * 255); // b
                        Instruction drawIns = new Instruction(c, x, y, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(drawIns)){
                            Debug.Log("指令不合法");
                            break;
                        }
                        user.lastPoint = (x, y);
                        user.instructionQueue.Enqueue(drawIns);
                        // Debug.Log(username + " : draw " + c + " " + x + " " + y + " " + r + " " + g + " " + b);
                    }else if (parts.Length == 4)
                    {
                        int x, y, r, g, b;
                        string c,dc;
                        c = parts[0]; // /d
                        x = int.Parse(parts[1]); // x
                        y = int.Parse(parts[2]); // y
                        dc = parts[3];
                        if (colorDict.ContainsKey(dc)) {
                            Color32 color = colorDict[dc];
                            r = color.r; // r
                            g = color.g; // g
                            b = color.b; // b
                        }else {
                            Debug.Log("抱歉此颜色目前未包含在,可联系管理员申请新增颜色");
                            // UI 提示
                            break;
                        }
                        Instruction drawIns = new Instruction(c, x, y, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(drawIns)){
                            Debug.Log("指令不合法");
                            break;
                        }
                        user.lastPoint = (x, y);
                        user.instructionQueue.Enqueue(drawIns);
                    }
                    else if (parts.Length == 6)
                    {
                        int x, y, r, g, b;
                        string c;
                        c = parts[0]; // /d
                        x = int.Parse(parts[1]); // x
                        y = int.Parse(parts[2]); // y
                        r = int.Parse(parts[3]); // r
                        g = int.Parse(parts[4]); // g
                        b = int.Parse(parts[5]); // b
                        
                        Instruction drawIns = new Instruction(c, x, y, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(drawIns)){
                            Debug.Log("指令不合法");
                            break;
                        }
                        user.lastColor = new Color(r, g, b);
                        user.lastPoint = (x, y);
                        user.instructionQueue.Enqueue(drawIns);
                        // Debug.Log(username + " : draw " + c + " " + x + " " + y + " " + r + " " + g + " " + b);
                    }
                    else
                    {
                        Debug.LogError("输入字符串格式不正确");
                    }
                }
                else
                {
                    Debug.LogError("输入字符串格式不正确");
                }
                break;
            case "/line":
            case "/l":
                // if (!PlaceCenter.Instance.CheckUser(username))
                // {
                //     break;
                // }
                // User lineUser = PlaceCenter.Instance.FindUser(username);
                if (parts.Length >= 5)
                {
                    if (parts.Length == 5)
                    {
                        int x, y, ex, ey, r, g, b;
                        string c;
                        c = parts[0]; // /l
                        x = int.Parse(parts[1]); // x
                        y = int.Parse(parts[2]); // y
                        ex = int.Parse(parts[3]); // ex
                        ey = int.Parse(parts[4]); // ey
                        Color color = user.lastColor;
                        r = (int)(color.r * 255); // r
                        g = (int)(color.g * 255); // g
                        b = (int)(color.b * 255); // b
                        Instruction ins_l = new Instruction(c, x, y, ex: ex, ey: ey, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_l)){
                            Debug.Log("指令不合法");
                            break;
                        }
                        user.instructionQueue.Enqueue(ins_l);
                    }else if (parts.Length == 6)
                    {
                        int x, y, ex, ey, r, g, b;
                        string c,dc;
                        c = parts[0]; // /l
                        x = int.Parse(parts[1]); // x
                        y = int.Parse(parts[2]); // y
                        ex = int.Parse(parts[3]); // ex
                        ey = int.Parse(parts[4]); // ey
                        dc = parts[5];
                        if (colorDict.ContainsKey(dc)) {
                            Color32 color = colorDict[dc];
                            r = color.r; // r
                            g = color.g; // g
                            b = color.b; // b
                        }else {
                            Debug.Log("抱歉此颜色目前未包含在,可联系管理员申请新增颜色");
                            // UI 提示
                            break;
                        }
                        Instruction ins_l = new Instruction(c, x, y, ex: ex, ey: ey, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_l)){
                            Debug.Log("指令不合法");
                            // UI 提示
                            break;
                        }
                        user.instructionQueue.Enqueue(ins_l);
                    }
                    else if (parts.Length == 8)
                    {
                        int x, y, ex, ey, r, g, b;
                        string c;
                        c = parts[0]; // /l
                        x = int.Parse(parts[1]); // x
                        y = int.Parse(parts[2]); // y
                        ex = int.Parse(parts[3]); // ex
                        ey = int.Parse(parts[4]); // ey
                        r = int.Parse(parts[5]); // r
                        g = int.Parse(parts[6]); // g
                        b = int.Parse(parts[7]); // b
                        user.lastColor = new Color(r, g, b);
                        Instruction ins_l = new Instruction(c, x, y, ex: ex, ey: ey, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_l)){
                            Debug.Log("指令不合法");
                            break;
                        }
                        user.instructionQueue.Enqueue(ins_l);
                    }
                    else
                    {
                        Debug.LogError("输入字符串格式不正确");
                    }
                }
                else
                {
                    Debug.LogError("输入字符串格式不正确");
                }
                break;
            case "/paint":
            case "/p":
                // if (!PlaceCenter.Instance.CheckUser(username))
                // {
                //     break;
                // }
                // User paintUser = PlaceCenter.Instance.FindUser(username);
                if (parts.Length >= 5)
                {
                    if (parts.Length == 5)
                    {
                        int x, y, dx, dy, r, g, b;
                        string c;
                        c = parts[0]; // /l
                        x = int.Parse(parts[1]); // x
                        y = int.Parse(parts[2]); // y
                        dx = int.Parse(parts[3]); // dx
                        dy = int.Parse(parts[4]); // dy
                        Color color = user.lastColor;
                        r = (int)(color.r * 255); // r
                        g = (int)(color.g * 255); // g
                        b = (int)(color.b * 255); // b
                        Instruction ins_p = new Instruction(c, x, y, dx, dy, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_p)){
                            Debug.Log("指令不合法");
                            break;
                        }
                        user.instructionQueue.Enqueue(ins_p);
                    }
                    else if (parts.Length == 8)
                    {
                        int x, y, dx, dy, r, g, b;
                        string c;
                        c = parts[0]; // /d
                        x = int.Parse(parts[1]); // x
                        y = int.Parse(parts[2]); // y
                        dx = int.Parse(parts[3]); // dx
                        dy = int.Parse(parts[4]); // dy
                        r = int.Parse(parts[5]); // r
                        g = int.Parse(parts[6]); // g
                        b = int.Parse(parts[7]); // b
                        user.lastColor = new Color(r, g, b);
                        Instruction ins_p = new Instruction(c, x, y, dx, dy, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_p)){
                            Debug.Log("指令不合法");
                            break;
                        }
                        user.instructionQueue.Enqueue(ins_p);
                    }
                    else
                    {
                        Debug.LogError("输入字符串格式不正确");
                    }
                }
                else
                {
                    Debug.LogError("输入字符串格式不正确");
                }
                break;
            case "/m":
                // if (!PlaceCenter.Instance.CheckUser(username))
                // {
                //     break;
                // }
                // User mUser = PlaceCenter.Instance.FindUser(username);
                if (parts.Length == 2) // 默认颜色
                {
                    int x, y, r, g, b;
                    string c,s;
                    c = parts[0]; // /d
                    s = parts[1]; // x
                    (x,y) = user.lastPoint;
                    Color color = user.lastColor;
                    r = (int)(color.r * 255); // r
                    g = (int)(color.g * 255); // g
                    b = (int)(color.b * 255); // b
                    for (int i = 0; i < s.Length; i++)
                    {
                        char digitIns = s[i];
                        (x,y) = ComputeQuickIns(digitIns,x,y);
                        Instruction drawIns = new Instruction("/d", x, y, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(drawIns)){
                            Debug.Log("指令不合法");
                            continue;
                        }
                        user.instructionQueue.Enqueue(drawIns);
                    }
                    x = Mathf.Clamp(x, 0, PlaceBoardManager.Instance.width - 1);
                    y = Mathf.Clamp(y, 0, PlaceBoardManager.Instance.height - 1);
                    user.lastPoint = (x, y);
                }else if (parts.Length == 3) {
                    int x, y, r, g, b;
                    string c,s,dc;
                    c = parts[0]; // /d
                    s = parts[1]; // seq
                    dc = parts[2];
                    (x,y) = user.lastPoint;
                    if (colorDict.ContainsKey(dc)) {
                        Color32 color = colorDict[dc];
                        r = color.r; // r
                        g = color.g; // g
                        b = color.b; // b
                    }else {
                        Debug.Log("抱歉此颜色目前未包含在,可联系管理员申请新增颜色");
                        // UI 提示
                        break;
                    }
                    for (int i = 0; i < s.Length; i++)
                    {
                        char digitIns = s[i];
                        (x,y) = ComputeQuickIns(digitIns,x,y);
                        Instruction drawIns = new Instruction("/d", x, y, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(drawIns)){
                            Debug.Log("指令不合法");
                            continue;
                        }
                        user.instructionQueue.Enqueue(drawIns);
                    }
                    x = Mathf.Clamp(x, 0, PlaceBoardManager.Instance.width - 1);
                    y = Mathf.Clamp(y, 0, PlaceBoardManager.Instance.height - 1);
                    user.lastPoint = (x, y);
                }else if (parts.Length == 5){ // 多颜色
                    int x, y, r, g, b;
                    string c,s;
                    c = parts[0]; // /d
                    s = parts[1]; // x
                    r = int.Parse(parts[2]); // r
                    g = int.Parse(parts[3]); // g
                    b = int.Parse(parts[4]); // b
                    (x,y) = user.lastPoint;
                    for (int i = 0; i < s.Length; i++)
                    {
                        char digitIns = s[i];
                        (x,y) = ComputeQuickIns(digitIns,x,y);
                        Instruction drawIns = new Instruction("/d", x, y, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(drawIns)){
                            Debug.Log("指令不合法");
                            continue;
                        }
                        user.instructionQueue.Enqueue(drawIns);
                        
                    }
                    user.lastColor = new Color(r, g, b);
                    x = Mathf.Clamp(x, 0, PlaceBoardManager.Instance.width - 1);
                    y = Mathf.Clamp(y, 0, PlaceBoardManager.Instance.height - 1);
                    user.lastPoint = (x, y);
                }
                break;
            case "visual":
            case "/v":
                // if (!PlaceCenter.Instance.CheckUser(username))
                // {
                //     break;
                // }
                // User vUser = PlaceCenter.Instance.FindUser(username);
                if (parts.Length == 3)
                {
                    int x, y;
                    string c;
                    c = parts[0]; // /d
                    x = int.Parse(parts[1]); // x
                    y = int.Parse(parts[2]); // y
                    PlaceConsoleAreaManager.Instance.VisualAuxiliaryLine(x, y, user.Camp);
                }
                else
                {
                    Debug.LogError("输入字符串格式不正确");
                }
                break;
            case "gs":
                 break;
            case "/take": // 拿别人颜料 ，谁家
            case "/t":
                break;
            case "/defense": // 防止别人拿 ，状态
            case "/de":
                break;
            case "/kill": // 除掉守护者，谁家
            case "/k":
                break;
                // case "/generate": // diffusion
                // case "/g":
                //     if (parts.Length >= 4)
                //     {
                //         int x,y;
                //         string c,p;
                //         c = parts[0];
                //         x = int.Parse(parts[1]);
                //         y = int.Parse(parts[2]);
                //         p = string.Join(" ", parts.Skip(3).ToArray());
                //         PixelsCanvasController.Instance.GenerateImage(x,y,p);
                //     }
                //     else
                //     {
                //         Debug.LogError("输入字符串格式不正确");
                //     }
                //     break;
                // case "/instruction": // llm
                // case "/ins":
                //     if (parts.Length >= 2)
                //     {
                //         string p;
                //         p = parts[1];
                //         PromptManager.Instance.GenerateInstruction(p);
                //     }
                //     else
                //     {
                //         Debug.LogError("输入字符串格式不正确");
                //     }
                //     break;
        }
    }
    public void DefaultGiftCommand(string username, string command) {
        // 找到 对应 的 礼物
        string parts = command.Trim();
        float message = float.Parse(parts);
        PlaceCenter.Instance.GainPower(username, message);
        // switch (parts)
        // {
        //     case "0.1":
        //         int message = 1;
        //         PlaceCenter.Instance.GainPower(username, message);
        //         break;
        //     case "1":
        //         int message = 1;
        //         PlaceCenter.Instance.GainPower(username, message);
        //         break;
        //     case "1.9":
        //         int message = 1;
        //         PlaceCenter.Instance.GainPower(username, message);
        //         break;
        //     case "5.2":
        //         int message = 1;
        //         PlaceCenter.Instance.GainPower(username, message);
        //         break;
        //     case "9.9":
        //         int message = 1;
        //         PlaceCenter.Instance.GainPower(username, message);
        //         break;
        //     case "20":
        //         int message = 1;
        //         PlaceCenter.Instance.GainPower(username, message);
        //         break;
        // }
    }
    public void DefaultLikeCommand(Like like) {
        string username = like.uname;
        string uface = like.uface;
        Debug.Log($"uface : {uface}");
        

        // PlaceCenter.Instance.GainPower(username, 1);
    }

    public (int x,int y) ComputeQuickIns(char m, int x, int y) {
        switch (m) {
            case '1':
                x -= 1;
                y -= 1;
                break;
            case '2':
                y -= 1;
                break;
            case '3':
                x += 1;
                y -= 1;
                break;
            case '4':
                x -= 1;
                break;
            case '5':
                break;
            case '6':
                x += 1;
                break;
            case '7':
                x -= 1;
                y += 1;
                break;
            case '8':
                y += 1;
                break;
            case '9':
                x += 1;
                y += 1;
                break;
        }
        return (x, y);
    }




    // 弹幕
    public void DefaultDanmuCommand(Dm dm) {
        string username = dm.userName;
        string msg = dm.msg.Trim();
        if (PlaceCenter.Instance.users.ContainsKey(username))
        {
            // 获取用户
            User user = PlaceCenter.Instance.users[username];
            // 指令 - 传统指令
            if (msg.StartsWith("/"))
            {
                DefaultRunChatCommand(user,msg);
            }
            
            // 指令 - 快捷指令
            /*
                快速加入
                快速画点
                快速画线
                快速画自定义线
                快速画圆、方块、三角、星星
            */

            // 快速画点
            if (Regex.IsMatch(msg, FAST_DRAW_PATTERN)) { // 快速画点
                DefaultRunChatCommand(user,"/d " + msg);
            }else if (Regex.IsMatch(msg, FAST_LINE_PATTERN)) { // 快速画线
                DefaultRunChatCommand(user,"/l " + msg);
            }else if (Regex.IsMatch(msg, FAST_DRAW_DIY_PATTERN)) { // 快速画自定义线
                DefaultRunChatCommand(user,"/m " + msg);
            }
        }

        List<string> selectList = new List<string>(){
            "蓝",
            "绿",
            "黄",
            "紫"
        };

        if (selectList.Contains(msg)) {
            string userFace = dm.userFace;
            User user = new User(username);
            StartCoroutine(DownloadImage(user, userFace));
            // 加入
            switch (msg)
            {
                case "蓝":
                    user.Camp = 1;
                    DefaultRunChatCommand(user,"/a 1");
                    // 需要下载资源
                    // UI 信息 创建
                    break;
                case "绿":
                    user.Camp = 2;
                    DefaultRunChatCommand(user,"/a 2");
                    break;
                case "黄":
                    user.Camp = 3;
                    DefaultRunChatCommand(user,"/a 3");
                    break;
                case "紫":
                    user.Camp = 4;
                    DefaultRunChatCommand(user,"/a 4");
                    break;
                default:
                    break;
            }
        }
        


    }


    /*
        ========= 协程 ========
    */
    IEnumerator DownloadImage(User user, string url) {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            user.userIcon = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
        }
    }

    // 颜色字典
    public Dictionary<string, Color32> colorDict = new Dictionary<string, Color32>(){
        {"白", new Color32(255, 255, 255, 255)},
        {"黑", new Color32(0, 0, 0, 255)},
        {"红", new Color32(255, 0, 0, 255)},
        {"绿", new Color32(0, 255, 0, 255)},
        {"蓝", new Color32(0, 0, 255, 255)},
        {"黄", new Color32(255, 255, 0, 255)},
        {"紫", new Color32(255, 0, 255, 255)},
        {"青", new Color32(0, 255, 255, 255)},
        {"橙", new Color32(255, 165, 0, 255)},
        {"粉", new Color32(255, 192, 203, 255)},
        {"灰", new Color32(128, 128, 128, 255)},
        {"棕", new Color32(165, 42, 42, 255)},
        {"金", new Color32(255, 215, 0, 255)},
        {"银", new Color32(192, 192, 192, 255)}
    };


    // PATTERN
    private const string FAST_DRAW_POINT_PATTERN = @"^\d{1,3} \d{1,3} \d{1,3} \d{1,3} \d{1,3}$";
    private const string FAST_DRAW_POINT_WITH_DEFAULT_COLOR_PATTERN = @"^\d{1,3} \d{1,3} [\u4E00-\u9FFF]+$";
    private const string FAST_DRAW_POINT_WITH_LAST_COLOR_PATTERN = @"^\d{1,3} \d{1,3}$";
    public const string FAST_DRAW_PATTERN = @"(^\d{1,3} \d{1,3}$)|(^\d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)";
    public const string FAST_LINE_PATTERN = @"(^\d{1,3} \d{1,3} \d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)|(^\d{1,3} \d{1,3} \d{1,3} \d{1,3}$)";
    public const string FAST_DRAW_DIY_PATTERN = @"(^[1-9]\d*$)|(^[1-9]\d* [\u4E00-\u9FFF]+$)";
}