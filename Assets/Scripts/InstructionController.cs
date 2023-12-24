using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructionController : MonoBehaviour
{
    // ChatCommandManager chatCommandManager;
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
        ChatCommandManager.Instance.RunChatCommand("cx",inputText); // Modify this line
    }
}
