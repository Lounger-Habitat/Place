using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaceBoardFrameManager : MonoBehaviour
{
    public TMP_Text JG1;
    public TMP_Text JG3;
    public TMP_Text JG7;
    public TMP_Text JG9;
    // Start is called before the first frame update
    void Start()
    {
        int width = PlaceBoardManager.Instance.width;
        int height = PlaceBoardManager.Instance.height;
        JG1.text = $"(0,0)";
        JG3.text = $"({width - 1},0)";
        JG7.text = $"(0,{height - 1})";
        JG9.text = $"({width - 1},{height - 1})";
    }
}
