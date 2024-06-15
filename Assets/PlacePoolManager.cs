using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePoolManager : MonoBehaviour
{
    public static PlacePoolManager Instance { get; private set; }
    public GameObject inkPrefab;
    public int poolAmount = 10;
 
    public List<GameObject> InkPools;
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
        InkPools = new List<GameObject>();
        for (int i = 0; i < poolAmount; i++)
        {
            GameObject obj = Instantiate(inkPrefab);
            obj.SetActive(false);
            InkPools.Add(obj);
        }
    }

    void Update()
    {
        
    }

    public GameObject GetInkInstance()
    {
        for (int i = 0; i < InkPools.Count; i++)
        {
            if (!InkPools[i].activeInHierarchy)
            {
                return InkPools[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = Instantiate(inkPrefab);
            InkPools.Add(obj);
            return obj;
        }
        
        return null;
    }
}
