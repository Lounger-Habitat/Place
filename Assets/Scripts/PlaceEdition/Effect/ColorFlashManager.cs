using UnityEngine;

public class ColorFlashManager : MonoBehaviour
{
    public Color32 aimColor;
    public Transform flash;
    public Transform iceMuzzleCone;
    public Transform snowFlakes;
    public Transform iceFlares;
    public Transform glowSpheres;
    public Transform fadeLight;
    void Start()
    {
        if (flash == null) flash = transform.Find("Muzzle Flash");
        if (iceMuzzleCone == null) iceMuzzleCone = transform.GetChild(0).Find("IceMuzzleCone");
        if (snowFlakes == null) snowFlakes = transform.GetChild(0).Find("SnowFlakes");
        if (iceFlares == null) iceFlares = transform.GetChild(0).Find("IceFlares");
        if (glowSpheres == null) glowSpheres = transform.GetChild(0).Find("GlowSpheres");
        if (fadeLight == null) fadeLight = transform.GetChild(0).Find("FadeLight");

    }

    public void SetFlashColor(Color32 color)
    {
        aimColor = color;
    }

    public void SetIceMuzzleConeColor(Color32 color)
    {
        Material mat = iceMuzzleCone.GetComponent<MeshRenderer>().material;
        mat.SetColor("_Color", color);
    }
}
