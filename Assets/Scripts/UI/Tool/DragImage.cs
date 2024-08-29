using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(KeepFixed))]
public class DragImage : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    //private Camera mainCamera; // 主摄像机
    private Vector3 offset; // 鼠标点击位置与图片位置的偏移量


    // public void OnDrag(PointerEventData eventData)
    // {
    //     //将鼠标的位置坐标进行钳制，然后加上位置差再赋值给图片position
    //     ((RectTransform)transform).position = new Vector3(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width), Mathf.Clamp(Input.mousePosition.y, 0, Screen.height), 0) + offset;
    // }
    // public void OnPointerDown(PointerEventData eventData)
    // {
    //     offset = ((RectTransform)transform).position - Input.mousePosition;
    // }


    [Header("检测边距百分比")] [Range(0f, 1f)] public float MonitorPercent = 0.8f;


    /// <summary>
    /// 画布变化的大小限制值
    /// </summary>
    [Header("调整的最小值")] public Vector2 minSize = new Vector2(50, 50);

    [Header("调整的最大值")] public Vector2 maxSize = new Vector2(500, 500);

    [Header("边缘图片")]
    // 边缘图片
    public Transform Right_Image;

    public Transform Down_Image;
    public Transform Top_Image;
    public Transform Left_Image;

    // UI RectTransform
    private RectTransform panelRectTransform;

    // UI原始位置
    private Vector2 originalLocalPointerPosition;

    // UI原始大小
    private Vector2 originalSizeDelta;

    /// <summary>
    /// 鼠标刚进入时位置
    /// </summary>
    private Vector2 v2PointEnter;

    // 鼠标是否按下
    private bool isPointerDown = false;

    // 当前操作方向
    public UI_Edge ui_edge;

    public KeepFixed KeepFixed;

    void Awake()
    {
        KeepFixed = GetComponent<KeepFixed>();
        ui_edge = UI_Edge.None;

        // 初始化
        panelRectTransform = transform.GetComponent<RectTransform>();

        // 隐藏边缘图标
        SetActiveEdgeImage(false);
    }

    /// <summary>
    /// 鼠标在UI上按下的事件
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerDown(PointerEventData data)
    {
        //  Debug.Log("OnPointerDown-----------"+ ui_edge.ToString());
        if (ui_edge == UI_Edge.None)
        {
            offset = ((RectTransform)transform).position - Input.mousePosition;
            return;
        }

        // 鼠标按下
        isPointerDown = true;
        StopCoroutine("edgeJudge");
    }


    float x, y;

    /// <summary>
    /// 拖拽实现Resize
    /// </summary>
    /// <param name="data"></param>
    public void OnDrag(PointerEventData data)
    {
        if (ui_edge == UI_Edge.None)
        {
            //将鼠标的位置坐标进行钳制，然后加上位置差再赋值给图片position
            ((RectTransform)transform).position = new Vector3(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width),
                Mathf.Clamp(Input.mousePosition.y, 0, Screen.height), 0) + offset;
            return;
        }
        if (panelRectTransform == null || ui_edge == UI_Edge.None || !isPointerDown)
            return;

        switch (ui_edge)
        {
            case UI_Edge.None:
                //将鼠标的位置坐标进行钳制，然后加上位置差再赋值给图片position
                ((RectTransform)transform).position = new Vector3(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width),
                    Mathf.Clamp(Input.mousePosition.y, 0, Screen.height), 0) + offset;
                break;
            case UI_Edge.Top:
                x = panelRectTransform.sizeDelta.x;
                y = panelRectTransform.sizeDelta.y + data.delta.y;
                break;
            case UI_Edge.Down:
                x = panelRectTransform.sizeDelta.x;
                y = panelRectTransform.sizeDelta.y - data.delta.y;
                break;
            case UI_Edge.Left:
                x = panelRectTransform.sizeDelta.x - data.delta.x;
                y = panelRectTransform.sizeDelta.y;
                break;
            case UI_Edge.Right:
                x = panelRectTransform.sizeDelta.x + data.delta.x;
                y = panelRectTransform.sizeDelta.y;
                break;
            default:
                break;
        }
        //直接进行赋值，这样会整体改变宽和高
        // panelRectTransform.sizeDelta = new Vector2(Mathf.Clamp(x, minSize.x, maxSize.x), Mathf.Clamp(y, minSize.y, maxSize.y));

