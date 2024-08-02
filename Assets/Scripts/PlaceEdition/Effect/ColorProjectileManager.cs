using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorProjectileManager : MonoBehaviour
{

    public Color aimColor;

    public Transform head;
    public Transform snowFlakes;
    public Transform iceFlares;
    public Transform tail;
    public Transform tailBack;
    public Transform fadeLight;
    // Start is called before the first frame update
    void Start()
    {
        if (head == null) head = transform.Find("Head");
        if (snowFlakes == null) snowFlakes = transform.Find("SnowFlakes");
        if (iceFlares == null) iceFlares = transform.Find("IceFlares");
        if (tail == null) tail = transform.Find("Tail");
        if (tailBack == null) tailBack = transform.Find("TailBack");
        if (fadeLight == null) fadeLight = transform.Find("FadeLight");

    }

    public void SetProjectileColor(Color32 color) {
        aimColor = color;
        SetHeadColor(aimColor);
        SetSnowFlakesColor(aimColor);
        SetIceFlaresColor(aimColor);
        SetTailColor(aimColor);
        // SetTailBackColor(aimColor);
        SetFadeLightColor(aimColor);
    }

    public void SetHeadColor(Color32 color)
    {
        Material mat = head.GetComponent<MeshRenderer>().material;
        mat.SetColor("_ShapeColor", color);
        mat.SetColor("_GlowColor", color);
    }

    public void SetSnowFlakesColor(Color32 color)
    {
        ParticleSystem ps = snowFlakes.GetComponent<ParticleSystem>();
        var mainModule = ps.main;
        ParticleSystem.MinMaxGradient psColor = mainModule.startColor;
        psColor.color = color;
        mainModule.startColor = psColor;
    }

    public void SetIceFlaresColor(Color32 color)
    {
        ParticleSystem ps = iceFlares.GetComponent<ParticleSystem>();
        var mainModule = ps.main;
        ParticleSystem.MinMaxGradient psColor = mainModule.startColor;
        psColor.color = color;
        mainModule.startColor = psColor;
    }

    public void SetTailColor(Color32 color)
    {
        TrailRenderer tr = tail.GetComponent<TrailRenderer>();
        tr.startColor = color;
        tr.endColor = Color.clear;
        Material mat = tr.material;
        mat.SetColor("_GlowColor", color);
    }

    public void SetTailBackColor(Color32 color)
    {
        TrailRenderer tr = tailBack.GetComponent<TrailRenderer>();
        tr.startColor = color;
        tr.endColor = Color.clear;
        Material mat = tr.material;
        mat.SetColor("_Color", color);
        mat.SetColor("_GlowColor", color);
    }

    public void SetFadeLightColor(Color32 color)
    {
        Light light = fadeLight.GetComponent<Light>();
        light.color = color;
    }


}
