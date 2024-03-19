using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        // 按下.，执行指令  测试 指令
        if (Input.GetKeyDown(KeyCode.Period))
        {
            RandomAddTest();
        }
        
    }

    public void CameraView() {
        Camera.main.transform.position = position;
        Camera.main.transform.rotation = Quaternion.Euler(rotation);
    }

    public void RandomAddTest(){
        // 添加player
        string[] cx1 = { "cx——1", "111" };
        string[] cx2 = { "cx——2", "111" };
        string[] cx3 = { "cx——3", "111" };
        string[] cx4 = { "cx——4", "111" };

        string[] gt1 = { "gt——1", "222" };
        string[] gt2 = { "gt——2", "222" };
        string[] gt3 = { "gt——3", "222" };
        string[] gt4 = { "gt——4", "222" };

        string[] by1 = { "by——1", "333" };
        string[] by2 = { "by——2", "333" };
        string[] by3 = { "by——3", "333" };
        string[] by4 = { "by——4", "333" };

        string[] hy1 = { "hy——1", "444" };
        string[] hy2 = { "hy——2", "444" };
        string[] hy3 = { "hy——3", "444" };
        string[] hy4 = { "hy——4", "444" };

        var combinedListLinq = new[] { cx1, cx2, cx3, cx4, gt1, gt2, gt3, gt4, by1, by2, by3, by4, hy1, hy2, hy3, hy4 }.SelectMany(a => a).ToList();
        
        StartCoroutine(RepeatFunctionCall(combinedListLinq));
        
        //执行指令
    }

    IEnumerator RepeatFunctionCall(List<string> combinedListLinq)
    {
        for (int i = 0; i < combinedListLinq.Count; i=i+2) // 循环
        {
            PlaceInstructionManager.DefaultRunChatCommand(combinedListLinq[i],combinedListLinq[i+1]); // 调用你的函数
            yield return new WaitForSeconds(1f); // 等待1秒
        }
    }
}
