using UnityEngine;
using OpenBLive.Runtime;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using NativeWebSocket;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections;
using UnityEditor;


public class PlaceBiliNetManager : MonoBehaviour
{
    public string accessKeyId;

    public string accessKeySecret;

    // 项目 id
    public string appId;

    public static PlaceBiliNetManager Instance { get; private set; }

    // public static PlaceBiliNetManager Instance
    // {
    //     get
    //     {
    //         if (instance == null)
    //         {
    //             instance = FindObjectOfType<PlaceBiliNetManager>();
    //             DontDestroyOnLoad(instance);
    //         }
    //         return instance;
    //     }
    // }

    public Action ConnectSuccess;
    public Action ConnectFailure;

    // 客户端
    private WebSocketBLiveClient m_WebSocketBLiveClient;

    // 
    private InteractivePlayHeartBeat m_PlayHeartBeat;
    private string gameId;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // code : 身份码
    public async void LinkStart(string code)
    {
        currentCode = code;
        //测试的密钥
        SignUtility.accessKeySecret = accessKeySecret;
        //测试的ID
        SignUtility.accessKeyId = accessKeyId;
        var ret = await BApi.StartInteractivePlay(code, appId);
        Debug.Log($"开启游戏返回数据：{ret}");
        //打印到控制台日志
        var gameIdResObj = JsonConvert.DeserializeObject<AppStartInfo>(ret);
        if (gameIdResObj.Code != 0)
        {
            Debug.LogError(gameIdResObj.Message);
            ConnectFailure?.Invoke();
            return;
        }

        // 主播信息
        //PlaceCenter.Instance.anchorName = gameIdResObj.Data.AnchorInfo.UName;

        m_WebSocketBLiveClient = new WebSocketBLiveClient(gameIdResObj.GetWssLink(), gameIdResObj.GetAuthBody());
        m_WebSocketBLiveClient.OnDanmaku += WebSocketBLiveClientOnDanmaku;
        m_WebSocketBLiveClient.OnGift += WebSocketBLiveClientOnGift;
        m_WebSocketBLiveClient.OnGuardBuy += WebSocketBLiveClientOnGuardBuy;
        m_WebSocketBLiveClient.OnSuperChat += WebSocketBLiveClientOnSuperChat;
        m_WebSocketBLiveClient.OnLike += WebSocketBliveClientOnLike;

        try
        {
            m_WebSocketBLiveClient.Connect(TimeSpan.FromSeconds(1), 100);
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
        if (m_WebSocketBLiveClient != null)
        {
            m_PlayHeartBeat.Dispose();
            m_WebSocketBLiveClient.Dispose();
        }

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

        PlaceInstructionManager.Instance.DefaultGiftCommand(sendGift.userName,
            (sendGift.price / sendGift.giftNum).ToString(), sendGift.giftNum);
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

        PlaceInstructionManager.Instance.DefaultDanmuCommand(dm);
    }

    private void WebSocketBliveClientOnLike(Like like)
    {
        StringBuilder sb = new StringBuilder("收到点赞!");
        sb.AppendLine();
        sb.Append("用户：");
        sb.AppendLine(like.uname);
        Debug.Log(sb);
        PlaceInstructionManager.Instance.DefaultLikeCommand(like);
    }

    // 心跳
    private static void M_PlayHeartBeat_HeartBeatSucceed()
    {
        Debug.Log($"查看当前ws的状态 ： {Instance.m_WebSocketBLiveClient.ws.State}");
        Debug.Log("心跳成功");
    }

    private static void M_PlayHeartBeat_HeartBeatError(string json)
    {
        Debug.Log("心跳失败" + json);
        Debug.Log($"查看当前ws的状态 ： {Instance.m_WebSocketBLiveClient.ws.State}");

        Instance.ReConnect();
    }


    private string currentCode;

    private bool isReconnect = false;

    // [MenuItem("MyMenu/Do Something")]
    private void ReConnect()
    {
        if (isReconnect)
        {
            Debug.Log("正在重连。。。。。");
            return;
        }

        try
        {
            Debug.Log("开启重连----");
            isReconnect = true;
            Instance.m_WebSocketBLiveClient.ws.OnOpen += ReConnectSuccess;
            Instance.m_WebSocketBLiveClient.Connect(TimeSpan.FromSeconds(1), 100);
            //ConnectSuccess?.Invoke();
            //Debug.Log("重接成功");
        }
        catch (Exception ex)
        {
            //ConnectFailure?.Invoke();
            Debug.Log("重接失败 : " + ex);
            isReconnect = false;
            throw;
        }
        m_PlayHeartBeat.Start();
    }

    void ReConnectSuccess()
    {
        isReconnect = false;
        Instance.m_WebSocketBLiveClient.ws.OnOpen -= ReConnectSuccess;
    }
    /*
     * IEnumerator connectToServer()
    {
        LinkEnd();
        Debug.Log("等待60s后重连");
        yield return new WaitForSeconds(61f);
        LinkStart(currentCode);
    }
    */

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
}