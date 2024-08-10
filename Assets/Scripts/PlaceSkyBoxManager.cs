using UnityEngine;

public class PlaceSkyBoxManager : MonoBehaviour
{
    public float  skySpeed = 0f;
    // Update is called once per frame
    private float init_rotation;

    void Start() {
        // 记录初始化的旋转角度
        init_rotation = RenderSettings.skybox.GetFloat("_Rotation");
        // 设置初始化的旋转角度
        RenderSettings.skybox.SetFloat("_Rotation", init_rotation);
    }

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation",Time.time * skySpeed);
    }

    // 结束
    void OnDestroy()
    {
        // 设置初始化的旋转角度
        RenderSettings.skybox.SetFloat("_Rotation", init_rotation);
    }
}
