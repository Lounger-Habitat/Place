using System;
using System.Collections;
using System.Collections.Generic;
using AllIn1VfxToolkit.Demo.Scripts;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class EffectFire : MonoBehaviour
{
    [SerializeField] private Transform endPosP;
    [SerializeField] private All1VfxDemoEffect currDemoEffect;
    [SerializeField] private GameObject projectileBasePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    public void PlayEffect()
    {
        //get end pos
        Vector3 endPos = endPosP.position + new Vector3(Random.Range(15, 30), Random.Range(15, 30), 0);
        Debug.Log(endPos);
        
        Transform tempTransform = null;
        tempTransform = Instantiate(currDemoEffect.muzzleFlashPrefab, projectileSpawnPoint.position, Quaternion.identity).transform;
        tempTransform.localRotation = Quaternion.identity;
        tempTransform.forward = projectileSpawnPoint.forward;
        tempTransform.parent = transform;
        tempTransform.localScale *= currDemoEffect.scaleMultiplier;
        
        
        
        Transform projectileBase = Instantiate(projectileBasePrefab, projectileSpawnPoint.position, Quaternion.identity).transform;
        projectileBase.LookAt(endPos);
        projectileBase.parent = transform;
        projectileBase.localRotation = Quaternion.identity;
        
        
        tempTransform = Instantiate(currDemoEffect.projectilePrefab, projectileSpawnPoint.position, Quaternion.identity).transform;
        tempTransform.localRotation = Quaternion.identity;
        tempTransform.forward = projectileBase.forward;
        tempTransform.parent = projectileBase;
        
        
        AllIn1DemoProjectile tempProjectileInstance = projectileBase.GetComponent<AllIn1DemoProjectile>();
        tempProjectileInstance.Initialize(transform, projectileSpawnPoint.forward, currDemoEffect.projectileSpeed, currDemoEffect.impactPrefab, currDemoEffect.scaleMultiplier);

        projectileBase.DOMove(endPos, 3.0f).SetEase(Ease.OutQuint);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayEffect();
        }
    }
}