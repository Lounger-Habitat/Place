using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownPanel : MonoBehaviour
{
    public TMP_Text countdownText; // 用于引用UI中的Text组件  
    private int startTime = 1200; // 20分钟转换为秒是1200秒  
    //private float countdownTime;
    private Coroutine handle;

    public Action TimeOverAction;
    public void StartTimeDown()  
    {  
        handle = StartCoroutine(UpdateCountdown(startTime));  
    }

    public void Init(Action action)
    {
        TimeOverAction += action;
    }
    IEnumerator UpdateCountdown(int duration)  
    {  
        float countdownTime = duration;  
        while (countdownTime >= 0)  
        {  var time=new TimeSpan(0, 0,Convert.ToInt32( countdownTime));
            //int min = (int)countdownTime / 60; 
            //string timeString = min.ToString("d2") + ":" + (min % 60).ToString("d2"); 
            countdownText.text = time.ToString(@"mm\:ss");
            //time.Minutes.ToString()+":"+time.Seconds.ToString("00"); // 显示当前经过的时间（以秒为单位）  
            yield return new WaitForSeconds(1.1f); // 等待1秒  
            countdownTime -= 1f; // 每秒递减1秒  
        }  
        Debug.Log("时间到!");
        TimeOverAction?.Invoke();
    }

    public void Reset()
    {
        TimeOverAction = null;
        StopCoroutine(handle);
    }
}
