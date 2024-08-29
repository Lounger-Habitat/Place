using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepFixed : MonoBehaviour
{
     [SerializeField]
    private UI_Edge uI_Edge;
    RectTransform panelRectTransform;
 
    /// <summary>
    /// 位置
    /// </summary>
    private Vector2 originalLocalPointerPosition;

    /// <summary>
    /// 大小
    /// </summary>
    private Vector2 originalSizeDelta;
    // Start is called before the first frame update
    void Start()
    {
        panelRectTransform=this.GetComponent<RectTransform>();

        originalLocalPointerPosition=panelRectTransform.anchoredPosition;
        originalSizeDelta = panelRectTransform.sizeDelta;
    }
     /// <summary>
 /// 通过传入缩放的方向和大小，动态改变图片的位置和大小
 /// </summary>
 /// <param name="pEdge"></param>
 /// <param name="v2"></param>
    public void SetPanelFixed(UI_Edge pEdge,Vector2 v2) 
    {
        uI_Edge=pEdge;
        switch (pEdge)
        {
            case UI_Edge.None:
                break;
            case UI_Edge.Top:
                originalLocalPointerPosition = panelRectTransform.anchoredPosition;
                originalSizeDelta = panelRectTransform.sizeDelta;
                if (v2.y> originalSizeDelta.y)
                {
                    panelRectTransform.sizeDelta = v2;
                    originalLocalPointerPosition.y += ((v2.y - originalSizeDelta.y) / 2);
                    panelRectTransform.anchoredPosition = originalLocalPointerPosition;

                }
                else if (v2.y< originalSizeDelta.y)
                {
                    panelRectTransform.sizeDelta = v2;
                    originalLocalPointerPosition.y -= ((originalSizeDelta.y - v2.y) / 2);
                    panelRectTransform.anchoredPosition = originalLocalPointerPosition;
                }
                else
                {
                   // Debug.Log("达到 最大值");
                }
                break;
            case UI_Edge.Down:
                originalLocalPointerPosition = panelRectTransform.anchoredPosition;
                originalSizeDelta = panelRectTransform.sizeDelta;
                if (v2.y > originalSizeDelta.y)
                {
                    panelRectTransform.sizeDelta = v2;
                    originalLocalPointerPosition.y -= ((v2.y - originalSizeDelta.y) / 2);
                    panelRectTransform.anchoredPosition = originalLocalPointerPosition;

                }
                else if (v2.y < originalSizeDelta.y)
                {
                    panelRectTransform.sizeDelta = v2;
                    originalLocalPointerPosition.y += ((originalSizeDelta.y - v2.y) / 2);
                    panelRectTransform.anchoredPosition = originalLocalPointerPosition;
                }
                else
                {
                   // Debug.Log("达到 最大值");
                }
                break;
            case UI_Edge.Left:
                originalLocalPointerPosition = panelRectTransform.anchoredPosition;
                originalSizeDelta = panelRectTransform.sizeDelta;
                if ( v2.x> originalSizeDelta.x)
                {
                    panelRectTransform.sizeDelta = v2;
                    originalLocalPointerPosition.x -= ((v2.x - originalSizeDelta.x) / 2);
                    panelRectTransform.anchoredPosition = originalLocalPointerPosition;

                }
                else if (v2.x < originalSizeDelta.x)
                {
                    panelRectTransform.sizeDelta = v2;
                    originalLocalPointerPosition.x += ((originalSizeDelta.x-v2.x) / 2);
                    panelRectTransform.anchoredPosition = originalLocalPointerPosition;
                }
                else 
                {
                  //  Debug.Log("达到 最大值");
                }

                break;
            case UI_Edge.Right:
                originalLocalPointerPosition = panelRectTransform.anchoredPosition;
                originalSizeDelta = panelRectTransform.sizeDelta;
                if (v2.x > originalSizeDelta.x)
                {
                    panelRectTransform.sizeDelta = v2;
                    originalLocalPointerPosition.x += ((v2.x - originalSizeDelta.x) / 2);
                    panelRectTransform.anchoredPosition = originalLocalPointerPosition;

                }
                else if (v2.x < originalSizeDelta.x)
                {
                    panelRectTransform.sizeDelta = v2;
                    originalLocalPointerPosition.x -= ((originalSizeDelta.x - v2.x) / 2);
                    panelRectTransform.anchoredPosition = originalLocalPointerPosition;
                }
                else
                {
                    //Debug.Log("达到 最大值");
                }
                break;
            default:
                break;
        }
    }
}
