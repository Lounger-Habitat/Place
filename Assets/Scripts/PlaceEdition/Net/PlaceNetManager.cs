using UnityEngine;

public class PlaceNetManager : MonoBehaviour
{


    public string platform = "platform";
    public string anchorName = "anchor";
    public static PlaceNetManager Instance { get; set; }

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


    [Header("选择平台(不能都选)")]
    public bool bilibili = true;
    public bool douyin = false;



    [Header("Bilibili")]
    public PlaceBiliNetManager placeBiliNetManager;
    public GameObject bilibiliLogin;


    [Header("Douyin")]
    public PlaceDyNetManager placeDyNetManager;
    public bool isDyDebug = false;
    public GameObject douyinLog;





    void Start()
    {

        if (bilibili)
        {
            // 启用 bilibili net 脚本
            placeBiliNetManager.enabled = true;
            // 启用 登录界面
            bilibiliLogin.SetActive(true);
            platform = "bilibili";

            // 确保dy没有同时打开
            douyin = false;
            isDyDebug = false;
            if (douyinLog) douyinLog.SetActive(false);
            placeDyNetManager.enabled = false;

        }
        else
        {
            // 启用 douyin net 脚本
            placeDyNetManager.enabled = true;
            platform = "tiktok";
            // 启用 控制台
            if (isDyDebug)
            {
                douyinLog.SetActive(true);
            }

            // 确保bili没有同时打开
            bilibili = false;
            if (bilibiliLogin) bilibiliLogin.SetActive(false);
            placeBiliNetManager.enabled = false;

        }

    }

    void Reset()
    {
        bilibili = false;
        douyin = false;
        isDyDebug = false;
        if (bilibiliLogin) bilibiliLogin.SetActive(false);
        if (douyinLog) douyinLog.SetActive(false);
    }


}

