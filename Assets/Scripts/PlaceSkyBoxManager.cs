using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceSkyBoxManager : MonoBehaviour
{
    public float  skySpeed = 0f;
    // Update is called once per frame

    void Start() {
        // 设置初始化的旋转角度
        RenderSettings.skybox.SetFloat("_Rotation", 0);
    }

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation",Time.time * skySpeed);
    }

    // 结束
    void OnDestroy()
    {
        // 设置初始化的旋转角度
        RenderSettings.skybox.SetFloat("_Rotation", 0);
    }
}
