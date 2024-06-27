using AllIn1VfxToolkit.Demo.Scripts;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections;

public class PlaceConsoleAreaManager : MonoBehaviour
{

    public GameObject ConsoleAreaName;
    // Start is called before the first frame update
    public GameObject ALine;


    [Header("颜料特效")]
    [SerializeField] private Transform endPosBottom;
    [SerializeField] private Transform endPosTop;
    [SerializeField] private Transform spawnPoint;



    [SerializeField] private Transform team1EndPosTop;
    [SerializeField] private Transform team1EndPosBottom;



    [SerializeField] private Transform team2EndPosTop;
    [SerializeField] private Transform team2EndPosBottom;



    public int debugx = 0;
    public int debugy = 0;

    private int boradWidth;
    private int boradHeight;
    private Vector3 frame;
    private float pixelWidth;
    private float pixelHeight;

    public static PlaceConsoleAreaManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // ConsoleAreaName = GameObject.Find("ConsoleAreaName");
        // ConsoleAreaName.SetActive(false);
        // 获取 frame 宽 高

        // TODO 滞后修改
        if (GameSettingManager.Instance.mode == GameMode.Create)
        {
            boradWidth = PlaceBoardManager.Instance.width;
            boradHeight = PlaceBoardManager.Instance.height;
            frame = endPosTop.position - endPosBottom.position;
            pixelWidth = frame.x / boradWidth;
            pixelHeight = frame.y / boradHeight;
        }
        else
        {
            boradWidth = PlaceTeamBoardManager.Instance.width;
            boradHeight = PlaceTeamBoardManager.Instance.height;
            frame = team1EndPosTop.position - team1EndPosBottom.position;
            pixelWidth = frame.x / boradWidth;
            pixelHeight = frame.y / boradHeight;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 沿sin曲线上下浮动 (No) Just 自转
        // transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time) * 0.001f, transform.position.z);

        // 缓慢自转
        // transform.Rotate(Vector3.up, 0.1f);


        // Vector2 mousePosition = Input.mousePosition;
        // 按下 E 键
        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     // Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        //
        //     // // 射线的终点，这里我们假设射线无限远，或者您可以设置一个最大距离
        //     // Vector3 rayEnd = ray.origin + ray.direction * 100f; // 例如，100单位远
        //
        //     // // 使用Debug.DrawLine可视化射线
        //     // Debug.DrawLine(ray.origin, rayEnd, Color.green, float.PositiveInfinity, false);
        //     // Vector2 a = GetScreenPositionFromPixel();
        //     // LaunchPaintEffect(mousePosition);
        //
        //     // int x = Random.Range(0, 500);
        //     // int y = Random.Range(0, 500);
        //     PlayEffect(debugx,debugy,1);
        // }
    }

    // // 发送 颜料 到画布
    // void SendPaintToCanvas(Instruction ins)
    // {
    //     // 1. 获取画布

    //     // 2. 获取颜料
    //     // 3. 将颜料发送到画布
    // }

    // public RawImage rawImage; // 假设这是您的RawImage对象
    // public Vector2 pixelPosition; // 假设这是您想要转换的像素坐标 (x, y)

    // public GameObject paintEffectPrefab; // 假设这是您的颜料特效预制体

    // 计算屏幕坐标的方法
    // public Vector2 GetScreenPositionFromPixel(int x,int y)
    // {
    //     // 获取Canvas的屏幕坐标
    //     Vector2 canvasScreenPosition = rawImage.transform.parent.GetComponent<RectTransform>().anchoredPosition;

    //     // 获取RawImage在Canvas中的位置
    //     Vector2 rawImagePosition = rawImage.GetComponent<RectTransform>().anchoredPosition;

    //     // 计算RawImage像素点的屏幕坐标
    //     // 注意：这里的pixelPosition.x和pixelPosition.y需要是归一化的值（0-1之间），如果是像素坐标需要先转换
    //     Vector2 screenPixelPosition = new Vector2(
    //         canvasScreenPosition.x + rawImagePosition.x * rawImage.texture.width * pixelPosition.x,
    //         canvasScreenPosition.y + rawImagePosition.y * rawImage.texture.height * pixelPosition.y
    //     );

    //     return screenPixelPosition;
    // }

    // // 发射颜料的方法
    // public void LaunchPaintEffect(Vector2 screenPosition)
    // {
    //     // 计算射线在摄像机视口坐标系中的位置
    //     Vector3 viewportPoint = new Vector3(screenPosition.x / Screen.width, screenPosition.y / Screen.height, 74f);
    //     // 将视口坐标转换为世界坐标
    //     Vector3 worldPosition = Camera.main.ViewportToWorldPoint(viewportPoint);
    //     Debug.Log(worldPosition);
    //     // 在世界坐标位置创建特效
    //     GameObject paintEffect = Instantiate(paintEffectPrefab, worldPosition, Quaternion.identity);

    // }

