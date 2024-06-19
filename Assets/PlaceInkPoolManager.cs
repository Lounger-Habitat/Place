using System.Collections.Generic;
using AllIn1VfxToolkit.Demo.Scripts;
using UnityEngine;

public class PlaceInkPoolManager : MonoBehaviour
{
    public static PlaceInkPoolManager Instance { get; private set; }
    [SerializeField] private All1VfxDemoEffect InkBuilder;
    public int poolAmount = 10;

    public List<GameObject> InkProjectilesPools;
    public List<GameObject> InkFlashPools;
    public List<GameObject> InkImpactPools;

    public bool willGrow;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InkProjectilesPools = new List<GameObject>();
        InkFlashPools = new List<GameObject>();
        InkImpactPools = new List<GameObject>();
        for (int i = 0; i < poolAmount; i++)
        {
            GameObject inkFlash = Instantiate(InkBuilder.muzzleFlashPrefab);
            GameObject inkProjectile = Instantiate(InkBuilder.projectilePrefab, transform.position, Quaternion.identity);
            GameObject inkImpact = Instantiate(InkBuilder.impactPrefab, transform.position, Quaternion.identity);

            inkFlash.SetActive(false);
            inkProjectile.SetActive(false);
            inkImpact.SetActive(false);

            InkProjectilesPools.Add(inkProjectile);
            InkFlashPools.Add(inkFlash);
            InkImpactPools.Add(inkImpact);

        }
    }

    void Update()
    {

    }

    public GameObject GetInkProjectile()
    {
        for (int i = 0; i < InkProjectilesPools.Count; i++)
        {
            if (!InkProjectilesPools[i].activeInHierarchy)
            {
                InkProjectilesPools[i].SetActive(true);
                return InkProjectilesPools[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = Instantiate(InkBuilder.projectilePrefab);
            InkProjectilesPools.Add(obj);
            return obj;
        }

        return null;
    }

    public GameObject GetInkFlash()
    {
        for (int i = 0; i < InkFlashPools.Count; i++)
        {
            if (!InkFlashPools[i].activeInHierarchy)
            {
                InkFlashPools[i].SetActive(true);
                return InkFlashPools[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = Instantiate(InkBuilder.muzzleFlashPrefab);
            InkFlashPools.Add(obj);
            return obj;
        }

        return null;
    }

    public GameObject GetInkImpact()
    {
        for (int i = 0; i < InkImpactPools.Count; i++)
        {
            if (!InkImpactPools[i].activeInHierarchy)
            {
                InkImpactPools[i].SetActive(true);
                return InkImpactPools[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = Instantiate(InkBuilder.impactPrefab);
            InkImpactPools.Add(obj);
            return obj;
        }

        return null;
    }
}
