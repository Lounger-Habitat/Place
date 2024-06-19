using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ByteDance.Live.Foundation.Logging;
using ByteDance.LiveOpenSdk.Push;
using ByteDance.LiveOpenSdk.Runtime.Utilities;
using UnityEngine;
using UnityEngine.UI;

using ByteDance.LiveOpenSdk.Room;
using ByteDance.LiveOpenSdk.Runtime;
using ByteDance.LiveOpenSdk.Utilities;

namespace Douyin.LiveOpenSDK.Samples
{
    /// <summary>
    /// <para>
    /// 负责控制示例场景 SampleGameScene 的行为。
    /// </para>
    /// <para>
    /// 使用方法：点击“指令直推模式”或“抖音云模式”二者之一，然后观察日志窗口。
    /// 两种模式互斥，无法同时生效。
    /// </para>
    /// <para>
    /// 需要配置参数的类：
    /// <list type="bullet">
    /// <item><see cref="SampleLiveOpenSdkManager"/></item>
    /// <item><see cref="SampleDyCloudManager"/></item>
    /// </list>
    /// </para>
    /// </summary>
    public class SampleGameStartup : MonoBehaviour
    {
        public Button Button_Mode1;
        public Button Button_Mode2;
        public Button Room_Button;
        public Button Room_Button_Update;
        public Text LogText;

        private readonly LogConsole _logConsole = new LogConsole();
        private LogWriter Log { get; } = new LogWriter(SdkUnityLogger.LogSink, "SampleGameStartup");

        // private static IRoomInfoService RoomInfoService { get; } = LiveOpenSdk.Instance.GetRoomInfoService();

        private void Awake()
        {
            // 把 SDK 的日志输出放到场景上
            _logConsole.Text = LogText;
            SdkUnityLogger.OnRichLog += _logConsole.WriteLog;
        }

        private void Start()
        {
            InitEvents();

            // 配置相关参数，请修改为实际的值
            // SampleLiveOpenSdkManager.AppId = "";
            SampleLiveOpenSdkManager.AppId = "tt767800c5e948f69a10";
            SampleLiveOpenSdkManager.Token = "B+/UhqBCwy6DnApGcQGt0xBEW5cmoodLcZLDAk3P7VbjJd7a6vRenewM0sTJjoKvDPqq00L64Y2yuBClHllTH/Sl1GWI9VZFY1kfSqjomCigzPhMByngGJVlTWM=";

            // SampleDyCloudManager.EnvId = "";
            // SampleDyCloudManager.ServiceId = "";

            // 初始化直播开放 SDK
            SampleLiveOpenSdkManager.Initialize();

            if (string.IsNullOrEmpty(SampleLiveOpenSdkManager.AppId))
            {
                Log.Warning("警告：未设置 AppId，SDK 功能不可用");
            }

            if (string.IsNullOrEmpty(SampleLiveOpenSdkManager.Token))
            {
                Log.Warning("警告：SDK 未能从命令行获得 token，请从直播伴侣启动 exe 或手动提供 token");
            }
        }

        private void OnDestroy()
        {
            // 销毁直播开放 SDK
            SampleLiveOpenSdkManager.Uninitialize();
        }

        private void InitEvents()
        {
            Button_Mode1.onClick.AddListener(StartDirectPushMode);
            Button_Mode2.onClick.AddListener(StartDyCloudMode);
            Room_Button.onClick.AddListener(RoomTest);
            Room_Button_Update.onClick.AddListener(RoomUpdateTest);
        }

        // Room Test
        private async void RoomTest()
        {
            Log.Info("开始：Room Test");

            // 创建房间
            // await RoomInfoService.WaitForRoomInfoAsync();
        }
        private async void RoomUpdateTest()
        {
            Log.Info("开始：Room Update Test");

            // 创建房间
            // await RoomInfoService.UpdateRoomInfoAsync();
        }

        // 指令直推模式
        private async void StartDirectPushMode()
        {
            if (string.IsNullOrEmpty(SampleLiveOpenSdkManager.Token))
            {
                Log.Warning("警告：SDK 未能从命令行获得 token，指令直推不可用");
            }

            Log.Info("开始：指令直推模式");

            // 初始化指令直推链路。
            await SampleMessagePushManager.Init();

            // 开启想要接收的消息类型的推送任务，表示对局开始。
            var msgTypes = new[] { PushMessageTypes.LiveComment, PushMessageTypes.LiveGift, PushMessageTypes.LiveLike };
            await Task.WhenAll(msgTypes.Select(SampleMessagePushManager.StartPush));

            // 若收到消息，会打印日志。
            Log.Info("结束：指令直推模式");
        }

        // 抖音云模式
        private async void StartDyCloudMode()
        {
            Log.Info("开始：抖音云模式");

            // 初始化抖音云。
            await SampleDyCloudManager.Init();

            // 短连接能力演示。
            await SampleDyCloudManager.StartTasks();

            // 长连接能力演示。若收到消息，会打印日志。
            await SampleDyCloudManager.ConnectWebSocket();

            Log.Info("结束：抖音云模式");
        }
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