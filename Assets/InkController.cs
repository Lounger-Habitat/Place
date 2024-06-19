using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllIn1VfxToolkit.Demo.Scripts;
using DG.Tweening;

public class InkController : MonoBehaviour
{
    [SerializeField] private All1VfxDemoEffect InkBuilder;

    Transform inkFlash;
    Transform inkProjectile;
    Transform inkImpact;

    /*
        flash position
        projectile start position
        projectile end position
        impact positon
    */

    // Start is called before the first frame update
    void Start()
    {
        // 闪光
        // inkFlash = Instantiate(InkBuilder.muzzleFlashPrefab, transform.position, Quaternion.identity).transform;
        // inkFlash.parent = transform;

        // // 喷射
        // inkProjectile = Instantiate(InkBuilder.projectilePrefab, transform.position, Quaternion.identity).transform;
        // inkProjectile.parent = transform;

        // inkImpact = Instantiate(InkBuilder.impactPrefab, transform.position, Quaternion.identity).transform;
        // inkImpact.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void InitImpact(Transform hierarchyParent, Vector3 projectileDir, float speed, GameObject impactPrefab, float impactScaleSize)
    // {
    //     currentHierarchyParent = hierarchyParent;
    //     currentImpactPrefab = impactPrefab;
    //     doImpactSpawn = currentImpactPrefab != null;
    //     this.impactScaleSize = impactScaleSize;

    //     //ApplyPrecisionOffsetToProjectileDir(ref projectileDir);
    //     //GetComponent<Rigidbody>().velocity = projectileDir * speed;
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Board"))
    //     {
    //         if(doImpactSpawn)
    //         {
    //             Transform t = Instantiate(currentImpactPrefab, transform.position, Quaternion.identity).transform;
    //             t.parent = currentHierarchyParent;
    //             t.localScale *= impactScaleSize;
    //         }
    //         //if(doShake) AllIn1Shaker.i.DoCameraShake(shakeAmountOnImpact);//相机震动，目前没有
    //         DOTween.Kill(transform);
    //         Destroy(gameObject);
    //     }
    // }
}

        // // 发射的闪光特效
        // Transform tempTransform = null;
        // // 实例化闪光特效 （闪光特效，发射位置，方向）
        // tempTransform = Instantiate(currDemoEffect.muzzleFlashPrefab, spawnPoint.position, Quaternion.identity).transform;
        // tempTransform.localRotation = Quaternion.identity;
        // tempTransform.forward = spawnPoint.forward;
        // tempTransform.parent = transform;
        // tempTransform.localScale *= currDemoEffect.scaleMultiplier;

        // // 子弹
        // Transform projectileBase = Instantiate(projectileBasePrefab, spawnPoint.position, Quaternion.identity).transform;
        // projectileBase.LookAt(aimPos);
        // projectileBase.parent = transform;
        // //projectileBase.localRotation = Quaternion.identity;

        // tempTransform = Instantiate(currDemoEffect.projectilePrefab, spawnPoint.position, Quaternion.identity).transform;
        // tempTransform.localRotation = Quaternion.identity;
        // tempTransform.forward = projectileBase.forward;
        // tempTransform.parent = projectileBase;
        
        
        // EffectBulletBase tempProjectileInstance = projectileBase.GetComponent<EffectBulletBase>();
        // tempProjectileInstance.Initialize(transform, spawnPoint.forward, currDemoEffect.projectileSpeed, currDemoEffect.impactPrefab, currDemoEffect.scaleMultiplier);

        // projectileBase.DOMove(aimPos, 2.0f).SetEase(Ease.OutQuint).OnComplete(() =>
        // {
        //     Destroy(projectileBase.gameObject);
        // });