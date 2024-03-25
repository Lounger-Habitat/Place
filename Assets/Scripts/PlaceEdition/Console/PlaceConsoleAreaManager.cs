using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class PlaceConsoleAreaManager : MonoBehaviour
{

    public GameObject ConsoleAreaName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 沿sin曲线上下浮动
        transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time) * 0.001f, transform.position.z);

        Vector2 mousePosition = Input.mousePosition;
        // 按下 E 键
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            // 射线的终点，这里我们假设射线无限远，或者您可以设置一个最大距离
            Vector3 rayEnd = ray.origin + ray.direction * 100f; // 例如，100单位远

            // 使用Debug.DrawLine可视化射线
            Debug.DrawLine(ray.origin, rayEnd, Color.green, float.PositiveInfinity, false);
            // Vector2 a = GetScreenPositionFromPixel();
            LaunchPaintEffect(mousePosition);
        }
    }

    // 发送 颜料 到画布
    void SendPaintToCanvas(Instruction ins)
    {
        // 1. 获取画布

        // 2. 获取颜料
        // 3. 将颜料发送到画布
    }

    public RawImage rawImage; // 假设这是您的RawImage对象
    public Vector2 pixelPosition; // 假设这是您想要转换的像素坐标 (x, y)

    public GameObject paintEffectPrefab; // 假设这是您的颜料特效预制体

    // 计算屏幕坐标的方法
    public Vector2 GetScreenPositionFromPixel(int x,int y)
    {
        // 获取Canvas的屏幕坐标
        Vector2 canvasScreenPosition = rawImage.transform.parent.GetComponent<RectTransform>().anchoredPosition;

        // 获取RawImage在Canvas中的位置
        Vector2 rawImagePosition = rawImage.GetComponent<RectTransform>().anchoredPosition;

        // 计算RawImage像素点的屏幕坐标
        // 注意：这里的pixelPosition.x和pixelPosition.y需要是归一化的值（0-1之间），如果是像素坐标需要先转换
        Vector2 screenPixelPosition = new Vector2(
            canvasScreenPosition.x + rawImagePosition.x * rawImage.texture.width * pixelPosition.x,
            canvasScreenPosition.y + rawImagePosition.y * rawImage.texture.height * pixelPosition.y
        );

        return screenPixelPosition;
    }

    // 发射颜料的方法
    public void LaunchPaintEffect(Vector2 screenPosition)
    {
        // 计算射线在摄像机视口坐标系中的位置
        Vector3 viewportPoint = new Vector3(screenPosition.x / Screen.width, screenPosition.y / Screen.height, 74f);
        // 将视口坐标转换为世界坐标
        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(viewportPoint);
        Debug.Log(worldPosition);
        // 在世界坐标位置创建特效
        GameObject paintEffect = Instantiate(paintEffectPrefab, worldPosition, Quaternion.identity);

    }
    

    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player")) // 确保触发器有一个特定的标签
    //     {

    //         // 角色刚刚进入触发器
    //         PlacePlayerController pc = other.transform.root.gameObject.GetComponent<PlacePlayerController>();
    //         string name = pc.user.username;
    //         Debug.Log(name + " 进入触发器");

    //         // 检查角色是否在队伍区域内
            
    //         // PlayerFSM stateMachine = other.transform.root.gameObject.GetComponent<PlayerFSM>();
    //         // if (pc.user.currentState == CharacterState.TransportingCommandToConsole)
    //         // {
    //         //     stateMachine.ChangeState(CharacterState.WaitingForCommandExecutionAtConsole);
    //         // }
            
    //     }
    // }

    // void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         // 角色刚刚离开触发器
    //         Debug.Log("角色离开触发器");
    //     }
    // }
}
