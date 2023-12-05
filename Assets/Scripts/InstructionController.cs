using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructionController : MonoBehaviour
{
    public PixelsContainer PixelContainer;
    public TMP_InputField instruction;
    public Button sendButton;
    // Start is called before the first frame update
    void Start()
    {
        sendButton.onClick.AddListener(OnButtonClick);
    }
    private void OnButtonClick()
    {
        // 获取InputField中的文本内容
        string inputText = instruction.text;

        // 调用你的其他函数
        RunInstruction(inputText);
    }

    // 你想要执行的函数
    private void RunInstruction(string inputText)
    {
        int x,y,r,g,b;
        string command;
        // 检查是否有足够的组件
        string[] components = inputText.Trim().Split(' ');
        if (components.Length >= 5)
        {
            // components[0] 是 "/d"
            command = components[0];
            x = int.Parse(components[1]);
            y = int.Parse(components[2]);
            r = int.Parse(components[3]);
            g = int.Parse(components[4]);
            b = int.Parse(components[5]);

            // 在这里可以使用解析得到的值进行其他操作
            Debug.Log("解析结果： x=" + x + ", y=" + y + ", r=" + r + ", g=" + g + ", b=" + b);
            PixelContainer.DrawCommand(command,x,y,r,g,b);

        }
        else
        {
            Debug.LogError("输入字符串格式不正确");
        }
        
    }
}
