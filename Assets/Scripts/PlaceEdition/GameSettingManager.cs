using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingManager : MonoBehaviour
{
    public static GameSettingManager Instance { get; private set; }
    
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
    public GameMode mode;
}

public enum GameMode {
    Create, // 创作模式
    Graffiti, // 涂鸦模式
    Competition // 竞赛模式
}