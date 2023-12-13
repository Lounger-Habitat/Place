using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class DefaultController : MonoBehaviour
{
    public static DefaultController Instance { get; private set; }

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
    public string directoryPath = "Assets/Images"; // 替换成你的目录路径

    public List<Texture2D> loadedTextures = new List<Texture2D>();

    PixelsContainer container;


    // Start is called before the first frame update
    void Start()
    {
        // 获取此脚本所在的Object上的PixelController脚本
        // 例如，如果你想在这里调用另一个脚本上的方法，可以这样做：
        container = GetComponent<PixelsContainer>();

        LoadResources();
    }

    void LoadResources()
    {
        // 检查目录是否存在
        if (Directory.Exists(directoryPath))
        {
            // 获取目录中的所有文件
            string[] files = Directory.GetFiles(directoryPath);

            foreach (string filePath in files)
            {
                // 检查文件是否是图片
                if (IsImageFile(filePath))
                {
                    // 加载图片资源并添加到List
                    Texture2D texture = LoadTexture(filePath);
                    if (texture != null)
                    {
                        loadedTextures.Add(texture);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Directory not found: " + directoryPath);
        }
    }

    bool IsImageFile(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        return extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif";
    }

    Texture2D LoadTexture(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2); // 创建一个临时Texture2D，稍后会被替换为实际的图像数据
        if (texture.LoadImage(fileData)) // 加载图像数据
        {
            return texture;
        }
        else
        {
            Debug.LogError("Failed to load texture: " + filePath);
            return null;
        }
    }

    public Color[,] ProcessImage(Texture2D image)
    {
        // 获取原始图片大小
        int originalWidth = image.width;
        int originalHeight = image.height;

        // Debug.Log("Original Size: " + originalWidth + " x " + originalHeight);

        // 图片等比例调整缩放,不超出画布大小
 
        (int targetWidth , int targetHeight) = ScaleImageToFitCanvas(originalWidth, originalHeight, 200, 200);

        Texture2D resizedTexture = ResizeTexture(image, targetWidth, targetHeight);

        // 遍历每个像素并获取颜色信息
        Color[] pixels = resizedTexture.GetPixels();
        // 根据宽高转成二维数组,把peixels里的数据放到二维数组里
        Color[,] pixels2D = new Color[targetWidth, targetHeight];
        for (int i = 0; i < pixels.Length; i++)
        {
            int x = i % targetWidth;
            int y = i / targetWidth;
            pixels2D[x, y] = pixels[i];
        }



        // foreach (Color pixelColor in pixels)
        // {
        //     // 处理每个像素的颜色信息
        //     Debug.Log("Pixel Color: " + pixelColor);
        // }

        return pixels2D;

        // 在这里可以将调整大小后的纹理保存为新的图片文件，如果需要的话
    }

    Texture2D ResizeTexture(Texture2D originalTexture, int targetWidth, int targetHeight)
    {
        RenderTexture rt = new RenderTexture(targetWidth, targetHeight, 0);
        Graphics.Blit(originalTexture, rt);
        RenderTexture.active = rt;

        Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight);
        resizedTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        resizedTexture.Apply();

        RenderTexture.active = null;
        Destroy(rt);

        return resizedTexture;
    }

    (int newWidth, int newHeight) ScaleImageToFitCanvas(int originalWidth, int originalHeight, int canvasWidth, int canvasHeight)
    {
        // 计算宽度和高度的缩放比例
        float widthScale = (float)canvasWidth /4 / originalWidth;
        float heightScale = (float)canvasHeight /4 / originalHeight;

        // 使用较小的缩放比例，以保持等比例缩放
        float scale = Mathf.Min(widthScale, heightScale);

        // 计算新的宽度和高度
        int newWidth = Mathf.RoundToInt(originalWidth * scale);
        int newHeight = Mathf.RoundToInt(originalHeight * scale);

        return (newWidth, newHeight);
    }

    // Update is called once per frame
    void Update()
    {
        // 按下 d 键，处理图片
        if (Input.GetKeyDown(KeyCode.D))
        {

            int x=10;
            int y=10;
            // 如果List中有图片，可以在这里获取第一张图片
            if (loadedTextures == null && loadedTextures.Count == 0)
            {
                Debug.Log("没图片！");
            }
            Texture2D originalTexture = loadedTextures[0];
            Color[,] imagePixel = ProcessImage(originalTexture);
            container.DrawPreImage(x,y,imagePixel);
        }
    }
}