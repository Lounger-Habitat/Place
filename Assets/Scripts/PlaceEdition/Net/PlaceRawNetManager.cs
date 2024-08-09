// using System;
// using System.Net.Sockets;
// using System.Text;
// using System.Threading.Tasks;
// using UnityEngine;

// public class PlaceNetManager : MonoBehaviour
// {

//     public static PlaceNetManager Instance { get; set; }

//     void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             // DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     // private Socket clientSocket;
//     private TcpClient clientSocket;
//     private NetworkStream networkStream;
//     private bool isListening;
//     private const string ip = "127.0.0.1"; // Python服务器的IP
//     private const int port = 7878; // Python服务器的端口

//     // UI 展示
//     public DanmakuManager danmakuManager;

//     void Update()
//     {
//         // 开启网络连接
//         if (Input.GetKeyDown(KeyCode.U))
//         {
//             ConnectToServer();
//         }
//         // 关闭网络连接
//         if (Input.GetKeyDown(KeyCode.I) && clientSocket != null)
//         {
//             _ = CloseConnectionAsync();
//         }
//         // 发送测试消息
//         if (Input.GetKeyDown(KeyCode.O) && clientSocket != null)
//         {
//             _ = RPC("Hello from Unity Send Message!");
            
//         }
//     }

//     private void ConnectToServer()
//     {
//         if (clientSocket == null || !clientSocket.Connected)
//         {
//             RunClientAsync();
//         }
//     }


//     private async Task CloseConnectionAsync()
//     {
//         if (clientSocket != null)
//         {
//             string closeMessage = "/close";
//             await SendAsync(closeMessage);
//             isListening = false;  // 停止监听消息
//             networkStream.Close();
//             clientSocket.Close();
//             clientSocket = null;

//             Debug.Log("Connection closed");
//         }
//     }


//     private async void RunClientAsync()
//     {
//         clientSocket = new TcpClient();
//         try
//         {

//             await clientSocket.ConnectAsync(ip, port);
//             if (clientSocket.Connected)
//             {
//                 Debug.Log("Connected to server");
//                 networkStream = clientSocket.GetStream();
//                 isListening = true;
//                 StartListeningForMessages();
//             }
//             else
//             {
//                 Debug.Log("Failed to connect to server");
//             }

//             // 发送消息
//             await SendAsync("/rpc Hello from Unity!");

//             // 接收消息
//             // var receivedMessage = await ReceiveAsync();
//             // Debug.Log("Message received: " + receivedMessage);
//         }
//         catch (Exception e)
//         {
//             Debug.LogError(e.ToString());
//         }
//     }

//     private async void StartListeningForMessages()
//     {
//         try
//         {
//             byte[] buffer = new byte[1024];

//             while (isListening && clientSocket.Connected)
//             {
//                 int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
//                 if (bytesRead > 0)
//                 {
//                     string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
//                     Debug.Log("Listening received: " + message);
//                     HandleCommand(message.Trim());
//                 }
//             }
//         }
//         catch (Exception e)
//         {
//             if (isListening)
//             {
//                 Debug.LogError("Error in receiving data: " + e.Message);
//             }
//         }
//     }

//     private void HandleCommand(string command)
//     {
//         // 处理消息 类型
//         string[] parts = command.Split(' ');
//         switch (parts[0])
//         {
//             case "/bili":
//                 HandleBiliResponse(parts);
//                 break;
//             case "/rpc":
//                 HandleRPCResponse(parts);
//                 break;
//             default:
//                 Debug.Log("Unknown command received: " + command);
//                 break;
//         }
//     }

//     private void HandleBiliResponse(string[] parts)
//     {
//         /*
//             1、检查消息类别
//                 弹幕消息
//                     聊天消息 /bili <danmu> | {s} 
//                     命令消息
//                         检查用户是否有权限，比如是否加入了队伍，是否是队长
//                         每个人都可以加入队伍的人，可以发送指令
//                 礼物消息    /bili <gift> | {s} 
//                 心跳消息

