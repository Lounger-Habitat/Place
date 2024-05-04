using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlaceTeamSlider : MonoBehaviour
{
    public Slider slider; // 拖拽滑动条的GameObject到这个字段
    public TMP_Text valueText; // 用于显示当前滑动条值的Text UI组件

    void Start()
    {
        // 初始设置文本
        slider.value = 0;
    }

    public void UpdateSlider(int input)
    {
        if (slider)
        {
            // 确保输入的值在滑动条的范围内
            int value = (int)Mathf.Clamp(input, slider.minValue, slider.maxValue);
            slider.value = value;
        }
    }
}
