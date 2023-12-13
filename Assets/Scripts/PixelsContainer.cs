using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PixelsContainer : MonoBehaviour
{
    // This is a container for the pixels that are spawned in the game
    // container is a matrix of pixels
    // each pixel is a game object , we may create a pixel cell class later
    public static PixelsCell[,] pixelContainer;
    public PixelsCell pixelCellPrefab;
    public int width;
    public int height;

    public float cell_w = 0.1f;
    public float cell_h = 0.1f;
    public float cell_d = 0.1f;

    public Color selectedColor = Color.black;

    // Start is called before the first frame update
    void Start()
    {
        // initialize the pixel container
        // 设置像素容器 transform 位于父物体的位置

        CreateContainer();
    }

    void CreateContainer()
    {
        // 初始化cell的宽高的默认值
        Vector3 cellScale = new Vector3(cell_w, cell_h, cell_d);

        pixelContainer = new PixelsCell[width, height];
        // 偏移量
        float offset_w = width * cell_w / 2f;
        float offset_h = 1f; // height * cell_h / 2f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                PixelsCell pixelCell = Instantiate(pixelCellPrefab, transform);
                Transform cellTransform = pixelCell.GetComponent<Transform>();
                cellTransform.localScale = cellScale;
                // 获取cell的宽高 , 用于设置cell的位置和大小 ,y是高度 , x是宽度, z是深度
                float cellWidth = cellTransform.localScale.x;
                float cellHeight = cellTransform.localScale.y;
                float cellDepth = cellTransform.localScale.z;




                // 设置格子的位置和大小
                cellTransform.localPosition = new Vector3(x * cellWidth - offset_w , y * cellHeight + offset_h, 5);
                cellTransform.localScale = new Vector3(cellWidth, cellHeight, cellDepth);

                pixelContainer[x, y] = pixelCell;
            }
        }
    }

    void UpdateCursorColor(Color color)
    {
        Debug.Log("w"+width+"--h"+height);
        if (pixelContainer != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++) 
                {
                    pixelContainer[x, y].SetCurrentCursorColor(color);
                }
            }
        }
    }
    public void OnRedSelect()
    {
        selectedColor = Color.red;
        UpdateCursorColor(selectedColor);

    }
    public void OnGreenSelect()
    {
        selectedColor = Color.green;
        UpdateCursorColor(selectedColor);
    }
    public void OnBlueSelect()
    {
        selectedColor = Color.blue;
        UpdateCursorColor(selectedColor);
    }
    public void DrawCommand(string command, int x, int y, int r, int g, int b)
    {
        if (command == "/d")
        {
            pixelContainer[x, y].SetColor(new Color(r, g, b));
        }
    }

    public void DrawPreImage(int sx, int sy, Color[,] pixels)
    {
        int w = pixels.GetLength(0);
        int h = pixels.GetLength(1);
        int x = sx;
        int y = sy;
        for (int i = 0; i < w; i++)
        {
            x = sx + i;
            for (int j = 0; j < h; j++)
            {
                y = sy + j;
                pixelContainer[x, y].SetColor(pixels[i, j]);
            }
        }
    }

    // 把container当前的像素颜色矩阵，保存成为图片
    public void SaveImage(string filePath = "Assets/Images/painting.png")
    {
        // 创建一个Texture2D
        Texture2D texture = new Texture2D(width, height);
        // 遍历每个像素
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y <height; y++)
            {
                // 获取像素颜色
                Color pixelColor = pixelContainer[x, y].GetColor();
                // 设置Texture2D的像素颜色
                texture.SetPixel(x, y, pixelColor);
            }
        }
        // 应用设置
        texture.Apply();
        // 把Texture2D保存成为图片
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);
    }

    public void Clear()
    {
        if (pixelContainer != null)
        {
            foreach (PixelsCell pixelCell in pixelContainer)
            {
                pixelCell.SetColor(Color.white);
            }
        }
    }

    void Update()
    {
        // 如果按下 C 键，清空所有像素
        if (Input.GetKeyDown(KeyCode.C))
        {
            Clear();
        }
        // 如果按下 S 键，保存图片
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveImage();
        }
    }
}
