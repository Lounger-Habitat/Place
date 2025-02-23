using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllIn1VfxToolkit.Demo.Scripts;
using DG.Tweening;

namespace AllIn1VfxToolkit.Demo.Scripts {

public class EffectBulletBase : MonoBehaviour
{
    // [SerializeField] private float inaccurateAmount = 0.05f;
        
    private GameObject currentImpactPrefab;
    private Transform currentHierarchyParent;
    private bool doImpactSpawn;
    // private bool doShake = false;
    // private float shakeAmountOnImpact;
    private float impactScaleSize;
        
    public void Initialize(Transform hierarchyParent, Vector3 projectileDir, float speed, GameObject impactPrefab, float impactScaleSize)
    {
        currentHierarchyParent = hierarchyParent;
        currentImpactPrefab = impactPrefab;
        doImpactSpawn = currentImpactPrefab != null;
        this.impactScaleSize = impactScaleSize;

        //ApplyPrecisionOffsetToProjectileDir(ref projectileDir);
        //GetComponent<Rigidbody>().velocity = projectileDir * speed;
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
            DOTween.Kill(transform);
            Destroy(gameObject);
        }
    }
}

}