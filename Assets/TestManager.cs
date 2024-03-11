using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public string ins;
    public string playerName;
    // Start is called before the first frame update

    public string gift="20";

    public Vector3 position;
    public Vector3 rotation;

    // Update is called once per frame
    void Update()
    {
        // 按下/，执行指令  接受 指令
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            PlaceInstructionManager.DefaultRunChatCommand(playerName,ins);
            // ChatCommandManager.Instance.RunChatCommand("test",ins);
        }
        // 按下,，执行指令  接受 指令
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            PlaceInstructionManager.DefaultGiftCommand(playerName,gift);
        }
        // 按下.，执行指令  接受 指令
        if (Input.GetKeyDown(KeyCode.Period))
        {
            CameraView();
        }
        
    }

    public void CameraView() {
        Camera.main.transform.position = position;
        Camera.main.transform.rotation = Quaternion.Euler(rotation);
    }

    public void RandomAddTest(){
        // 添加player

        //执行指令
    }
}
