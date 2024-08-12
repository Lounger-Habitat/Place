using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ByteDance.Live.Foundation.Logging;
using ByteDance.LiveOpenSdk.Push;
using ByteDance.LiveOpenSdk.Runtime.Utilities;
using UnityEngine;
using UnityEngine.UI;
public class PlaceDyNetManager : MonoBehaviour
{

    // 项目 id
    public string appId;
    public Text LogText;

    private readonly LogConsole _logConsole = new LogConsole();
    private LogWriter Log { get; } = new LogWriter(SdkUnityLogger.LogSink, "SampleGameStartup");
    public static PlaceDyNetManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // else
        // {
        //     if (Instance != this)
        //     {
        //         Destroy(gameObject);
        //     }
        // }
        
        // 把 SDK 的日志输出放到场景上
        _logConsole.Text = LogText;
        SdkUnityLogger.OnRichLog += _logConsole.WriteLog;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (Instance!=this)return;
        // 设置参数
        PlaceDySdkManager.AppId = appId;
        // 初始化 SDK

        PlaceDySdkManager.Initialize();

        if (string.IsNullOrEmpty(PlaceDySdkManager.AppId))
        {
            Log.Warning("警告：未设置 AppId，SDK 功能不可用");
        }

        if (string.IsNullOrEmpty(PlaceDySdkManager.Token))
        {
            Log.Warning("警告：SDK 未能从命令行获得 token，请从直播伴侣启动 exe 或手动提供 token");
        }

        StartDirectPushMode();
    }

    // void OnDestroy()
    // {
    //     
    //     // 释放资源
    //     
    // }

    async void StartDirectPushMode()
    {
        if (string.IsNullOrEmpty(PlaceDySdkManager.Token))
        {
            Log.Warning("警告：SDK 未能从命令行获得 token，指令直推不可用");
        }


        Log.Info("开始：指令直推模式");
        // 指令直推
        // 初始化指令直推链路。
        await PlaceDyMessagePushManager.Init();

        // 开启想要接收的消息类型的推送任务，表示对局开始。
        var msgTypes = new[] { PushMessageTypes.LiveComment, PushMessageTypes.LiveGift, PushMessageTypes.LiveLike };
        await Task.WhenAll(msgTypes.Select(PlaceDyMessagePushManager.StartPush));

        // 若收到消息，会打印日志。
        Log.Info("结束：指令直推模式");
    }

    internal class LogConsole
    {
        private const int MaxCount = 15;
        private readonly Queue<string> _messageQueue = new Queue<string>();

        public Text Text { get; set; }

        public Severity MinSeverity = Severity.Info;

        public void WriteLog(Severity severity, string richText)
        {
            if (!severity.IsAtLeast(MinSeverity)) return;

            while (_messageQueue.Count >= MaxCount)
            {
                _messageQueue.Dequeue();
            }

            _messageQueue.Enqueue(richText);

            if (Text == null) return;

            Text.text = string.Join("\n", _messageQueue);
        }
    }
}
