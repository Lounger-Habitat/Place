using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightPixels : MonoBehaviour
{
    public int textureWidth = 512;
    public int textureHeight = 512;

    public int x = 100;
    public int y = 100;
    public Material material;

    // 轨迹图   初始化全黑
    Texture2D trajectoryTexture;



    void Start()
    {
        // 创建一个新的 Texture2D
        trajectoryTexture = initTrajectoryTexture();

        // 设置 alpha 通道
        SetAlphaChannel(trajectoryTexture, 0f); // 0.5f 是示例中的 alpha 值

        // 将纹理应用到材质
        material.SetTexture("_TrajectoryTex", trajectoryTexture);
    }

    Texture2D initTrajectoryTexture()
    {
        Texture2D texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[textureWidth * textureHeight];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.black;
        }
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    void Update()
    {
        // 按下 k 键
        if (Input.GetKeyDown(KeyCode.K))
        {
            // 设置所有像素的 alpha 通道值
            SetPixelAlpha(trajectoryTexture,x,y ,1f);
            Debug.Log($"set pixel{x},{y} alpha");
            material.SetTexture("_PixelStateTex", trajectoryTexture);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            // 设置所有像素的 alpha 通道值
            SetAlphaChannel(trajectoryTexture, 0f);
            Debug.Log($"set pixel{x},{y} alpha");
            material.SetTexture("_PixelStateTex", trajectoryTexture);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            // 设置所有像素的 alpha 通道值
            SetAlphaChannel(trajectoryTexture, 1f);
            Debug.Log("set all pixel alpha");
            material.SetTexture("_PixelStateTex", trajectoryTexture);
        }
        // 设置指定位置的像素 alpha 通道值
        // SetPixelAlpha(pixelStateTexture, 100, 100, 0.5f);
    }

    void SetAlphaChannel(Texture2D texture, float alphaValue)
    {
        Color[] pixels = texture.GetPixels();

        // 设置每个像素的 alpha 通道值
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i].a = alphaValue;
        }

        // 将修改后的像素数组应用到纹理
        texture.SetPixels(pixels);

        // 应用修改并更新 GPU 上的纹理数据
        texture.Apply();
    }

    void SetPixelAlpha(Texture2D texture, int x, int y, float alphaValue)
    {
        // 获取指定位置的像素颜色
        Color pixelColor = texture.GetPixel(x, y);

        // 设置 alpha 通道值
        pixelColor.a = alphaValue;

        // 将修改后的像素颜色应用到纹理
        texture.SetPixel(x, y, pixelColor);

        // 应用修改并更新 GPU 上的纹理数据
        texture.Apply();
    }
}