//下面采取了固定某个边的方式，单边改变宽和高
        KeepFixed.SetPanelFixed(ui_edge,
            new Vector2(Mathf.Clamp(x, minSize.x, maxSize.x), Mathf.Clamp(y, minSize.y, maxSize.y)));
    }


    /// <summary>
    /// 鼠标抬起事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        // Debug.Log("OnPointerUp-----------"); 
        isPointerDown = false;
        ui_edge = UI_Edge.None;
        SetActiveEdgeImage(false);
        StopCoroutine("edgeJudge");
    }

    /// <summary>
    /// 鼠标进入UI的事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter-----------");
        //Debug.Log(" eventData.position:" + eventData.position);
        if (isPointerDown)
        {
            return;
        }

        // 开启边缘检测
        StartCoroutine("edgeJudge");
    }

    /// <summary>
    /// 鼠标退出UI的事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isPointerDown)
        {
            return;
        }

        Debug.Log("OnPointerExit-----------");
        isPointerDown = false;
        StopCoroutine("edgeJudge");
        SetActiveEdgeImage(false);
    }


    /// <summary>
    /// 设置边缘画的显隐
    /// </summary>
    /// <param name="isActive"></param>
    private void SetActiveEdgeImage(bool isActive)
    {
        Right_Image.gameObject.SetActive(isActive);
        Down_Image.gameObject.SetActive(isActive);
        Left_Image.gameObject.SetActive(isActive);
        Top_Image.gameObject.SetActive(isActive);
    }


    /// <summary>
    /// 边缘判断协程
    /// </summary>
    /// <returns></returns>
    IEnumerator edgeJudge()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform,
                Input.mousePosition, null,
                out v2PointEnter);
            //  Debug.Log("_pos:" + v2PointEnter);  
            ui_edge = GetCurrentEdge(v2PointEnter);
            //v2PointEnter = Input.mousePosition;

            SetActiveEdgeImage(false);

            switch (ui_edge)
            {
                case UI_Edge.None:
                    SetActiveEdgeImage(false);
                    break;
                case UI_Edge.Left:
                    Left_Image.gameObject.SetActive(true);
                    // Left_Image.localPosition = new Vector3(Left_Image.localPosition.x,
                    //     Mathf.Clamp(v2PointEnter.y, -panelRectTransform.rect.height / 2,
                    //         panelRectTransform.rect.height / 2),
                    //     Left_Image.localPosition.z);
                    break;
                case UI_Edge.Right:
                    Right_Image.gameObject.SetActive(true);
                    // Right_Image.localPosition = new Vector3(Right_Image.localPosition.x,
                    //     Mathf.Clamp(v2PointEnter.y, -panelRectTransform.rect.height / 2,
                    //         panelRectTransform.rect.height / 2),
                    //     Right_Image.localPosition.z);
                    break;
                case UI_Edge.Down:
                    Down_Image.gameObject.SetActive(true);
                    // Down_Image.localPosition = new Vector3(
                    //     Mathf.Clamp(v2PointEnter.x, -panelRectTransform.rect.width / 2,
                    //         panelRectTransform.rect.width / 2),
                    //     Down_Image.localPosition.y, Down_Image.localPosition.z);

                    break;
                case UI_Edge.Top:
                    Top_Image.gameObject.SetActive(true);
                    // Top_Image.localPosition = new Vector3(
                    //     Mathf.Clamp(v2PointEnter.x, -panelRectTransform.rect.width / 2,
                    //         panelRectTransform.rect.width / 2),
                    //     Top_Image.localPosition.y, Top_Image.localPosition.z);

                    break;
                default:
                    SetActiveEdgeImage(false);
                    break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// 判断鼠标在 Panel 的那个边缘
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private UI_Edge GetCurrentEdge(Vector2 pos)
    {
        if (pos.x < 0 && pos.x < -panelRectTransform.sizeDelta.x / 2 * MonitorPercent)
        {
            return UI_Edge.Left;
        }

        else if (pos.x > 0 && pos.x > panelRectTransform.sizeDelta.x / 2 * MonitorPercent)
        {
            return UI_Edge.Right;
        }
        else
        {
            if (pos.y < 0 && pos.y < -panelRectTransform.sizeDelta.y / 2 * MonitorPercent)
            {
                return UI_Edge.Down;
            }
            else if (pos.y > 0 && pos.y > panelRectTransform.sizeDelta.y / 2 * MonitorPercent)
            {
                return UI_Edge.Top;
            }
        }

        return UI_Edge.None;
    }

    private void SetWidthHeight(UI_Edge tuI_Edge, Vector2 wh)
    {
        Vector2 V2Pos = panelRectTransform.anchoredPosition;
        Vector2 V2Size = panelRectTransform.sizeDelta;
    }
}

public enum UI_Edge
{
    None,
    Top,
    Down,
    Left,
    Right,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}