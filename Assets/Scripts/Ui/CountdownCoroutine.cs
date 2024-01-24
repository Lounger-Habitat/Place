using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownCoroutine : MonoBehaviour
{
    public TMP_Text countdownText; // 用于引用UI中的Text组件  
    private float startTime = 12000f; // 20分钟转换为秒是12000秒  
    private float countdownTime;

    public Action TimeOverAction;
    public void StartTimeDown()  
    {  
        StartCoroutine(UpdateCountdown(startTime));  
    }

    public void Init(Action action)
    {
        TimeOverAction += action;
    }
    IEnumerator UpdateCountdown(float duration)  
    {  
        countdownTime = duration;  
        while (countdownTime > 0)  
        {  
            countdownText.text = Time.timeSinceLevelLoad.ToString("f2") + "s"; // 显示当前经过的时间（以秒为单位）  
            yield return new WaitForSeconds(1f); // 等待1秒  
            countdownTime -= 1f; // 每秒递减1秒  
        }  
        Debug.Log("时间到!");
        TimeOverAction?.Invoke();
    }

    public void ResetThis()
    {
        TimeOverAction = null;
    }
}
