using UnityEngine;

public class PlaceDyDebugLog : MonoBehaviour
{
    public RectTransform logPanel;

    public bool isOn = true;

    public void OnClick()
    {
        isOn = !isOn;
        switch (isOn)
        {
            case true:
                LogOff();
                break;
            case false:
                LogOn();
                break;
        }
    }

    void LogOn()
    {
        // 控制移动 position y
        logPanel.anchoredPosition = new Vector2(logPanel.anchoredPosition.x, 0);
    }

    void LogOff()
    {
        // 控制移动 position y
        logPanel.anchoredPosition = new Vector2(logPanel.anchoredPosition.x, -1085);
    }
}
