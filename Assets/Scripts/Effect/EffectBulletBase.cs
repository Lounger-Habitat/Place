using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllIn1VfxToolkit.Demo.Scripts;

namespace AllIn1VfxToolkit.Demo.Scripts {

public class EffectBulletBase : MonoBehaviour
{
    [SerializeField] private float inaccurateAmount = 0.05f;
        
    private GameObject currentImpactPrefab;
    private Transform currentHierarchyParent;
    private bool doImpactSpawn;
    private bool doShake = false;
    private float shakeAmountOnImpact, impactScaleSize;
        
    public void Initialize(Transform hierarchyParent, Vector3 projectileDir, float speed, GameObject impactPrefab, float impactScaleSize)
    {
        currentHierarchyParent = hierarchyParent;
        currentImpactPrefab = impactPrefab;
        doImpactSpawn = currentImpactPrefab != null;
        this.impactScaleSize = impactScaleSize;

        //ApplyPrecisionOffsetToProjectileDir(ref projectileDir);
        //GetComponent<Rigidbody>().velocity = projectileDir * speed;
    }
        
    public void AddScreenShakeOnImpact(float projectileImpactShakeAmount)
    {
        doShake = true;
        shakeAmountOnImpact = projectileImpactShakeAmount;
    }

    private void ApplyPrecisionOffsetToProjectileDir(ref Vector3 projectileDir)
    {
        projectileDir.x += Random.Range(-inaccurateAmount, inaccurateAmount);
        projectileDir.y += Random.Range(-inaccurateAmount, inaccurateAmount);
        projectileDir.z += Random.Range(-inaccurateAmount, inaccurateAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Board"))
        {
            if(doImpactSpawn)
            {
                Transform t = Instantiate(currentImpactPrefab, transform.position, Quaternion.identity).transform;
                t.parent = currentHierarchyParent;
                t.localScale *= impactScaleSize;
            }
            //if(doShake) AllIn1Shaker.i.DoCameraShake(shakeAmountOnImpact);//相机震动，目前没有
            Destroy(gameObject);
        }
    }
}

}