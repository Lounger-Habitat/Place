using System;
using BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables;
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
    public GameDisplayRatio displayRatio = GameDisplayRatio.R9_16;
    

    private GameMode _mode;
    public GameMode Mode
    {
        get => _mode;
        set
        {
            if ((int)value>4)
            {
                _mode = GameMode.Create;
            }
            else
            {
                _mode = value;
            }
        }
    }

    [ContextMenu("Show")]
    public void showMode()
    {
        Debug.Log(Mode);
    }
}

public enum GameDisplayRatio
{
    R16_9 = 0,
    R9_16 = 4,
}

public enum GameMode {
    Create=1, // 创作模式
    Graffiti, // 涂鸦模式
    Competition, // 竞赛模式
    NewYear//新春模式
}

