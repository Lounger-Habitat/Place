using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PixelsCell : MonoBehaviour
{
    // 需要实时获取当前颜色
    private Color currentColor = Color.black;
    private Color defaultColor = Color.white;
    private Color lastColor = Color.white;
    private Renderer cellRenderer;

    void Start()
    {
        // 获取网格的Renderer组件
        cellRenderer = GetComponent<Renderer>();
        cellRenderer.material.color = defaultColor;
    }

    void OnMouseOver()
    {
        // 鼠标悬停时改变颜色
        cellRenderer.material.color = currentColor;
    }

    void OnMouseExit()
    {
        // 鼠标离开时恢复颜色
        cellRenderer.material.color = lastColor;
    }

    void OnMouseDown()
    {
        
        cellRenderer.material.color = currentColor;
        lastColor = cellRenderer.material.color;
    }

    public void SetColor(Color color)
    {
        // 设置颜色
        currentColor = color;
    }

    public void DrawPixel(Color color){
        cellRenderer.material.color = color;
        lastColor = cellRenderer.material.color;
    }
}
