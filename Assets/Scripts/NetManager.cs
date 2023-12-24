using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NetManager : MonoBehaviour
{

    // private Socket clientSocket;
    private TcpClient clientSocket;
    private NetworkStream networkStream;
    private bool isListening;
    private const string ip = "127.0.0.1"; // Python服务器的IP
    private const int port = 7878; // Python服务器的端口

    public DanmakuManager danmakuManager;

    void Start()
    {
        // 在 Start 方法中调用异步操作
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ConnectToServer();
        }

        if (Input.GetKeyDown(KeyCode.K) && clientSocket != null)
        {
            _ = CloseConnectionAsync();
        }

        if (Input.GetKeyDown(KeyCode.J) && clientSocket != null)
        {
            _ = RPC("Hello from Unity Send Message!");
            
        }
    }

    private void ConnectToServer()
    {
        if (clientSocket == null || !clientSocket.Connected)
        {
            RunClientAsync();
        }
    }


    private async Task CloseConnectionAsync()
    {
        if (clientSocket != null)
        {
            string closeMessage = "/close";
            await SendAsync(closeMessage);
            isListening = false;  // 停止监听消息
            networkStream.Close();
            clientSocket.Close();
            clientSocket = null;

            Debug.Log("Connection closed");
        }
    }


    private async void RunClientAsync()
    {
        clientSocket = new TcpClient();
        try
        {

            await clientSocket.ConnectAsync(ip, port);
            if (clientSocket.Connected)
            {
                Debug.Log("Connected to server");
                networkStream = clientSocket.GetStream();
                isListening = true;
                StartListeningForMessages();
            }
            else
            {
                Debug.Log("Failed to connect to server");
            }

            // 发送消息
            await SendAsync("/rpc Hello from Unity!");

            // 接收消息
            // var receivedMessage = await ReceiveAsync();
            // Debug.Log("Message received: " + receivedMessage);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    private async void StartListeningForMessages()
    {
        try
        {
            byte[] buffer = new byte[1024];

            while (isListening && clientSocket.Connected)
            {
                int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    // Debug.Log("Listening received: " + message);
                    HandleCommand(message.Trim());
                }
            }
        }
        catch (Exception e)
        {
            if (isListening)
            {
                Debug.LogError("Error in receiving data: " + e.Message);
            }
        }
    }

    private void HandleCommand(string command)
    {
        string[] parts = command.Split(' ');

        switch (parts[0])
        {
            case "/bili":
                HandleBiliResponse(parts);
                break;
            case "/rpc":
                HandleRPCResponse(parts);
                break;
            default:
                Debug.Log("Unknown command received: " + command);
                break;
        }
    }

    private void HandleBiliResponse(string[] parts)
    {
        String buffer = String.Join(" ", parts);
        string[] items = buffer.Trim().Split("/bili");
        // 处理 command1
        for (int i = 0; i < items.Length; i++)
        {
            string[] item = items[i].Trim().Split("|");
            switch (item[0])
            {
                case "<danmu>":
                    string[] c = item[1].Trim().Split(":");
                    if (c.Length > 1 && c[1].StartsWith("/"))
                        {
                            print("获得弹幕指令");
                            string[] room_and_user = c[0].Trim().Split(" ");
                            ChatCommandManager.Instance.RunChatCommand(room_and_user[1],c[1]);
                        }
                    break;
                case "<gift>":
                    break;
                default:
                    break;
            }
            Debug.Log(item[1]);
            danmakuManager.AddNewDanmaku(item[1]);
        }
    }

    private void HandleRPCResponse(string[] parts)
    {
        // 处理 command2
        Debug.Log(String.Join(" ", parts));
    }


    public async Task RPC(string message)
    {
        // string result = await SendMessageToServerAsync(message);
        // print("RPC : " + result);
        await SendMessageToServerAsync(message);
    }

    public async Task SendMessageToServerAsync(string message)
    {
        if (clientSocket != null && clientSocket.Connected)
        {
            try
            {
                await SendAsync(message); // 发送消息
                // return await ReceiveAsync(); // 等待并接收响应
            }
            catch (Exception e)
            {
                Debug.LogError("Error in sending/receiving data: " + e.Message);
                // return null;
            }
        }
        else
        {
            Debug.LogError("No connection established.");
            // return null;
        }
    }

    private async Task SendAsync(string message)
    {
        if (networkStream != null && networkStream.CanWrite)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await networkStream.WriteAsync(buffer, 0, buffer.Length);
        }
    }

    private async Task<string> ReceiveAsync()
    {
        byte[] buffer = new byte[1024];
        int received = await networkStream.ReadAsync(buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer, 0, received);
    }



    void OnApplicationQuit()
    {
        _ = CloseConnectionAsync();
    }
}

