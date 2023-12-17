using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ChatCommandManager : MonoBehaviour
{ 

    public static ChatCommandManager Instance { get; private set; }

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
    // TeamManager teamManager;
    // PixelsContainer pixelsContainer;
    void Start()
    {
    }

    public void RunChatCommand(string command)
    {
        string[] parts = command.Trim().Split(' ');
        switch (parts[0])
        {
            case "/create":
            case "/c":
                if (parts.Length >= 3)
                {
                    TeamManager.Instance.CreateTeam(parts[1], parts[2]);
                }
                break;
            case "/add":
            case "/a":
                if (parts.Length >= 2)
                {
                    // 这里调用加入队伍的逻辑
                    TeamManager.Instance.AddTeam(parts[1]);
                }
                break;
            case "/say":
            case "/s":
                if (parts.Length >= 2)
                {
                    // 这里调用发送消息的逻辑
                    // SayMessage(string.Join(" ", parts, 1, parts.Length - 1));
                    Debug.Log(parts[1]);
                }
                break;
            // 可以在这里添加其他命令的处理
            case "/draw":
            case "/d":
                if (parts.Length >= 5)
                {
                    int x,y,r,g,b;
                    string c;
                    c = parts[0];
                    x = int.Parse(parts[1]);
                    y = int.Parse(parts[2]);
                    r = int.Parse(parts[3]);
                    g = int.Parse(parts[4]);
                    b = int.Parse(parts[5]);
                    PixelsCanvasController.Instance.DrawCommand(c,x,y,r,g,b);
                }
                else
                {
                    Debug.LogError("输入字符串格式不正确");
                }
                break;
            case "/generate":
            case "/g":
                if (parts.Length >= 4)
                {
                    int x,y;
                    string c,p;
                    c = parts[0];
                    x = int.Parse(parts[1]);
                    y = int.Parse(parts[2]);
                    p = string.Join(" ", parts.Skip(3).ToArray());
                    PixelsCanvasController.Instance.GenerateImage(x,y,p);
                }
                else
                {
                    Debug.LogError("输入字符串格式不正确");
                }
                break;
            case "/instruction":
            case "/ins":
                if (parts.Length >= 2)
                {
                    string p;
                    p = parts[1];
                    PromptManager.Instance.GenerateInstruction(p);
                }
                else
                {
                    Debug.LogError("输入字符串格式不正确");
                }
                break;
        }
    }
}

