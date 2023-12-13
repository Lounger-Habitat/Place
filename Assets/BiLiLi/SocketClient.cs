using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SocketClient : MonoBehaviour
{
    private BaseSocket socket;

    private void Start()
    {
        socket = new UnitySocketClient((msg) =>
        {
            Debug.Log($"接到消息：{msg}");
        });
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.G))
        {
            socket.Send("hello");
        }
    }

    private void OnDestroy(){
        socket.Close();
    }
}
