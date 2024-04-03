using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceSkyBoxManager : MonoBehaviour
{
    public float  skySpeed = 0f;
    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation",Time.time * skySpeed);
    }
}
