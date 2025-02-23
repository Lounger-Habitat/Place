using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlaceBiliLoginManager : MonoBehaviour
{

    [Header("记住身份码")]
    public string IsSaveCodeSaveKey = "IsSaveIdCode";
    public bool IsSaveIdCode;
    public Toggle SaveIdCodeToggle;

    [Header("身份码输入框")]
    public string IdCodeSaveKey = "IdCode";
    public string IdCode;
    public InputField IdCodeInputField;

    [Header("<去获取>超链接")]
    public string HyperLinkUrl;
    public Text HyperLinkText;

    [Header("开启玩法")]
    public Button StartToPlayButton;

    [Header("Others")]
    public Button CloseButton;
    public Animator AnimationController;
    public string ShowAnim = "Show";
    public string HideAnim = "Hide";
    private int showHash;
    private int hideHash;

    [Header("Events")]
    public UnityEvent StartToPlayEvent;//点击开启玩法按钮时触发
    public UnityEvent LinkSuccessEvent;//连接成功时触发
    public UnityEvent LinkFailedEvent;//连接失败时触发
    public UnityEvent ShowEvent;//显示UI时触发
    public UnityEvent HideEvent;//隐藏UI时触发

    public static PlaceBiliLoginManager Instance { get; private set; }

    public bool connected = false;

    // public static PlaceBiliLoginManager Instance {
    //     get
    //     {
    //         if (instance == null)
    //         {
    //             instance = FindObjectOfType<PlaceBiliLoginManager>();
    //             DontDestroyOnLoad(instance);
    //         }
    //         return instance;
    //     }
    // }

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else {
            if (Instance != this) {
                Destroy(gameObject);
            }
        }
    }


    public virtual void Initial()
    {
        //config read and init
        IsSaveIdCode = BilibiliPlayerPrefs.GetBool(IsSaveCodeSaveKey);
        SaveIdCodeToggle.isOn = IsSaveIdCode;
        if (SaveIdCodeToggle)
        {
            IdCode = BilibiliPlayerPrefs.GetString(IdCodeSaveKey);
        }
        else
        {
            IdCode = string.Empty;
            BilibiliPlayerPrefs.SetString(IdCodeSaveKey, IdCode);
        }
        IdCodeInputField.text = IdCode;

        //add ui listener
        IdCodeInputField.onValueChanged.AddListener(ChangeIdCode);
        SaveIdCodeToggle.onValueChanged.AddListener(ChangeIsSaveIdCode);
        StartToPlayButton.onClick.AddListener(StartToPlay);

        CloseButton.onClick.AddListener(Hide);

        //init url
        HyperLinkText.text = $"<a href={HyperLinkUrl}>去获取</a>";

        //init aniamtion hash
        showHash = Animator.StringToHash(ShowAnim);
        hideHash = Animator.StringToHash(HideAnim);


        //init openblive sdk
        if (PlaceBiliNetManager.Instance != null)
        {
            PlaceBiliNetManager.Instance.ConnectSuccess += LinkSuccess;
            PlaceBiliNetManager.Instance.ConnectFailure += LinkFailed;
        }

        LinkSuccessEvent.AddListener(Connected);

        if (IdCodeInputField.text != string.Empty) {
            if (connected == false) {
                StartToPlay();
            }else {
                Hide();
            }
        }

    }

    /// <summary>
    /// 修改Id Code
    /// </summary>
    /// <param name="code"></param>
    public virtual void ChangeIdCode(string code)
    {
#if UNITY_EDITOR
        Debug.Log("ID Code is changed...");
#endif
        //处理错误字符
        var result = code.Replace(" ", string.Empty);
        result = result.Replace("\n", string.Empty);
        result = result.Replace("\r", string.Empty);
        result = result.Replace("\f", string.Empty);

        IdCodeInputField.SetTextWithoutNotify(result);
        IdCode = result;

        //if (IsSaveIdCode)
        //{
        //    BilibiliPlayerPrefs.SetString(IdCodeSaveKey, IdCode);
        //}
    }

    /// <summary>
    /// 修改是否保存Id Code
    /// </summary>
    /// <param name="isOn"></param>
    public virtual void ChangeIsSaveIdCode(bool isOn)
    {
#if UNITY_EDITOR
        Debug.Log("Save ID Code state is changed...");
#endif
        IsSaveIdCode = isOn;
        BilibiliPlayerPrefs.SetBool(IsSaveCodeSaveKey, IsSaveIdCode);

        if (!IsSaveIdCode)
        {
            BilibiliPlayerPrefs.SetString(IdCodeSaveKey, string.Empty);
        }
    }

    /// <summary>
    /// 点击开启玩法时触发
    /// </summary>
    public virtual void StartToPlay()
    {
        PlaceBiliNetManager.Instance?.LinkStart(IdCode);
        StartToPlayEvent?.Invoke();
    }

    /// <summary>
    /// 连接成功时触发
    /// </summary>
    protected virtual void LinkSuccess()
    {
        if (IsSaveIdCode)
        {
            BilibiliPlayerPrefs.SetString(IdCodeSaveKey, IdCode);
        }
        Debug.Log("连接成功");
        LinkSuccessEvent?.Invoke();
    }

    /// <summary>
    /// 连接失败时触发
    /// </summary>
    protected virtual void LinkFailed()
    {
        LinkFailedEvent?.Invoke();
        Debug.LogError("连接失败");
    }

    /// <summary>
    /// 显示UI
    /// </summary>
    protected virtual void Show()
    {
        if (AnimationController != null)
        {
            AnimationController.Play(showHash);
        }
        ShowEvent?.Invoke();
        Debug.Log("显示身份码弹框");
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    protected virtual void Hide()
    {
        if (AnimationController != null)
        {
            AnimationController.Play(hideHash);
        }
        HideEvent?.Invoke();
        Debug.Log("关闭身份码弹框");
    }


    #region Unity Func
    protected virtual void Start()
    {
        Initial();
    }
    #endregion


    void Connected() {
        connected = true;
        Hide();
    }
}
