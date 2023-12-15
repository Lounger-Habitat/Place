using UnityEngine;

public class RippleController : MonoBehaviour
{
    public Material rippleMaterial;
    public Transform character;
    public float rippleStrength = 0.5f;
    public float rippleDistance = 5.0f;
    public float rippleMaxRadius = 10.0f; // Replace 10.0f with the desired value


    void Start()
    {
        
        if (rippleMaterial != null)
        {
            rippleMaterial.SetFloat("_RippleMaxRadius", rippleMaxRadius);
        }
    }
    
    void Update()
    {
        if (character != null && rippleMaterial != null)
        {
            // 这里需要将角色的世界坐标转换为地面网格的局部坐标
            Vector3 worldPosition = character.position;
            // 可能需要根据地面的实际放置和尺寸进行调整
            rippleMaterial.SetVector("_RippleOrigin", new Vector4(worldPosition.x, worldPosition.y, worldPosition.z,0));
            rippleMaterial.SetFloat("_RippleStrength", rippleStrength);
            rippleMaterial.SetFloat("_RippleDistance", rippleDistance);
        }
    }

    public void TriggerRipple()
    {
        if (rippleMaterial != null)
        {
            rippleMaterial.SetFloat("_RippleTime", Time.time);
        }
    }
}
