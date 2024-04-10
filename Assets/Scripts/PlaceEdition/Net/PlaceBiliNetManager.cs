using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenBLive.Runtime;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using NativeWebSocket;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Text.RegularExpressions;

public class PlaceBiliNetManager : MonoBehaviour
{
    public string accessKeyId;
    public string accessKeySecret;
    // 项目 id
    public string appId;

    private static PlaceBiliNetManager instance;

    public static PlaceBiliNetManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlaceBiliNetManager>();
            }
            return instance;
        }
    }

    public Action ConnectSuccess;
    public Action ConnectFailure;

    // 客户端
    private WebSocketBLiveClient m_WebSocketBLiveClient;
    // 
    private InteractivePlayHeartBeat m_PlayHeartBeat;
    private string gameId;

    // code : 身份码
    public async void LinkStart(string code)
    {
        //测试的密钥
        SignUtility.accessKeySecret = accessKeySecret;
        //测试的ID
        SignUtility.accessKeyId = accessKeyId;
        var ret = await BApi.StartInteractivePlay(code, appId);
        //打印到控制台日志
        var gameIdResObj = JsonConvert.DeserializeObject<AppStartInfo>(ret);
        if (gameIdResObj.Code != 0)
        {
            Debug.LogError(gameIdResObj.Message);
            ConnectFailure?.Invoke();
            return;
        }

        m_WebSocketBLiveClient = new WebSocketBLiveClient(gameIdResObj.GetWssLink(), gameIdResObj.GetAuthBody());
        m_WebSocketBLiveClient.OnDanmaku += WebSocketBLiveClientOnDanmaku;
        m_WebSocketBLiveClient.OnGift += WebSocketBLiveClientOnGift;
        m_WebSocketBLiveClient.OnGuardBuy += WebSocketBLiveClientOnGuardBuy;
        m_WebSocketBLiveClient.OnSuperChat += WebSocketBLiveClientOnSuperChat;

        try
        {
            m_WebSocketBLiveClient.Connect(TimeSpan.FromSeconds(1), 1000000);
            ConnectSuccess?.Invoke();
            Debug.Log("连接成功");
        }
        catch (Exception ex)
        {
            ConnectFailure?.Invoke();
            Debug.Log("连接失败 : " + ex);
            throw;
        }

        gameId = gameIdResObj.GetGameId();
        m_PlayHeartBeat = new InteractivePlayHeartBeat(gameId);
        m_PlayHeartBeat.HeartBeatError += M_PlayHeartBeat_HeartBeatError;
        m_PlayHeartBeat.HeartBeatSucceed += M_PlayHeartBeat_HeartBeatSucceed;
        m_PlayHeartBeat.Start();
    }

    public async Task LinkEnd()
    {
        m_WebSocketBLiveClient.Dispose();
        m_PlayHeartBeat.Dispose();
        await BApi.EndInteractivePlay(appId, gameId);
        Debug.Log("游戏关闭");
    }

    // 醒目留言
    private void WebSocketBLiveClientOnSuperChat(SuperChat superChat)
    {
        StringBuilder sb = new StringBuilder("收到SC!");
        sb.AppendLine();
        sb.Append("来自用户：");
        sb.AppendLine(superChat.userName);
        sb.Append("留言内容：");
        sb.AppendLine(superChat.message);
        sb.Append("金额：");
        sb.Append(superChat.rmb);
        sb.Append("元");
        Debug.Log(sb);
    }

    // 大航海
    private void WebSocketBLiveClientOnGuardBuy(Guard guard)
    {
        StringBuilder sb = new StringBuilder("收到大航海!");
        sb.AppendLine();
        sb.Append("来自用户：");
        sb.AppendLine(guard.userInfo.userName);
        sb.Append("赠送了");
        sb.Append(guard.guardUnit);
        Debug.Log(sb);
    }

    // 礼物
    private void WebSocketBLiveClientOnGift(SendGift sendGift)
    {
        StringBuilder sb = new StringBuilder("收到礼物!");
        sb.AppendLine();
        sb.Append("来自用户：");
        sb.AppendLine(sendGift.userName);
        sb.Append("赠送了");
        sb.Append(sendGift.giftNum);
        sb.Append("个");
        sb.Append(sendGift.giftName);
        Debug.Log(sb);
    }

    // 弹幕
    private void WebSocketBLiveClientOnDanmaku(Dm dm)
    {
        StringBuilder sb = new StringBuilder("收到弹幕!");
        sb.AppendLine();
        sb.Append("用户：");
        sb.AppendLine(dm.userName);
        sb.Append("弹幕内容：");
        sb.Append(dm.msg);
        Debug.Log(sb);

        string msg = dm.msg.Trim();
        string username = dm.userName;
        
        // 指令 - 传统指令
        if (msg.StartsWith("/"))
        {
            PlaceInstructionManager.DefaultRunChatCommand(username,msg);
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
            PlaceInstructionManager.DefaultRunChatCommand(username,"/d " + msg);
        }else if (Regex.IsMatch(msg, FAST_LINE_PATTERN)) { // 快速画线
            PlaceInstructionManager.DefaultRunChatCommand(username,"/l " + msg);
        }else if (Regex.IsMatch(msg, FAST_DRAW_DIY_PATTERN)) { // 快速画自定义线
            PlaceInstructionManager.DefaultRunChatCommand(username,"/m " + msg);
        }else {
            // 加入
            switch (msg)
            {
                case "蓝":
                    PlaceInstructionManager.DefaultRunChatCommand(username,"/a 1");
                    // UI 信息 创建
                    break;
                case "绿":
                    PlaceInstructionManager.DefaultRunChatCommand(username,"/a 2");
                    break;
                case "黄":
                    PlaceInstructionManager.DefaultRunChatCommand(username,"/a 3");
                    break;
                case "紫":
                    PlaceInstructionManager.DefaultRunChatCommand(username,"/a 4");
                    break;
                default:
                    break;
            }
        }

    }

    // 心跳
    private static void M_PlayHeartBeat_HeartBeatSucceed()
    {
        Debug.Log("心跳成功");
    }

    private static void M_PlayHeartBeat_HeartBeatError(string json)
    {
        Debug.Log("心跳失败" + json);
    }

    private void Update()
    {
        #if !UNITY_WEBGL || UNITY_EDITOR
        if (m_WebSocketBLiveClient is { ws: { State: WebSocketState.Open } })
        {
            m_WebSocketBLiveClient.ws.DispatchMessageQueue();
        }
        #endif
    }



    private void OnDestroy()
    {
        if (m_WebSocketBLiveClient == null)
            return;

        m_PlayHeartBeat.Dispose();
        m_WebSocketBLiveClient.Dispose();
    }


    // PATTERN
    private const string FAST_DRAW_POINT_PATTERN = @"^\d{1,3} \d{1,3} \d{1,3} \d{1,3} \d{1,3}$";
    private const string FAST_DRAW_POINT_WITH_DEFAULT_COLOR_PATTERN = @"^\d{1,3} \d{1,3} [\u4E00-\u9FFF]+$";
    private const string FAST_DRAW_POINT_WITH_LAST_COLOR_PATTERN = @"^\d{1,3} \d{1,3}$";
    public const string FAST_DRAW_PATTERN = @"(^\d{1,3} \d{1,3}$)|(^\d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)";
    public const string FAST_LINE_PATTERN = @"(^\d{1,3} \d{1,3} \d{1,3} \d{1,3} [\u4E00-\u9FFF]+$)|(^\d{1,3} \d{1,3} \d{1,3} \d{1,3}$)";
    public const string FAST_DRAW_DIY_PATTERN = @"(^[1-9]\d*$)|(^[1-9]\d* [\u4E00-\u9FFF]+$)";

}
