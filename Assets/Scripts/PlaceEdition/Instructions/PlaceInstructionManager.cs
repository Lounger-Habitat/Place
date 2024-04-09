using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using Unity.VisualScripting;
using System;

public static class PlaceInstructionManager
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
    public static void DefaultRunChatCommand(string username, string command)
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
                if (!PlaceCenter.Instance.CheckUser(username))
                {
                    break;
                }
                User quickDrawUser = PlaceCenter.Instance.FindUser(username);
                if (parts.Length > 1)
                {
                    int x, y, r, g, b;
                    string c;
                    c = parts[0]; // /d
                    x = quickDrawUser.lastPoint.x; // x
                    y = quickDrawUser.lastPoint.y; // y
                    (x,y) = ComputeQuickIns(c[0],x,y);
                    r = int.Parse(parts[1]); // x
                    g = int.Parse(parts[2]); // y
                    b = int.Parse(parts[3]); // r
                    Instruction drawIns = new Instruction("/d", x, y, r: r, g: g, b: b);
                    if (!PlaceBoardManager.Instance.CheckIns(drawIns)){
                        Debug.Log("指令不合法");
                        break;
                    }
                    quickDrawUser.lastColor = new Color(r, g, b);
                    quickDrawUser.lastPoint = (x, y);
                    quickDrawUser.instructionQueue.Enqueue(drawIns);
                }
                else
                {
                    int x, y, r, g, b;
                    string c;
                    c = parts[0]; // /d
                    x = quickDrawUser.lastPoint.x; // x
                    y = quickDrawUser.lastPoint.y; // y
                    (x,y) = ComputeQuickIns(c[0],x,y);
                    Color color = quickDrawUser.lastColor;
                    r = (int)(color.r * 255); // r
                    g = (int)(color.g * 255); // g
                    b = (int)(color.b * 255); // b
                    Instruction drawIns = new Instruction("/d", x, y, r: r, g: g, b: b);
                    if (!PlaceBoardManager.Instance.CheckIns(drawIns)){
                        Debug.Log("指令不合法");
                        break;
                    }
                    quickDrawUser.lastPoint = (x, y);
                    quickDrawUser.instructionQueue.Enqueue(drawIns);
                }
                break;
            case "111":
            case "222":
            case "333":
            case "444":
                string ins = parts[0];
                string teamId = Regex.Replace(ins, @"(.)\1+", m => m.Groups[1].Value);
                PlaceCenter.Instance.JoinTeam(username, teamId);
                break;
            case "/add":
            case "/a":
                if (parts.Length >= 2)
                {
                    // 这里调用加入队伍的逻辑
                    PlaceCenter.Instance.JoinTeam(username, parts[1]);
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
                    PlaceCenter.Instance.SayMessage(username, message);
                }
                break;
            // 可以在这里添加其他命令的处理
            case "/draw":
            case "/d":
                if (!PlaceCenter.Instance.CheckUser(username))
                {
                    break;
                }
                User drawUser = PlaceCenter.Instance.FindUser(username);
                if (parts.Length >= 3)
                {
                    if (parts.Length == 3)
                    {
                        int x, y, r, g, b;
                        string c;
                        c = parts[0]; // /d
                        x = int.Parse(parts[1]); // x
                        y = int.Parse(parts[2]); // y
                        Color color = drawUser.lastColor;
                        r = (int)(color.r * 255); // r
                        g = (int)(color.g * 255); // g
                        b = (int)(color.b * 255); // b
                        Instruction drawIns = new Instruction(c, x, y, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(drawIns)){
                            Debug.Log("指令不合法");
                            break;
                        }
                        drawUser.lastPoint = (x, y);
                        drawUser.instructionQueue.Enqueue(drawIns);
                        // Debug.Log(username + " : draw " + c + " " + x + " " + y + " " + r + " " + g + " " + b);
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
                        drawUser.lastColor = new Color(r, g, b);
                        drawUser.lastPoint = (x, y);
                        drawUser.instructionQueue.Enqueue(drawIns);
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
                if (!PlaceCenter.Instance.CheckUser(username))
                {
                    break;
                }
                User lineUser = PlaceCenter.Instance.FindUser(username);
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
                        Color color = lineUser.lastColor;
                        r = (int)(color.r * 255); // r
                        g = (int)(color.g * 255); // g
                        b = (int)(color.b * 255); // b
                        Instruction ins_l = new Instruction(c, x, y, ex: ex, ey: ey, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_l)){
                            Debug.Log("指令不合法");
                            break;
                        }
                        lineUser.instructionQueue.Enqueue(ins_l);
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
                        lineUser.lastColor = new Color(r, g, b);
                        Instruction ins_l = new Instruction(c, x, y, ex: ex, ey: ey, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_l)){
                            Debug.Log("指令不合法");
                            break;
                        }
                        lineUser.instructionQueue.Enqueue(ins_l);
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
                if (!PlaceCenter.Instance.CheckUser(username))
                {
                    break;
                }
                User paintUser = PlaceCenter.Instance.FindUser(username);
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
                        Color color = paintUser.lastColor;
                        r = (int)(color.r * 255); // r
                        g = (int)(color.g * 255); // g
                        b = (int)(color.b * 255); // b
                        Instruction ins_p = new Instruction(c, x, y, dx, dy, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_p)){
                            Debug.Log("指令不合法");
                            break;
                        }
                        paintUser.instructionQueue.Enqueue(ins_p);
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
                        paintUser.lastColor = new Color(r, g, b);
                        Instruction ins_p = new Instruction(c, x, y, dx, dy, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_p)){
                            Debug.Log("指令不合法");
                            break;
                        }
                        paintUser.instructionQueue.Enqueue(ins_p);
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
                if (!PlaceCenter.Instance.CheckUser(username))
                {
                    break;
                }
                User mUser = PlaceCenter.Instance.FindUser(username);
                if (parts.Length > 2) // 多颜色
                {
                    int x, y, r, g, b;
                    string c,s;
                    c = parts[0]; // /d
                    s = parts[1]; // x
                    r = int.Parse(parts[2]); // r
                    g = int.Parse(parts[3]); // g
                    b = int.Parse(parts[4]); // b
                    (x,y) = mUser.lastPoint;
                    for (int i = 0; i < s.Length; i++)
                    {
                        char digitIns = s[i];
                        (x,y) = ComputeQuickIns(digitIns,x,y);
                        Instruction drawIns = new Instruction("/d", x, y, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(drawIns)){
                            Debug.Log("指令不合法");
                            continue;
                        }
                        mUser.instructionQueue.Enqueue(drawIns);
                        
                    }
                    mUser.lastColor = new Color(r, g, b);
                    x = Mathf.Clamp(x, 0, PlaceBoardManager.Instance.width - 1);
                    y = Mathf.Clamp(y, 0, PlaceBoardManager.Instance.height - 1);
                    mUser.lastPoint = (x, y);
                }else{ // 默认颜色
                    int x, y, r, g, b;
                    string c,s;
                    c = parts[0]; // /d
                    s = parts[1]; // x
                    (x,y) = mUser.lastPoint;
                    Color color = mUser.lastColor;
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
                        mUser.instructionQueue.Enqueue(drawIns);
                    }
                    x = Mathf.Clamp(x, 0, PlaceBoardManager.Instance.width - 1);
                    y = Mathf.Clamp(y, 0, PlaceBoardManager.Instance.height - 1);
                    mUser.lastPoint = (x, y);
                }
                break;
            case "visual":
            case "/v":
                if (!PlaceCenter.Instance.CheckUser(username))
                {
                    break;
                }
                User vUser = PlaceCenter.Instance.FindUser(username);
                if (parts.Length == 3)
                {
                    int x, y;
                    string c;
                    c = parts[0]; // /d
                    x = int.Parse(parts[1]); // x
                    y = int.Parse(parts[2]); // y
                    PlaceConsoleAreaManager.Instance.VisualAuxiliaryLine(x, y, vUser.camp);
                }
                else
                {
                    Debug.LogError("输入字符串格式不正确");
                }
                break;
            case "gs":
                 
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
    public static void DefaultGiftCommand(string username, string command) {
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

    public static (int x,int y) ComputeQuickIns(char m, int x, int y) {
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
}