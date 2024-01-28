using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public string ins;
    public string playerName;
    // Start is called before the first frame update

    public Vector3 position;
    public Vector3 rotation;

    // Update is called once per frame
    void Update()
    {
        // 按下9，执行指令  接受 指令
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            PlaceInstructionManager.DefaultRunChatCommand(playerName,ins);
            // ChatCommandManager.Instance.RunChatCommand("test",ins);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            CameraView();
        }
        
    }

    public void CameraView() {
        Camera.main.transform.position = position;
        Camera.main.transform.rotation = Quaternion.Euler(rotation);
    }
}
