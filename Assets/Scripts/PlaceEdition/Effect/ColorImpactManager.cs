using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorImpactManager : MonoBehaviour
{
    public Color32 aimColor;

    public Transform impact;
    public Transform impactGlow;
    public Transform impactGlowLight;
    public Transform impactFlare;
    public Transform impactRing;
    public Transform impactRing2;
    public Transform impactSparks;
    public Transform impactFlakes;
    public Transform fadeLight;
    // Start is called before the first frame update
    void Start()
    {
        if (impact == null) impact = transform.Find("Impact");
        if (impactGlow == null) impactGlow = transform.GetChild(0).Find("ImpactGlow");
        if (impactGlowLight == null) impactGlowLight = transform.GetChild(0).Find("ImpactGlowLight");
        if (impactFlare == null) impactFlare = transform.GetChild(0).Find("ImpactFlare");
        if (impactRing == null) impactRing = transform.GetChild(0).Find("ImpactRing");
        if (impactRing2 == null) impactRing2 = transform.GetChild(0).Find("ImpactRing2");
        if (impactSparks == null) impactSparks = transform.GetChild(0).Find("ImpactSparks");
        if (impactFlakes == null) impactFlakes = transform.GetChild(0).Find("ImpactFlakes");
        if (fadeLight == null) fadeLight = transform.GetChild(0).Find("FadeLight");
    }

    public void SetImpactColor(Color32 color)
    {
        aimColor = color;
        SetImpactGlowColor(aimColor);
        SetImpactGlowLightColor(aimColor);
        SetImpactFlareColor(aimColor);
        SetImpactRingColor(aimColor);
        SetImpactRing2Color(aimColor);
        SetImpactSparksColor(aimColor);
        SetImpactFlakesColor(aimColor);
        // SetFadeLightColor(aimColor);
    }

    private void SetImpactFlakesColor(Color32 color)
    {
        ParticleSystem ps = impactFlakes.GetComponent<ParticleSystem>();
        var mainModule = ps.main;
        ParticleSystem.MinMaxGradient psColor = mainModule.startColor;
        psColor.color = color;
        mainModule.startColor = psColor;
        Material mat = impactFlakes.GetComponent<Renderer>().material;
        mat.SetColor("_Color", color);
    }

    private void SetImpactSparksColor(Color32 color)
    {
        ParticleSystem ps = impactSparks.GetComponent<ParticleSystem>();
        var mainModule = ps.main;
        ParticleSystem.MinMaxGradient psColor = mainModule.startColor;
        psColor.color = color;
        mainModule.startColor = psColor;

        Material mat = impactSparks.GetComponent<Renderer>().material;
        mat.SetColor("_Color", color);
    }

    private void SetImpactRing2Color(Color32 color)
    {
        Material mat = impactRing2.GetComponent<Renderer>().material;
        mat.SetColor("_Color", color);
    }

    private void SetImpactRingColor(Color32 color)
    {
        Material mat = impactRing.GetComponent<Renderer>().material;
        mat.SetColor("_Color", color);
    }

    public void SetImpactGlowColor(Color32 color)
    {
        Material mat = impactGlow.GetComponent<Renderer>().material;
        mat.SetColor("_Color", color);
    }
    // public void SetImpactGlowColor(Color32 color)
    // {
    //     ParticleSystem ps = impactGlow.GetComponent<ParticleSystem>();
    //     var colorOverLifetimeModule = ps.colorOverLifetime;
    //     Gradient gradient = new Gradient();


    //     float hue, saturation, value;
    //     Color.RGBToHSV(color, out hue, out saturation, out value);

    //     // 稍微调整色调值，例如增加或减少5度
    //     hue = (hue + 0.05f) % 1f; // 因为HSV中的Hue是0-1的范围

    //     // 假设有一个方法将HSV转换回RGB
    //     Color analogousColor = Color.HSVToRGB(hue, saturation, value);

    //     GradientColorKey[] colorKeys = new GradientColorKey[]
    //     {
    //         new GradientColorKey(Color.white, 0.0f), // 0% 时为黑色
    //         new GradientColorKey(color, 0.185f), // 25% 时仍为黑色，开始变浅
    //         new GradientColorKey(analogousColor, 0.632f),   // 50% 时为灰色
    //         new GradientColorKey(Color.white, 1.0f)   // 100% 时为白色
    //     };
    //     GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]
    //     {
    //         new GradientAlphaKey(0.0f, 0.0f), // 0% 时完全不透明
    //         new GradientAlphaKey(1.0f, 0.132f), // 25% 时开始透明度渐变
    //         new GradientAlphaKey(1.0f, 0.5f),  // 50% 时透明度为50%
    //         new GradientAlphaKey(0.0f, 1.0f)   // 100% 时完全透明
    //     };
    //     gradient.SetKeys(colorKeys, alphaKeys);
    //     colorOverLifetimeModule.color = gradient;
    // }

    public void SetImpactGlowLightColor(Color32 color)
    {
        ParticleSystem ps = impactGlowLight.GetComponent<ParticleSystem>();
        var mainModule = ps.main;
        ParticleSystem.MinMaxGradient psColor = mainModule.startColor;
        psColor.color = color;
        mainModule.startColor = psColor;

        Material mat = impactGlowLight.GetComponent<Renderer>().material;
        mat.SetColor("_Color", color);
    }

    public void SetImpactFlareColor(Color32 color)
    {
        Material mat = impactFlare.GetComponent<Renderer>().material;
        mat.SetColor("_Color", color);
    }

    // public void SetImpactFlareColor(Color32 color)
    // {
    //     ParticleSystem ps = impactGlow.GetComponent<ParticleSystem>();
    //     var colorOverLifetimeModule = ps.colorOverLifetime;
    //     Gradient gradient = new Gradient();

    //     // main
    //     var mainModule = ps.main;
    //     ParticleSystem.MinMaxGradient psColor = mainModule.startColor;
    //     psColor.color = color;
    //     mainModule.startColor = psColor;


    //     float hue, saturation, value;
    //     Color.RGBToHSV(color, out hue, out saturation, out value);

    //     // 稍微调整色调值，例如增加或减少5度
    //     // hue = (hue + 0.05f) % 1f; // 因为HSV中的Hue是0-1的范围

    //     float lowerSaturation = saturation * 0.8f; // 饱和度降低20%
    //     float higherValue = value * 1.2f; // 亮度提高20%

    //     Color[] analogousColors = new Color[3];
    //     for (int i = 0; i < 3; i++)
    //     {
    //         float hueOffset = (i == 1) ? 0 : 0.05f * (i == 0 ? -1 : 1);
    //         float newHue = (hue + hueOffset) % 1; // 确保色相在0-1之间

    //         Color newColorHSV = Color.HSVToRGB(newHue, lowerSaturation, higherValue);
    //         analogousColors[i] = newColorHSV;
    //     }

    //     // 假设有一个方法将HSV转换回RGB
    //     // Color analogousColor = Color.HSVToRGB(hue, saturation, value);

    //     GradientColorKey[] colorKeys = new GradientColorKey[]
    //     {
    //         new GradientColorKey(aimColor, 0.0f), // 0% 
    //         new GradientColorKey(analogousColors[1], 0.25f), //     25% 
    //         new GradientColorKey(analogousColors[2], 0.50f),   // 50% 
    //         new GradientColorKey(Color.white, 1.0f)   // 100% 时为白色
    //     };
    //     GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]
    //     {
    //         new GradientAlphaKey(0.0f, 0.0f), // 0% 时完全不透明
    //         new GradientAlphaKey(0f, 0.8f), // 25% 时开始透明度渐变
    //         new GradientAlphaKey(1.0f, 1f),  // 100% 时透明度为100%
    //     };
    //     gradient.SetKeys(colorKeys, alphaKeys);
    //     colorOverLifetimeModule.color = gradient;
    // }

    // public void SetFadeLightColor(Color32 color)
    // {
    //     Light light = fadeLight.GetComponent<Light>();
    //     light.color = color;
    // }
}
