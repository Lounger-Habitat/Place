using System.Collections.Generic;
using UnityEngine;
using OpenBLive.Runtime.Data;
using UnityEngine.Networking;
using System.Collections;
using System.Text.RegularExpressions;

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
                    (x, y) = ComputeQuickIns(c[0], x, y);
                    r = int.Parse(parts[1]); // x
                    g = int.Parse(parts[2]); // y
                    b = int.Parse(parts[3]); // r
                    Instruction drawIns = new Instruction("/d", x, y, r: r, g: g, b: b);
                    if (!PlaceBoardManager.Instance.CheckIns(drawIns))
                    {
                        Debug.Log("指令不合法");
                        PlaceUIManager.Instance.AddTips(new TipsItem()
                        {
                            text = $"尊敬的{user.Name},输入的指令不合法"
                        });
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
                    (x, y) = ComputeQuickIns(c[0], x, y);
                    Color color = user.lastColor;
                    r = (int)(color.r * 255); // r
                    g = (int)(color.g * 255); // g
                    b = (int)(color.b * 255); // b
                    Instruction drawIns = new Instruction("/d", x, y, r: r, g: g, b: b);
                    if (!PlaceBoardManager.Instance.CheckIns(drawIns))
                    {
                        Debug.Log("指令不合法");
                        PlaceUIManager.Instance.AddTips(new TipsItem()
                        {
                            text = $"尊敬的{user.Name},输入的指令不合法"
                        });
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
                if (parts.Length == 2)
                {
                    // 这里调用加入队伍的逻辑
                    PlaceCenter.Instance.JoinGame(user, parts[1]);
                }
                break;
            // case "/say":
            //     if (parts.Length >= 2)
            //     {
            //         // 这里调用发送消息的逻辑
            //         // SayMessage(string.Join(" ", parts, 1, parts.Length - 1));
            //         Debug.Log(parts[1]);
            //         string message = parts[1];
            //         PlaceCenter.Instance.SayMessage(user, message);
            //     }
            //     break;
            case "/draw":
            case "/d":
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
                        if (!PlaceBoardManager.Instance.CheckIns(drawIns))
                        {
                            Debug.Log($"{user.Name} , /d 指令不合法,{drawIns}");
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},输入的指令不合法"
                            });
                            break;
                        }
                        user.lastPoint = (x, y);
                        user.instructionQueue.Enqueue(drawIns);
                        // Debug.Log(username + " : draw " + c + " " + x + " " + y + " " + r + " " + g + " " + b);
                    }
                    else if (parts.Length == 4)
                    {
                        int x, y, r, g, b;
                        string c, dc;
                        Color32 color = user.lastColor;
                        c = parts[0]; // /d
                        x = int.Parse(parts[1]); // x
                        y = int.Parse(parts[2]); // y
                        dc = parts[3];
                        if (colorDict.ContainsKey(dc))
                        {
                            color = colorDict[dc];
                            r = color.r; // r
                            g = color.g; // g
                            b = color.b; // b
                        }
                        else
                        {
                            Debug.Log("抱歉此颜色目前未包含在,可联系管理员申请新增颜色");
                            // UI 提示
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},抱歉此颜色({dc})目前未包含在内,可联系管理员申请新增颜色"
                            });
                            break;
                        }
                        Instruction drawIns = new Instruction(c, x, y, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(drawIns))
                        {
                            Debug.Log($"{user.Name} , /d 指令不合法,{drawIns}");
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},输入的指令不合法"
                            });
                            break;
                        }
                        user.lastPoint = (x, y);
                        user.lastColor = color;
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
                        if (!PlaceBoardManager.Instance.CheckIns(drawIns))
                        {
                            Debug.Log($"{user.Name} , /d 指令不合法,{drawIns}");
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},输入的指令不合法"
                            });
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

                        // 暂时保留 仅用做 合法检测
                        Instruction ins_l = new Instruction(c, x, y, ex: ex, ey: ey, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_l))
                        {
                            Debug.Log($"{user.Name} , /l 指令不合法,{ins_l}");
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},输入的指令不合法"
                            });
                            break;
                        }




                        if (user.level > 50)
                        {
                            // ======= 一笔画 ========
                            user.instructionQueue.Enqueue(ins_l);
                        }
                        else
                        {
                            // ======= Line2point =======
                            List<(int, int)> points = PlaceBoardManager.Instance.GetLinePoints(x, y, ex, ey);
                            foreach ((int, int) point in points)
                            {
                                Instruction drawIns = new Instruction("/d", point.Item1, point.Item2, r: r, g: g, b: b);
                                user.instructionQueue.Enqueue(drawIns);
                            }
                        }

                        // 手动 输入 指令 + 大贡献
                        user.score += 30;
                        user.lastPoint = (ex, ey);


                    }
                    else if (parts.Length == 6)
                    {
                        // 文字颜色 - dc
                        string c, dc;
                        int x, y, ex, ey, r, g, b;
                        Color32 color = user.lastColor;
                        c = parts[0]; // /l
                        x = int.Parse(parts[1]); // x
                        y = int.Parse(parts[2]); // y
                        ex = int.Parse(parts[3]); // ex
                        ey = int.Parse(parts[4]); // ey
                        dc = parts[5];
                        if (colorDict.ContainsKey(dc))
                        {
                            color = colorDict[dc];
                            r = color.r; // r
                            g = color.g; // g
                            b = color.b; // b
                        }
                        else
                        {
                            Debug.Log("抱歉此颜色目前未包含在,可联系管理员申请新增颜色");
                            // UI 提示
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},抱歉此颜色({dc})目前未包含在,可联系管理员申请新增颜色"
                            });
                            break;
                        }
                        // 暂时保留 仅用做 合法检测
                        Instruction ins_l = new Instruction(c, x, y, ex: ex, ey: ey, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_l))
                        {
                            Debug.Log($"{user.Name} , /l 指令不合法,{ins_l}");
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},输入的指令不合法"
                            });
                            break;
                        }



                        if (user.level < 50)
                        {

                            // ======= Line2point =======

                            List<(int, int)> points = PlaceBoardManager.Instance.GetLinePoints(x, y, ex, ey);
                            foreach ((int, int) point in points)
                            {
                                Instruction drawIns = new Instruction("/d", point.Item1, point.Item2, r: r, g: g, b: b);
                                user.instructionQueue.Enqueue(drawIns);
                            }
                        }
                        else
                        {
                            // ======= 一笔画 ========
                            user.instructionQueue.Enqueue(ins_l);
                        }

                        user.score += 30;
                        user.lastColor = color;
                        user.lastPoint = (ex, ey);


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
                        // 暂时保留 仅用做 合法检测
                        Instruction ins_l = new Instruction(c, x, y, ex: ex, ey: ey, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_l))
                        {
                            Debug.Log($"{user.Name} , /l 指令不合法,{ins_l}");
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},输入的指令不合法"
                            });
                            break;
                        }

                        if (user.level < 50)
                        {

                            // ======= Line2point =======

                            List<(int, int)> points = PlaceBoardManager.Instance.GetLinePoints(x, y, ex, ey);
                            foreach ((int, int) point in points)
                            {
                                Instruction drawIns = new Instruction("/d", point.Item1, point.Item2, r: r, g: g, b: b);
                                user.instructionQueue.Enqueue(drawIns);
                            }
                        }
                        else
                        {
                            // ======= 一笔画 ========
                            user.instructionQueue.Enqueue(ins_l);
                        }
                        user.score += 30;
                        user.lastPoint = (ex, ey);
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
                // break; // 2024-05-06 :⚠️ 暂时废弃 
                // 2024-05-07 , 改主意了，又不废弃了
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
                        // 暂时保留 仅用做 合法检测
                        Instruction ins_p = new Instruction(c, x, y, dx: dx, dy: dy, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_p))
                        {
                            Debug.Log($"{user.Name} , /p 指令不合法,{ins_p}");
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},输入的指令不合法"
                            });
                            break;
                        }

                        // ======= Square2point =======
                        List<(int, int)> points = PlaceBoardManager.Instance.GetSquarePoints(x, y, dx, dy);
                        foreach ((int, int) point in points)
                        {
                            Instruction drawIns = new Instruction("/d", point.Item1, point.Item2, r: r, g: g, b: b);
                            user.instructionQueue.Enqueue(drawIns);
                        }
                        user.score += 30;
                    }
                    else if (parts.Length == 6)
                    {
                        int x, y, dx, dy, r, g, b;
                        string c, dc;
                        Color32 color = user.lastColor;
                        c = parts[0]; // /l
                        x = int.Parse(parts[1]); // x
                        y = int.Parse(parts[2]); // y
                        dx = int.Parse(parts[3]); // dx
                        dy = int.Parse(parts[4]); // dy
                        dc = parts[5];
                        if (colorDict.ContainsKey(dc))
                        {
                            color = colorDict[dc];
                            r = color.r; // r
                            g = color.g; // g
                            b = color.b; // b
                        }
                        else
                        {
                            Debug.Log("抱歉此颜色目前未包含在,使用上一次颜色,可联系管理员申请新增颜色");
                            // UI 提示
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},抱歉此颜色({dc})目前未包含在,可联系管理员申请新增颜色"
                            });
                            break;
                        }
                        // 暂时保留 仅用做 合法检测
                        Instruction ins_p = new Instruction(c, x, y, dx: dx, dy: dy, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_p))
                        {
                            Debug.Log($"{user.Name} , /p 指令不合法,{ins_p}");
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},输入的指令不合法"
                            });
                            break;
                        }

                        // ======= Square2point =======
                        List<(int, int)> points = PlaceBoardManager.Instance.GetSquarePoints(x, y, dx, dy);
                        foreach ((int, int) point in points)
                        {
                            Instruction drawIns = new Instruction("/d", point.Item1, point.Item2, r: r, g: g, b: b);
                            user.instructionQueue.Enqueue(drawIns);
                        }
                        user.score += 30;
                        user.lastColor = color;
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
                        // 暂时保留 仅用做 合法检测
                        Instruction ins_p = new Instruction(c, x, y, dx: dx, dy: dy, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_p))
                        {
                            Debug.Log($"{user.Name} , /p 指令不合法,{ins_p}");
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},输入的指令不合法"
                            });
                            break;
                        }

                        // ======= Square2point =======
                        List<(int, int)> points = PlaceBoardManager.Instance.GetSquarePoints(x, y, dx, dy);
                        foreach ((int, int) point in points)
                        {
                            Instruction drawIns = new Instruction("/d", point.Item1, point.Item2, r: r, g: g, b: b);
                            user.instructionQueue.Enqueue(drawIns);
                        }
                        user.score += 30;
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
            case "/rect": // 矩形 无填充
            case "/r":
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
                        // 暂时保留 仅用做 合法检测
                        Instruction ins_rect = new Instruction(c, x, y, dx: dx, dy: dy, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_rect))
                        {
                            Debug.Log($"{user.Name} , /rect 指令不合法,{ins_rect}");
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},输入的指令不合法"
                            });
                            break;
                        }

                        // ======= Rect2point =======
                        if (user.level < 50)
                        {
                            List<(int, int)> points = PlaceBoardManager.Instance.GetRectPoints(x, y, dx, dy);
                            foreach ((int, int) point in points)
                            {
                                Instruction drawIns = new Instruction("/d", point.Item1, point.Item2, r: r, g: g, b: b);
                                user.instructionQueue.Enqueue(drawIns);
                            }
                        }
                        else
                        {
                            // ====== Rect2Line =======
                            List<(int, int, int, int)> lines = PlaceBoardManager.Instance.GetRectLines(x, y, dx, dy);
                            foreach ((int, int, int, int) line in lines)
                            {
                                Instruction lineIns = new Instruction("/l", x: line.Item1, y: line.Item2, ex: line.Item3, ey: line.Item4, r: r, g: g, b: b);
                                user.instructionQueue.Enqueue(lineIns);
                            }
                        }
                        user.score += 30;
                    }
                    else if (parts.Length == 6)
                    {
                        int x, y, dx, dy, r, g, b;
                        string c, dc;
                        Color32 color = user.lastColor;
                        c = parts[0]; // /l
                        x = int.Parse(parts[1]); // x
                        y = int.Parse(parts[2]); // y
                        dx = int.Parse(parts[3]); // dx
                        dy = int.Parse(parts[4]); // dy
                        dc = parts[5];
                        if (colorDict.ContainsKey(dc))
                        {
                            color = colorDict[dc];
                            r = color.r; // r
                            g = color.g; // g
                            b = color.b; // b
                        }
                        else
                        {
                            Debug.Log("抱歉此颜色目前未包含在,使用上一次颜色,可联系管理员申请新增颜色");
                            // UI 提示
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},抱歉此颜色({dc})目前未包含在,可联系管理员申请新增颜色"
                            });
                            break;
                        }
                        // 暂时保留 仅用做 合法检测
                        Instruction ins_rect = new Instruction(c, x: x, y: y, dx: dx, dy: dy, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_rect))
                        {
                            Debug.Log($"{user.Name} , /rect 指令不合法,{ins_rect}");
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},输入的指令不合法"
                            });
                            break;
                        }

                        // ======= Rect2point =======
                        if (user.level < 50)
                        {
                            List<(int, int)> points = PlaceBoardManager.Instance.GetRectPoints(x, y, dx, dy);
                            foreach ((int, int) point in points)
                            {
                                Instruction drawIns = new Instruction("/d", point.Item1, point.Item2, r: r, g: g, b: b);
                                user.lastColor = color;
                                user.instructionQueue.Enqueue(drawIns);
                            }
                        }
                        else
                        {
                            // ====== Rect2Line =======
                            List<(int, int, int, int)> lines = PlaceBoardManager.Instance.GetRectLines(x, y, dx, dy);
                            foreach ((int, int, int, int) line in lines)
                            {
                                Instruction lineIns = new Instruction("/l", x: line.Item1, y: line.Item2, ex: line.Item3, ey: line.Item4, r: r, g: g, b: b);
                                user.lastColor = color;
                                user.instructionQueue.Enqueue(lineIns);
                            }
                        }
                        user.score += 30;
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
                        // 暂时保留 仅用做 合法检测
                        Instruction ins_rect = new Instruction(c, x: x, y: y, dx: dx, dy: dy, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(ins_rect))
                        {
                            Debug.Log($"{user.Name} , /rect 指令不合法,{ins_rect}");
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},输入的指令不合法"
                            });
                            break;
                        }

                        // ======= Rect2point =======
                        if (user.level < 50)
                        {
                            List<(int, int)> points = PlaceBoardManager.Instance.GetRectPoints(x, y, dx, dy);
                            foreach ((int, int) point in points)
                            {
                                Instruction drawIns = new Instruction("/d", point.Item1, point.Item2, r: r, g: g, b: b);
                                user.instructionQueue.Enqueue(drawIns);
                            }
                        }
                        else
                        {
                            // ====== Rect2Line =======
                            List<(int, int, int, int)> lines = PlaceBoardManager.Instance.GetRectLines(x, y, dx, dy);
                            foreach ((int, int, int, int) line in lines)
                            {
                                Instruction lineIns = new Instruction("/l", x: line.Item1, y: line.Item2, ex: line.Item3, ey: line.Item4, r: r, g: g, b: b);
                                user.instructionQueue.Enqueue(lineIns);
                            }
                        }
                        user.score += 30;

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
            case "/c":
                if (parts.Length == 4)
                {
                    int x, y, r;
                    string c;
                    c = parts[0]; // /d
                    x = int.Parse(parts[1]); // x
                    y = int.Parse(parts[2]); // y
                    r = int.Parse(parts[3]); // radius
                    Color32 color = user.lastColor;

                    // 检测 合法
                    if (x + r >= PlaceBoardManager.Instance.width || y + r >= PlaceBoardManager.Instance.height || x - r < 0 || y - r < 0)
                    {
                        Debug.Log($"{user.Name} , /c 指令不合法,L4");
                        PlaceUIManager.Instance.AddTips(new TipsItem()
                        {
                            text = $"尊敬的{user.Name},输入的指令不合法"
                        });
                        break;
                    }

                    // ======= Square2point =======
                    List<(int, int)> points = PlaceBoardManager.Instance.GetCirclePoints(x, y, r);
                    foreach ((int, int) point in points)
                    {
                        Instruction drawIns = new Instruction("/d", point.Item1, point.Item2, r: color.r, g: color.g, b: color.b);
                        user.instructionQueue.Enqueue(drawIns);
                    }
                    user.score += 30;
                }
                else if (parts.Length == 5)
                {
                    int x, y, r, g, b, radius;
                    string c, dc;
                    Color32 color = user.lastColor;
                    c = parts[0]; // /d
                    x = int.Parse(parts[1]); // x
                    y = int.Parse(parts[2]); // y
                    radius = int.Parse(parts[3]);
                    dc = parts[4];
                    if (colorDict.ContainsKey(dc))
                    {
                        color = colorDict[dc];
                        r = color.r; // r
                        g = color.g; // g
                        b = color.b; // b
                    }
                    else
                    {
                        Debug.Log("抱歉此颜色目前未包含在,可联系管理员申请新增颜色");
                        // UI 提示
                        PlaceUIManager.Instance.AddTips(new TipsItem()
                        {
                            text = $"尊敬的{user.Name},抱歉此颜色({dc})目前未包含在,可联系管理员申请新增颜色"
                        });
                        break;
                    }
                    // 检测 合法
                    if (x + radius >= PlaceBoardManager.Instance.width || y + radius >= PlaceBoardManager.Instance.height || x - radius < 0 || y - radius < 0)
                    {
                        Debug.Log($"{user.Name} , /c 指令不合法,L5");
                        PlaceUIManager.Instance.AddTips(new TipsItem()
                        {
                            text = $"尊敬的{user.Name},输入的指令不合法"
                        });
                        break;
                    }

                    // ======= Square2point =======
                    List<(int, int)> points = PlaceBoardManager.Instance.GetCirclePoints(x, y, radius);
                    foreach ((int, int) point in points)
                    {
                        Instruction drawIns = new Instruction("/d", point.Item1, point.Item2, r: r, g: g, b: b);
                        user.lastColor = color;
                        user.instructionQueue.Enqueue(drawIns);
                    }
                    user.score += 30;
                }
                else if (parts.Length == 7)
                {
                    int x, y, r, g, b, radius;
                    string c;
                    c = parts[0]; // /d
                    x = int.Parse(parts[1]); // x
                    y = int.Parse(parts[2]); // y
                    radius = int.Parse(parts[3]);
                    r = int.Parse(parts[4]); // r
                    g = int.Parse(parts[5]); // g
                    b = int.Parse(parts[6]); // b
                    user.lastColor = new Color(r, g, b);

                    // 检测 合法
                    if (x + radius >= PlaceBoardManager.Instance.width || y + radius >= PlaceBoardManager.Instance.height || x - radius < 0 || y - radius < 0)
                    {
                        Debug.Log($"{user.Name} , /c 指令不合法,L7");
                        PlaceUIManager.Instance.AddTips(new TipsItem()
                        {
                            text = $"尊敬的{user.Name},输入的/c指令不合法"
                        });
                        break;
                    }

                    // ======= Square2point =======
                    List<(int, int)> points = PlaceBoardManager.Instance.GetCirclePoints(x, y, radius);
                    foreach ((int, int) point in points)
                    {
                        Instruction drawIns = new Instruction("/d", point.Item1, point.Item2, r: r, g: g, b: b);
                        user.instructionQueue.Enqueue(drawIns);
                    }
                    user.score += 30;
                }
                else
                {
                    Debug.LogError("输入字符串格式不正确");
                }
                break;
            case "/m":
                if (parts.Length == 2) // 默认颜色
                {
                    int x, y, r, g, b;
                    string c, s;
                    c = parts[0]; // /d
                    s = parts[1]; // x
                    (x, y) = user.lastPoint;
                    Color color = user.lastColor;
                    r = (int)(color.r * 255); // r
                    g = (int)(color.g * 255); // g
                    b = (int)(color.b * 255); // b
                    for (int i = 0; i < s.Length; i++)
                    {
                        char digitIns = s[i];
                        (x, y) = ComputeQuickIns(digitIns, x, y);
                        Instruction drawIns = new Instruction("/d", x, y, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(drawIns))
                        {
                            Debug.Log($"{user.Name} , /m 指令不合法,{drawIns}");
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},输入的/m指令不合法"
                            });
                            continue;
                        }
                        user.instructionQueue.Enqueue(drawIns);
                    }
                    x = Mathf.Clamp(x, 0, PlaceBoardManager.Instance.width - 1);
                    y = Mathf.Clamp(y, 0, PlaceBoardManager.Instance.height - 1);
                    user.lastPoint = (x, y);
                }
                else if (parts.Length == 3)
                {
                    int x, y, r, g, b;
                    string c, s, dc;
                    Color32 color = user.lastColor;
                    c = parts[0]; // /d
                    s = parts[1]; // seq
                    dc = parts[2];
                    (x, y) = user.lastPoint;
                    if (colorDict.ContainsKey(dc))
                    {
                        color = colorDict[dc];
                        r = color.r; // r
                        g = color.g; // g
                        b = color.b; // b
                    }
                    else
                    {
                        Debug.Log("抱歉此颜色目前未包含在,可联系管理员申请新增颜色");
                        // UI 提示
                        PlaceUIManager.Instance.AddTips(new TipsItem()
                        {
                            text = $"尊敬的{user.Name},抱歉此颜色({dc})目前未包含在,可联系管理员申请新增颜色"
                        });
                        break;
                    }
                    for (int i = 0; i < s.Length; i++)
                    {
                        char digitIns = s[i];
                        (x, y) = ComputeQuickIns(digitIns, x, y);
                        Instruction drawIns = new Instruction("/d", x, y, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(drawIns))
                        {
                            Debug.Log($"{user.Name} , /m 指令不合法,{drawIns}");
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},输入的指令不合法"
                            });
                            continue;
                        }
                        user.instructionQueue.Enqueue(drawIns);
                    }
                    x = Mathf.Clamp(x, 0, PlaceBoardManager.Instance.width - 1);
                    y = Mathf.Clamp(y, 0, PlaceBoardManager.Instance.height - 1);
                    user.lastPoint = (x, y);
                    user.lastColor = color;
                    user.score += 30;
                }
                else if (parts.Length == 5)
                { // 多颜色
                    int x, y, r, g, b;
                    string c, s;
                    c = parts[0]; // /d
                    s = parts[1]; // x
                    r = int.Parse(parts[2]); // r
                    g = int.Parse(parts[3]); // g
                    b = int.Parse(parts[4]); // b
                    (x, y) = user.lastPoint;
                    for (int i = 0; i < s.Length; i++)
                    {
                        char digitIns = s[i];
                        (x, y) = ComputeQuickIns(digitIns, x, y);
                        Instruction drawIns = new Instruction("/d", x, y, r: r, g: g, b: b);
                        if (!PlaceBoardManager.Instance.CheckIns(drawIns))
                        {
                            Debug.Log($"{user.Name} , /m 指令不合法,{drawIns}");
                            PlaceUIManager.Instance.AddTips(new TipsItem()
                            {
                                text = $"尊敬的{user.Name},输入的指令不合法"
                            });
                            continue;
                        }
                        user.instructionQueue.Enqueue(drawIns);

                    }
                    user.lastColor = new Color(r, g, b);
                    x = Mathf.Clamp(x, 0, PlaceBoardManager.Instance.width - 1);
                    y = Mathf.Clamp(y, 0, PlaceBoardManager.Instance.height - 1);
                    user.lastPoint = (x, y);
                    user.score += 30;
                }
                break;
            case "visual":
            case "/v":
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
            // case "gs":
            //     break;
            // case "/take": // 拿别人颜料 ，谁家
            // case "/t":
            //     break;
            // case "/defense": // 防止别人拿 ，状态
            // case "/de":
            //     break;
            // case "/kill": // 除掉守护者，谁家
            // case "/k":
            //     break;
            case "/roll":
                if (parts.Length == 5)
                {
                    int x, y, max;
                    string c,name;
                    c = parts[0]; // /roll
                    x = int.Parse(parts[1]); // x
                    y = int.Parse(parts[2]); // y
                    max = int.Parse(parts[3]); // max
                    name = parts[4]; // name
                    List<Instruction> IL = PlaceCenter.Instance.FreeGenerateImage(x, y, max,name);
                    if (IL.Count != 0)
                    {
                        IL.ForEach(i => user.instructionQueue.Enqueue(i));
                    }else {
                        Debug.Log("roll 失败,或许名字key 不对");
                    }
                }
                else if (parts.Length == 6)
                {
                    int x, y, max;
                    string c,name,mode;
                    c = parts[0]; // /roll
                    x = int.Parse(parts[1]); // x
                    y = int.Parse(parts[2]); // y
                    max = int.Parse(parts[3]); // max
                    name = parts[4]; // name
                    mode = parts[5]; // mode : gift
                    if (mode == "gift")
                    {
                        Texture2D userTex = user.userIcon.texture;
                        List<Instruction> IL = PlaceCenter.Instance.GiftGenerateImage(x, y, max,userTex,name);
                        if (IL.Count != 0)
                        {
                            IL.ForEach(i => user.instructionQueue.Enqueue(i));
                        }else {
                            Debug.Log("roll 失败,或许名字key 不对");
                        }
                    }
                    // List<Instruction> IL = PlaceCenter.Instance.FreeGenerateImage(x, y, max,name);
                    // if (IL.Count != 0)
                    // {
                    //     IL.ForEach(i => user.instructionQueue.Enqueue(i));
                    // }else {
                    //     Debug.Log("roll 失败,或许名字key 不对");
                    // }
                }
                // 从 已有的 图集 中 找一个 图
                // 把 图 -> 指令
                // 给到 player
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
            // case "/f":
            //     break; // 2024-05-09 :⚠️ 暂时废弃
                       // if (parts.Length == 4)
                       // {
                       //     int x, y, r, g, b;
                       //     string c, dc;
                       //     c = parts[0]; // /d
                       //     x = int.Parse(parts[1]); // x
                       //     y = int.Parse(parts[2]); // y
                       //     dc = parts[3];
                       //     if (colorDict.ContainsKey(dc))
                       //     {
                       //         Color32 color = colorDict[dc];
                       //         r = color.r; // r
                       //         g = color.g; // g
                       //         b = color.b; // b
                       //     }
                       //     else
                       //     {
                       //         Debug.Log("抱歉此颜色目前未包含在,可联系管理员申请新增颜色");
                       //         // UI 提示
                       //         break;
                       //     }
                       //     List<(int, int)> points = PlaceBoardManager.Instance.GetFillPoints(x, y, user.Id);
                       //     foreach ((int, int) point in points)
                       //     {
                       //         Instruction drawIns = new Instruction("/d", point.Item1, point.Item2, r: r, g: g, b: b);
                       //         user.instructionQueue.Enqueue(drawIns);
                       //     }
                       // }
                       // break;
            // case "/test":
            //     if (parts.Length == 3)
            //     {
            //         string c, id, message;
            //         c = parts[0]; // /d
            //         id = parts[1]; // name
            //         message = parts[2]; // y
            //         // Debug.Log($"{c} {name} {message}");
            //         string name = FindById(id);
            //         if (name == "")
            //         {
            //             Debug.Log("未找到对应的用户");
            //             return;
            //         }
            //         DefaultGiftCommand(name, message, 1);
            //     }
            //     else if (parts.Length == 4)
            //     {
            //         string c, id;
            //         long count;
            //         c = parts[0]; // /d
            //         id = parts[1]; // name
            //         count = long.Parse(parts[3]); // count
            //         string name = FindById(id);
            //         User u = PlaceCenter.Instance.users[name];
            //         // Debug.Log($"{c} {name} {count}");
            //         PlaceCenter.Instance.GainLikePower(u, count);
            //     }
            //     // 从 已有的 图集 中 找一个 图
            //     // 把 图 -> 指令
            //     // 给到 player
            //     break;
        }
    }
    string FindById(string id)
    {
        foreach (var item in PlaceCenter.Instance.users)
        {
            if (item.Value.Id.ToString() == id)
            {
                return item.Key;
            }
        }
        return "";
    }
    public void DefaultGiftCommand(string username, string command, long num)
    {
        // 找到 对应 的 礼物
        string parts = command.Trim();
        float message = float.Parse(parts);
        StartCoroutine(GainMultiPower(username, message, num));
        // PlaceCenter.Instance.GainPower(username, message);
    }

    IEnumerator GainMultiPower(string username, float message, long num)
    {
        for (int i = 0; i < num; i++)
        {
            PlaceCenter.Instance.GainPower(username, message);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void DefaultLikeCommand(Like like)
    {
        string username = like.uname;
        if (PlaceCenter.Instance.users.ContainsKey(username))
        {
            // 获取用户
            User user = PlaceCenter.Instance.users[username];
            long count = like.unamelike_count;
            // 指令 - 传统指令
            PlaceCenter.Instance.GainLikePower(user, count);
        }


        // PlaceCenter.Instance.GainPower(username, 1);
    }

    public (int x, int y) ComputeQuickIns(char m, int x, int y)
    {
        switch (m)
        {
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
    public void DefaultDanmuCommand(Dm dm)
    {
        string username = dm.userName;
        string msg = dm.msg.Trim();
        if (PlaceCenter.Instance.users.ContainsKey(username))
        {
            // 获取用户
            User user = PlaceCenter.Instance.users[username];
            // 指令 - 传统指令
            if (msg.StartsWith("/"))
            {
                DefaultRunChatCommand(user, msg);
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
            if (Regex.IsMatch(msg, FAST_DRAW_PATTERN))
            { // 快速画点
                DefaultRunChatCommand(user, "/d " + msg);
            }
            else if (Regex.IsMatch(msg, FAST_LINE_PATTERN))
            { // 快速画线
                DefaultRunChatCommand(user, "/l " + msg);
            }
            else if (Regex.IsMatch(msg, FAST_DRAW_DIY_PATTERN))
            { // 快速画自定义线
                DefaultRunChatCommand(user, "/m " + msg);
            }
            else if (Regex.IsMatch(msg, FAST_CIRCLE_PATTERN))
            {// 快速画圆
                DefaultRunChatCommand(user, "/c " + msg);
            }

            // no slash
            if (Regex.IsMatch(msg, FAST_DRAW_POINT_PATTERN_NO_SLASH))
            { // 快速画点
                DefaultRunChatCommand(user, "/" + msg);
            }
            else if (Regex.IsMatch(msg, FAST_LINE_PATTERN_NO_SLASH))
            { // 快速画线
                DefaultRunChatCommand(user, "/" + msg);
            }
            else if (Regex.IsMatch(msg, FAST_DRAW_DIY_PATTERN_NO_SLASH))
            { // 快速画自定义线
                DefaultRunChatCommand(user, "/" + msg);
            }
            else if (Regex.IsMatch(msg, FAST_CIRCLE_PATTERN_NO_SLASH))
            {// 快速画圆
                DefaultRunChatCommand(user, "/" + msg);
            }else if (Regex.IsMatch(msg, FAST_PAINT_PATTERN_NO_SLASH))
            {// 快速画方块
                DefaultRunChatCommand(user, "/" + msg);
            }else if (Regex.IsMatch(msg, FAST_RECT_PATTERN_NO_SLASH))
            {// 快速画rect
                DefaultRunChatCommand(user, "/" + msg);
            }
            
            // chinese
            if (Regex.IsMatch(msg, FAST_DRAW_POINT_PATTERN_CHINESE))
            { // 快速画点
                msg = msg.Replace("点", "d");
                DefaultRunChatCommand(user, "/" + msg);
            }
            else if (Regex.IsMatch(msg, FAST_LINE_PATTERN_CHINESE))
            { // 快速画线
                msg = msg.Replace("线", "l");
                DefaultRunChatCommand(user, "/" + msg);
            }
            else if (Regex.IsMatch(msg, FAST_CIRCLE_PATTERN_CHINESE))
            {// 快速画圆
                msg = msg.Replace("圆", "c");
                DefaultRunChatCommand(user, "/" + msg);
            }else if (Regex.IsMatch(msg, FAST_PAINT_PATTERN_CHINESE))
            {// 快速画方块
                msg = msg.Replace("面", "paint");
                DefaultRunChatCommand(user, "/" + msg);
            }else if (Regex.IsMatch(msg, FAST_RECT_PATTERN_CHINESE))
            {// 快速画rect
                msg = msg.Replace("矩形", "rect");
                DefaultRunChatCommand(user, "/" + msg);
            }
        }
        // List<string> selectList = new List<string>(){
        //     "蓝",
        //     "绿",
        //     "黄",
        //     "紫",
        // };

        // 第一次 加入游戏
        string firstJoinFormat = @"1|11|111|2|22|222|蓝|绿|/a \d";

        if (Regex.IsMatch(msg, firstJoinFormat))
        {
            string userFace = dm.userFace;
            User user = new User(username);
            StartCoroutine(DownloadImage(user, userFace));
            // 加入
            if (msg.StartsWith("/a"))
            {
                user.Camp = int.Parse(Regex.Match(msg, @"\d+").Value);
                user.Id = PlaceCenter.Instance.GenId();
                DefaultRunChatCommand(user, msg);
            }
            else
            {
                switch (msg)
                {
                    case "蓝":
                    case "1":
                    case "11":
                    case "111":
                        user.Camp = 1;
                        DefaultRunChatCommand(user, "/a 1");
                        // 需要下载资源
                        // UI 信息 创建
                        break;
                    case "绿":
                    case "2":
                    case "22":
                    case "222":
                        user.Camp = 2;
                        DefaultRunChatCommand(user, "/a 2");
                        break;
                    case "黄":
                        user.Camp = 3;
                        DefaultRunChatCommand(user, "/a 3");
                        break;
                    case "紫":
                        user.Camp = 4;
                        DefaultRunChatCommand(user, "/a 4");
                        break;
                    default:
                        break;
                }
            }

        }

    }


    /*
        ========= 协程 ========
    */
    IEnumerator DownloadImage(User user, string url)
    {
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
    // 常用 - 14色 +3 +2
    public Dictionary<string, Color32> colorDict = new Dictionary<string, Color32>(){
        // 无色彩系
        {"白", new Color32(255, 255, 255, 255)},
        {"银", new Color32(192, 192, 192, 255)},
        {"灰", new Color32(128, 128, 128, 255)},
        {"黑", new Color32(0, 0, 0, 255)},

        // 红
        {"红", new Color32(255, 0, 0, 255)},
        {"洋红", new Color32(255, 0, 255, 255)},
        {"橙", new Color32(255, 102, 0, 255)},
        {"粉", new Color32(255, 192, 203, 255)},

        // 绿
        {"绿", new Color32(0, 128, 0, 255)},
        {"浅绿", new Color32(144, 238, 144, 255)},
        {"鲜绿", new Color32(0, 255, 0, 255)},

        // 蓝
        {"蓝", new Color32(0, 0, 255, 255)},
        {"浅蓝", new Color32(173, 216, 230, 255)},
        {"蔚蓝", new Color32(0, 123, 167, 255)},
        {"天蓝", new Color32(0, 127, 255, 255)},

        {"黄", new Color32(255, 255, 0, 255)},
        {"紫", new Color32(106,  13  ,173, 255)},
        {"青", new Color32(0, 255, 255, 255)},


        {"棕", new Color32(165, 42, 42, 255)},
        {"金", new Color32(255, 215, 0, 255)},

    };

    // 中国色 - 526色
    public Dictionary<string, Color32> chineseColorDict = new Dictionary<string, Color32>(){
        {"白", new Color32(255, 255, 255, 255)},
    };


    // PATTERN
    private const string FAST_DRAW_POINT_PATTERN = @"^\d{1,3} \d{1,3} \d{1,3} \d{1,3} \d{1,3}$";
    private const string FAST_DRAW_POINT_WITH_DEFAULT_COLOR_PATTERN = @"^\d{1,3} \d{1,3} [\u4E00-\u9FFF]+$";
    private const string FAST_DRAW_POINT_WITH_LAST_COLOR_PATTERN = @"^\d{1,3} \d{1,3}$";


    //  use
    public const string FAST_DRAW_PATTERN = @"(^\d{1,3} \d{1,3}$)|(^\d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)";
    public const string FAST_LINE_PATTERN = @"(^\d{1,3} \d{1,3} \d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)|(^\d{1,3} \d{1,3} \d{1,3} \d{1,3}$)";
    public const string FAST_DRAW_DIY_PATTERN = @"(^[1-9]\d*$)|(^[1-9]\d* [\u4E00-\u9FFF]+$)";
    public const string FAST_CIRCLE_PATTERN = @"(^\d{1,3} \d{1,3} \d{1,3}$)|(^\d{1,3} \d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)";
    public const string FAST_PAINT_PATTERN = @"(^\d{1,3} \d{1,3} \d{1,3} \d{1,3}$)|(^\d{1,3} \d{1,3} \d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)";
    public const string FAST_RECT_PATTERN = @"(^\d{1,3} \d{1,3} \d{1,3} \d{1,3}$)|(^\d{1,3} \d{1,3} \d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)";

    // fast no slash
    public const string FAST_DRAW_POINT_PATTERN_NO_SLASH = @"(^d \d{1,3} \d{1,3}$)|(^d \d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)";
    public const string FAST_LINE_PATTERN_NO_SLASH = @"(^l \d{1,3} \d{1,3} \d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)|(^l \d{1,3} \d{1,3} \d{1,3} \d{1,3}$)";
    public const string FAST_DRAW_DIY_PATTERN_NO_SLASH = @"(^m [1-9]\d*$)|(^m [1-9]\d* [\u4E00-\u9FFF]+$)";
    public const string FAST_CIRCLE_PATTERN_NO_SLASH = @"(^c \d{1,3} \d{1,3} \d{1,3}$)|(^c \d{1,3} \d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)";
    public const string FAST_PAINT_PATTERN_NO_SLASH = @"(^p \d{1,3} \d{1,3} \d{1,3} \d{1,3}$)|(^p \d{1,3} \d{1,3} \d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)";
    public const string FAST_RECT_PATTERN_NO_SLASH = @"(^r \d{1,3} \d{1,3} \d{1,3} \d{1,3}$)|(^r \d{1,3} \d{1,3} \d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)";

    public const string FAST_DRAW_POINT_PATTERN_CHINESE = @"(^点 \d{1,3} \d{1,3}$)|(^点 \d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)";
    public const string FAST_LINE_PATTERN_CHINESE = @"(^线 \d{1,3} \d{1,3} \d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)|(^线 \d{1,3} \d{1,3} \d{1,3} \d{1,3}$)";
    public const string FAST_CIRCLE_PATTERN_CHINESE = @"(^圆 \d{1,3} \d{1,3} \d{1,3}$)|(^圆 \d{1,3} \d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)";
    public const string FAST_PAINT_PATTERN_CHINESE = @"(^面 \d{1,3} \d{1,3} \d{1,3} \d{1,3}$)|(^p \d{1,3} \d{1,3} \d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)";
    public const string FAST_RECT_PATTERN_CHINESE = @"(^方框 \d{1,3} \d{1,3} \d{1,3} \d{1,3}$)|(^方框 \d{1,3} \d{1,3} \d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)";


}