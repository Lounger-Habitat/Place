using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UnitySocketClient : BaseSocket
{
    private Socket clientSocket; // 客户端socket
    private Action<string> msgCallback; // 消息回调
    private byte[] readBuff; // 收到消息的缓存

    public UnitySocketClient(Action<string> callback) {
        msgCallback = callback;
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        new Thread(() => {
            clientSocket.Connect("127.0.0.1", 12345); //连接服务器, 未连接上就会一直阻塞
            Listening();
        }).Start();
    }

    public void Listening() {
        readBuff = new byte[1024];
        while(true) {
            Receive();
        }
    }

    public void Receive() {
        Array.Clear(readBuff, 0, readBuff.Length); // 清空缓存
        int count = clientSocket.Receive(readBuff); // 收到消息, 并存放在缓冲区, 没有消息就会一直阻塞
        string msg = Encoding.UTF8.GetString(readBuff, 0, count);
        msgCallback("客服发来消息: " + msg);
    }

    public void Send(string msg) {
        byte[] buffer = Encoding.UTF8.GetBytes(msg);
        clientSocket.Send(buffer);
    }

    ~UnitySocketClient() {
        clientSocket.Close();
    }
}

public interface BaseSocket {
    void Listening(); // 监听连接, 监听消息
    void Receive(); // 接收到消息
    void Send(string msg); // 发送消息
}