//         */
//         String buffer = String.Join(" ", parts);
//         string[] items = buffer.Trim().Split("/bili");
//         for (int i = 1; i < items.Length; i++)
//         {
//             string[] item = items[i].Trim().Split("|");
//             Debug.Log("type" + item[0]);
//             switch (item[0].Trim())
//             {
//                 case "<danmu>":
//                     // 这是干嘛呢？
//                     string[] c = item[1].Trim().Split(":");
//                     // 指令 / 传统指令
//                     if (c.Length > 1 && c[1].StartsWith("/"))
//                     {
//                         string[] room_and_user = c[0].Trim().Split(" ");
//                         PlaceInstructionManager.Instance.DefaultRunChatCommand(room_and_user[1],c[1]);
//                     }
//                     // 正则 指令，先用 111 222 333 444 代替
//                     if (c.Length > 1 && c[1].StartsWith("111"))
//                     {
//                         string[] room_and_user = c[0].Trim().Split(" ");
//                         PlaceInstructionManager.Instance.DefaultRunChatCommand(room_and_user[1],c[1]);
//                     }
//                     if (c.Length > 1 && c[1].StartsWith("222"))
//                     {
//                         string[] room_and_user = c[0].Trim().Split(" ");
//                         PlaceInstructionManager.Instance.DefaultRunChatCommand(room_and_user[1],c[1]);
//                     }
//                     if (c.Length > 1 && c[1].StartsWith("333"))
//                     {
//                         string[] room_and_user = c[0].Trim().Split(" ");
//                         PlaceInstructionManager.Instance.DefaultRunChatCommand(room_and_user[1],c[1]);
//                     }
//                     if (c.Length > 1 && c[1].StartsWith("444"))
//                     {
//                         string[] room_and_user = c[0].Trim().Split(" ");
//                         PlaceInstructionManager.Instance.DefaultRunChatCommand(room_and_user[1],c[1]);
//                     }
//                     // 普通弹幕
//                     break;
//                 case "<gift>":
//                     string[] c_gift = item[1].Trim().Split(":");
//                     if (c_gift.Length > 1)
//                     {
//                         string[] room_and_user = c_gift[0].Trim().Split(" ");
//                         PlaceInstructionManager.Instance.DefaultRunChatCommand(room_and_user[1],c_gift[1]);
//                     }
//                     break;
//                 case "<heart>":
//                     break;
//                 default:
//                     break;
//             }
//             // 界面显示
//             // danmakuManager.AddNewDanmaku(item[1]);
//         }
//     }

//     private void HandleRPCResponse(string[] parts)
//     {
//         // 处理 command2
//         Debug.Log(String.Join(" ", parts));
//     }


//     public async Task RPC(string message)
//     {
//         await SendMessageToServerAsync(message);
//     }

//     public async Task SendMessageToServerAsync(string message)
//     {
//         if (clientSocket != null && clientSocket.Connected)
//         {
//             try
//             {
//                 await SendAsync(message); // 发送消息
//                 // return await ReceiveAsync(); // 等待并接收响应
//             }
//             catch (Exception e)
//             {
//                 Debug.LogError("Error in sending/receiving data: " + e.Message);
//                 // return null;
//             }
//         }
//         else
//         {
//             Debug.LogError("No connection established.");
//             // return null;
//         }
//     }

//     private async Task SendAsync(string message)
//     {
//         if (networkStream != null && networkStream.CanWrite)
//         {
//             byte[] buffer = Encoding.UTF8.GetBytes(message);
//             await networkStream.WriteAsync(buffer, 0, buffer.Length);
//         }
//     }

//     private async Task<string> ReceiveAsync()
//     {
//         byte[] buffer = new byte[1024];
//         int received = await networkStream.ReadAsync(buffer, 0, buffer.Length);
//         return Encoding.UTF8.GetString(buffer, 0, received);
//     }



//     void OnApplicationQuit()
//     {
//         _ = CloseConnectionAsync();
//     }
// }

