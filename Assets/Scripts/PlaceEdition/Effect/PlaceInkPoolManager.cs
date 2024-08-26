using System.Collections.Generic;
using AllIn1VfxToolkit.Demo.Scripts;
using UnityEngine;

public class PlaceInkPoolManager : MonoBehaviour
{
    public static PlaceInkPoolManager Instance { get; private set; }

    [SerializeField] private All1VfxDemoEffect InkBuilder;
    [SerializeField] private All1VfxDemoEffect BlueInkBuilder;
    [SerializeField] private All1VfxDemoEffect GreenInkBuilder;
    public int poolAmount = 10;

    // Blue
    public List<GameObject> BlueInkProjectilesPools;
    public List<GameObject> BlueInkFlashPools;
    public List<GameObject> BlueInkImpactPools;

    // Green
    public List<GameObject> GreenInkProjectilesPools;
    public List<GameObject> GreenInkFlashPools;
    public List<GameObject> GreenInkImpactPools;

    public bool willGrow;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        { 
            Destroy(gameObject);
        }
    }

    void Start()
    {
        BlueStart();
        GreenStart();
    }

    void BlueStart()
    {
        BlueInkProjectilesPools = new List<GameObject>();
        BlueInkFlashPools = new List<GameObject>();
        BlueInkImpactPools = new List<GameObject>();
        for (int i = 0; i < poolAmount; i++)
        {
            GameObject blueInkFlash = Instantiate(BlueInkBuilder.muzzleFlashPrefab,transform);
            GameObject blueInkProjectile = Instantiate(BlueInkBuilder.projectilePrefab, transform.position, Quaternion.identity,transform);
            GameObject blueInkImpact = Instantiate(BlueInkBuilder.impactPrefab, transform.position, Quaternion.identity,transform);

            blueInkFlash.SetActive(false);
            blueInkProjectile.SetActive(false);
            blueInkImpact.SetActive(false);


            BlueInkProjectilesPools.Add(blueInkProjectile);
            BlueInkFlashPools.Add(blueInkFlash);
            BlueInkImpactPools.Add(blueInkImpact);

        }

    }
    void GreenStart()
    {
        GreenInkProjectilesPools = new List<GameObject>();
        GreenInkFlashPools = new List<GameObject>();
        GreenInkImpactPools = new List<GameObject>();
        for (int i = 0; i < poolAmount; i++)
        {
            GameObject greenInkFlash = Instantiate(GreenInkBuilder.muzzleFlashPrefab,transform);
            GameObject greenInkProjectile = Instantiate(GreenInkBuilder.projectilePrefab, transform.position, Quaternion.identity,transform);
            GameObject greenInkImpact = Instantiate(GreenInkBuilder.impactPrefab, transform.position, Quaternion.identity,transform);

            greenInkFlash.SetActive(false);
            greenInkProjectile.SetActive(false);
            greenInkImpact.SetActive(false);

            GreenInkProjectilesPools.Add(greenInkProjectile);
            GreenInkFlashPools.Add(greenInkFlash);
            GreenInkImpactPools.Add(greenInkImpact);

        }
    }

    void Update()
    {

    }

    // public GameObject GetInkProjectile()
    // {
    //     for (int i = 0; i < InkProjectilesPools.Count; i++)
    //     {
    //         if (!InkProjectilesPools[i].activeInHierarchy)
    //         {
    //             InkProjectilesPools[i].SetActive(true);
    //             return InkProjectilesPools[i];
    //         }
    //     }
    //     if (willGrow)
    //     {
    //         GameObject obj = Instantiate(InkBuilder.projectilePrefab);
    //         InkProjectilesPools.Add(obj);
    //         return obj;
    //     }

    //     return null;
    // }

    public GameObject GetBlueInkProjectile()
    {
        for (int i = 0; i < BlueInkProjectilesPools.Count; i++)
        {
            if (!BlueInkProjectilesPools[i].activeInHierarchy)
            {
                BlueInkProjectilesPools[i].SetActive(true);
                return BlueInkProjectilesPools[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = Instantiate(BlueInkBuilder.projectilePrefab,transform);
            BlueInkProjectilesPools.Add(obj);
            return obj;
        }

        return null;
    }

    public GameObject GetGreenInkProjectile()
    {
        for (int i = 0; i < GreenInkProjectilesPools.Count; i++)
        {
            if (!GreenInkProjectilesPools[i].activeInHierarchy)
            {
                GreenInkProjectilesPools[i].SetActive(true);
                return GreenInkProjectilesPools[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = Instantiate(GreenInkBuilder.projectilePrefab,transform);
            GreenInkProjectilesPools.Add(obj);
            return obj;
        }

        return null;
    }

    public GameObject GetBlueInkFlash()
    {
        for (int i = 0; i < BlueInkFlashPools.Count; i++)
        {
            if (!BlueInkFlashPools[i].activeInHierarchy)
            {
                BlueInkFlashPools[i].SetActive(true);
                return BlueInkFlashPools[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = Instantiate(BlueInkBuilder.muzzleFlashPrefab,transform);
            BlueInkFlashPools.Add(obj);
            return obj;
        }

        return null;
    }
    public GameObject GetGreenInkFlash()
    {
        for (int i = 0; i < GreenInkFlashPools.Count; i++)
        {
            if (!GreenInkFlashPools[i].activeInHierarchy)
            {
                GreenInkFlashPools[i].SetActive(true);
                return GreenInkFlashPools[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = Instantiate(GreenInkBuilder.muzzleFlashPrefab,transform);
            GreenInkFlashPools.Add(obj);
            return obj;
        }

        return null;
    }

    public GameObject GetBlueInkImpact()
    {
        for (int i = 0; i < BlueInkImpactPools.Count; i++)
        {
            if (!BlueInkImpactPools[i].activeInHierarchy)
            {
                BlueInkImpactPools[i].SetActive(true);
                return BlueInkImpactPools[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = Instantiate(BlueInkBuilder.impactPrefab,transform);
            BlueInkImpactPools.Add(obj);
            return obj;
        }

        return null;
    }
    public GameObject GetGreenInkImpact()
    {
        for (int i = 0; i < GreenInkImpactPools.Count; i++)
        {
            if (!GreenInkImpactPools[i].activeInHierarchy)
            {
                GreenInkImpactPools[i].SetActive(true);
                return GreenInkImpactPools[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = Instantiate(GreenInkBuilder.impactPrefab,transform);
            GreenInkImpactPools.Add(obj);
            return obj;
        }

        return null;
    }
}