    public void PlayEffect(int x, int y, int camp)
    {
        // 检测 x,y 小于 width height, 超出部分做截断处理
        // x = Mathf.Clamp(x, 0, boradWidth);
        // y = Mathf.Clamp(y, 0, boradHeight);

        // Vector3 delta = new Vector3((float)(x * pixelWidth), (float)(y * pixelHeight), 0f);
        // Vector3 aimPos = endPosBottom.position + delta;


        Vector3 aimPos = CalAimPos(x, y, camp);

        GameObject projGo;
        if (camp == 1)
        {
            projGo = PlaceInkPoolManager.Instance.GetBlueInkProjectile();
        }
        else
        {
            projGo = PlaceInkPoolManager.Instance.GetGreenInkProjectile();
        }
        projGo.transform.position = spawnPoint.position;
        projGo.transform.forward = spawnPoint.forward;
        ProjectileController projScript = projGo.GetComponent<ProjectileController>();
        projScript.camp = camp;
        projScript.Launch(aimPos, 2.0f);


        // Transform tempTransform = null;
        // tempTransform = Instantiate(currDemoEffect.muzzleFlashPrefab, spawnPoint.position, Quaternion.identity).transform;
        // tempTransform.localRotation = Quaternion.identity;
        // tempTransform.forward = spawnPoint.forward;
        // tempTransform.parent = transform;
        // tempTransform.localScale *= currDemoEffect.scaleMultiplier;



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
    }

    public void VisualAuxiliaryLine(int x, int y, int camp)
    {
        float offset = 1f;
        float fadeTime = 3f;
        Vector3 aimPos = CalAimPos(x, y, camp);
        // 实例化 ALine
        GameObject temp = Instantiate(ALine, endPosBottom.position, Quaternion.identity);
        // 获取 ALine 的 子物体
        LineRenderer XAxis = temp.transform.GetChild(0).GetComponent<LineRenderer>();
        LineRenderer YAxis = temp.transform.GetChild(1).GetComponent<LineRenderer>();

        // 获取 ALine 的 Axis Text
        TextMeshPro X1AxisText = temp.transform.GetChild(2).GetComponent<TextMeshPro>();
        TextMeshPro X2AxisText = temp.transform.GetChild(3).GetComponent<TextMeshPro>();
        TextMeshPro Y1AxisText = temp.transform.GetChild(4).GetComponent<TextMeshPro>();
        TextMeshPro Y2AxisText = temp.transform.GetChild(5).GetComponent<TextMeshPro>();

        // 设置 ALine X的位置
        XAxis.SetPosition(0, new Vector3(endPosBottom.position.x, aimPos.y, aimPos.z));
        XAxis.SetPosition(1, new Vector3(endPosTop.position.x, aimPos.y, aimPos.z));

        // 设置 ALine Y的位置
        YAxis.SetPosition(0, new Vector3(aimPos.x, endPosBottom.position.y, aimPos.z));
        YAxis.SetPosition(1, new Vector3(aimPos.x, endPosTop.position.y, aimPos.z));

        // 设置 ALine 的颜色

        // 设置 ALine 的宽度
        YAxis.startWidth = 0.1f;
        YAxis.endWidth = 0.1f;
        XAxis.startWidth = 0.1f;
        XAxis.endWidth = 0.1f;

        // 设置 Text 位置
        X1AxisText.transform.position = new Vector3(endPosBottom.position.x - offset, aimPos.y, aimPos.z);
        X2AxisText.transform.position = new Vector3(endPosTop.position.x + offset, aimPos.y, aimPos.z);
        Y1AxisText.transform.position = new Vector3(aimPos.x, endPosBottom.position.y - 0.5f, aimPos.z);
        Y2AxisText.transform.position = new Vector3(aimPos.x, endPosTop.position.y + 0.5f, aimPos.z);
        // 设置 Text 文字
        X1AxisText.text = "" + x;
        X2AxisText.text = "" + x;
        Y1AxisText.text = "" + y;
        Y2AxisText.text = "" + y;

        StartCoroutine(FadeOutLineRenderer(XAxis, fadeTime));
        StartCoroutine(FadeOutLineRenderer(YAxis, fadeTime));
        StartCoroutine(FadeOutTextMeshPro(X1AxisText, fadeTime));
        StartCoroutine(FadeOutTextMeshPro(X2AxisText, fadeTime));
        StartCoroutine(FadeOutTextMeshPro(Y1AxisText, fadeTime));
        StartCoroutine(FadeOutTextMeshPro(Y2AxisText, fadeTime));
        // 三秒后销毁
        Destroy(temp, 3.5f);


    }

    IEnumerator FadeOutTextMeshPro(TextMeshPro tmp, float duration)
    {
        // 从当前Alpha值渐变到0
        tmp.DOKill(); // 首先停止所有正在运行的动画
        tmp.DOFade(0f, duration); // 渐隐动画
        yield return new WaitForSeconds(duration); // 等待动画完成
        tmp.gameObject.SetActive(false); // 可选：在动画结束后禁用TextMeshPro对象
    }

    IEnumerator FadeOutLineRenderer(LineRenderer lr, float duration)
    {
        // 从当前Alpha值渐变到0
        lr.material.DOKill(); // 首先停止所有正在运行的动画
        lr.material.DOFade(0f, duration); // 渐隐动画
        yield return new WaitForSeconds(duration); // 等待动画完成
        lr.enabled = false; // 可选：在动画结束后禁用LineRenderer
    }

    public Vector3 CalAimPos(int x, int y, int camp)
    {
        x = Mathf.Clamp(x, 0, boradWidth);
        y = Mathf.Clamp(y, 0, boradHeight);
        Vector3 delta = new Vector3((float)(x * pixelWidth), (float)(y * pixelHeight), 0f);

        Vector3 aimPos;
        if (GameSettingManager.Instance.mode == GameMode.Competition)
        {
            if (camp == 1)
            {
                aimPos = team1EndPosBottom.position + delta;
            }
            else
            {
                aimPos = team2EndPosBottom.position + delta;
            }
        }
        else
        {
            aimPos = endPosBottom.position + delta;
        }
        return aimPos;
    }
}
