using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataNoDeleteManager : MonoBehaviour
{
    public static DataNoDeleteManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this) {
                Destroy(gameObject);
            }
        }
    }
  

    public bool isAutoPlay = false;
    public bool addAutoPlayer = false;
    public int playTime = 15;
    public int maxNumber = 25;
    public bool isNormalModel = true;
}
