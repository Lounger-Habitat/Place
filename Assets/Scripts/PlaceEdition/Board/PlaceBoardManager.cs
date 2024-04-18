using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Assets.GifAssets.PowerGif;
using System.Collections;

public class PlaceBoardManager : MonoBehaviour
{
    public Camera mainCamera;
    public Renderer canvasRenderer;

    public string mode = "3D"; // or 2D

    // for 2d canvas use
    public RawImage canvasImage;

    private Texture2D texture;
    private Color drawColor = Color.white; // 可以改为您想要的颜色

    public int height = 500;
    public int width = 500;
    public Texture2D defaultTexture;
    public int recorderTime = 60;
    public int[] pixelsInfos;

    // 画作唯一id
    public static int UniqueId = 0;

    public static PlaceBoardManager Instance { get; private set; }

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

        // DiffusionManager.Instance.OnImageLoaded += OnImageLoaded;
        // 假设平面使用的是材质的第一个贴图
        // 生成一个新的贴图
        Texture2D myTexture = GenerateTexture(height, width, new Color(64/255f,64/255f,64/255f)); // 可以根据需要调整尺寸和颜色

        if (mode == "2D")
        {
            if (canvasImage == null)
            {
                canvasImage = GetComponent<RawImage>();
            }
            // Debug.Log(uiimage.texture.name);
            // 将新贴图应用到某个对象的材质上
            // 例如，将其应用到当前游戏对象的 Renderer 上
            // Renderer renderer = GetComponent<Renderer>();

            // if (canvasMaterial != null)
            // {
            //     canvasMaterial.mainTexture = myTexture;
            // }
            // texture = (Texture2D)canvasMaterial.mainTexture;

            canvasImage.texture = myTexture;
            texture = canvasImage.texture as Texture2D;
        }
        else if (mode == "3D")
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            if (canvasRenderer == null)
            {
                canvasRenderer = GetComponent<Renderer>();
            }
            canvasRenderer.material.mainTexture = myTexture;
            texture = (Texture2D)canvasRenderer.material.mainTexture;
        }
        // 将新贴图应用到某个对象的材质上
        // 例如，将其应用到当前游戏对象的 Renderer 上
        // Renderer renderer = GetComponent<Renderer>();

        // MarkEdges(texture);
        defaultTexture = texture;

        // LoadResources();
        UniqueId = GenerateUniqueId();
        
    }

    void Update()
    {
        if (mode == "2D")
        {
            // if (Input.GetMouseButtonDown(0))
            // {
            //     Vector2 mousePosition = Input.mousePosition;
            //     RectTransform rectTransform = canvasImage.rectTransform;
            //     Vector2 localPoint;
            //     if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, null, out localPoint))
            //     {
            //         // 在这里处理点击的本地坐标
            //         // Vector2 pixelUV = hit.textureCoord;
            //         // pixelUV.x *= texture.width;
            //         // pixelUV.y *= texture.height;
            //         Vector2 uv = new Vector2((localPoint.x + rectTransform.rect.width / 2) / rectTransform.rect.width,
            //                                 (localPoint.y + rectTransform.rect.height / 2) / rectTransform.rect.height);
            //         uv.x *= texture.width;
            //         uv.y *= texture.height;
            //         // Vector2Int texCoord = new Vector2Int((int)(localPoint.x + rectTransform.rect.width / 2), (int)(localPoint.y + rectTransform.rect.height / 2));
            //         // Debug.Log(uv);

            //         UpdateTexture((int)uv.x, (int)uv.y);
            //     }
            // }
        }
        else if (mode == "3D")
        {
            // if (Input.GetMouseButtonDown(0))
            // {
            //     RaycastHit hit;
            //     Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            //     if (Physics.Raycast(ray, out hit))
            //     {
            //         Vector2 pixelUV = hit.textureCoord;
            //         pixelUV.x *= texture.width;
            //         pixelUV.y *= texture.height;

            //         UpdateTexture((int)pixelUV.x, (int)pixelUV.y);
            //     }
            // }
        }
        // 按下 d 键
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                // 读取 /Asssets/Images/SourceTexture.png
                // Texture2D sourceTexture = LoadTexture("Assets/Images/dog.jpg");
                // Debug.Log(sourceTexture.name);

                if (TestManager.Instance.loadedTextures == null && TestManager.Instance.loadedTextures.Count == 0)
                {
                    Debug.Log("没图片！");
                }
                Texture2D originalTexture = TestManager.Instance.loadedTextures[TestManager.Instance.index];
                bool ist = CheckForTransparency(texture);
                Debug.Log(ist);
                Debug.Log(originalTexture.name);
                PasteTexture(originalTexture, 100, 100);
            }

            // 如果按下 C 键，清空所有像素
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Reset();
            }
            // 如果按下 . 键，保存图片
            if (Input.GetKeyDown(KeyCode.J))
            {
                SaveImage();
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                GenGif($"Assets/Images/test");
            }
        }
    }

    public static int GenerateUniqueId()
    {
        return UniqueId++;
    }

    List<Texture2D> LoadResources(string directoryPath)
    {
        List<Texture2D> loadedTextures = new List<Texture2D>();
        // 检查目录是否存在
        if (Directory.Exists(directoryPath))
        {
            // 获取目录中的所有文件
            string[] files = Directory.GetFiles(directoryPath);

            foreach (string filePath in files)
            {
                // 检查文件是否是图片
                Debug.Log("filePath : " + filePath);
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
        return loadedTextures;
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

    void UpdateTexture(int x, int y)
    {
        texture.SetPixel(x, y, drawColor);
        texture.Apply(); // 应用更改到贴图
    }

    // 生成新贴图的函数
    public Texture2D GenerateTexture(int width, int height, Color fillColor)
    {
        // 创建一个新的空白贴图
        Texture2D newTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        // 填充贴图
        Color[] fillPixels = new Color[width * height];
        pixelsInfos = new int[width * height];
        for (int i = 0; i < fillPixels.Length; i++)
        {
            fillPixels[i] = fillColor;
            pixelsInfos[i] = 0;
        }
        newTexture.SetPixels(fillPixels);
        newTexture.Apply();

        return newTexture;
    }

    void PasteTexture(Texture2D source, int posX, int posY, int limit = 10)
    {
        // 新Texture2D
        Texture2D scaleTexture;

        if (source.width > texture.width / limit || source.height > texture.height / limit)
        {
            // 等比例缩放源贴图到目标贴图的1/10大小
            int maxWidth = texture.width / limit;
            int maxHeight = texture.height / limit;
            scaleTexture = ScaleTextureProportionally(source, maxWidth, maxHeight);
            Debug.Log(scaleTexture);
        }
        else
        {
            scaleTexture = source;
        }
        // 检查贴图是否超出画布范围
        if (posX + scaleTexture.width > texture.width || posY + scaleTexture.height > texture.height)
        {
            Debug.Log("The source texture is too large");
        }

        Color[] sourcePixels = scaleTexture.GetPixels();
        Color[] targetPixels = texture.GetPixels(posX, posY, scaleTexture.width, scaleTexture.height);

        for (int y = 0; y < scaleTexture.height; y++)
        {
            for (int x = 0; x < scaleTexture.width; x++)
            {
                int targetIndex = y * scaleTexture.width + x;
                Color sourcePixel = sourcePixels[targetIndex];

                if (sourcePixel.a > 0)  // 检查透明度
                {
                    targetPixels[targetIndex] = sourcePixel;
                }
            }
        }

        texture.SetPixels(posX, posY, scaleTexture.width, scaleTexture.height, targetPixels);
        texture.Apply(); // 应用更改到目标贴图
    }


    public Texture2D ScaleTextureProportionally(Texture2D source, int maxWidth, int maxHeight)
    {
        if (source == null)
        {
            Debug.LogError("Source texture is null.");
            return null;
        }

        if (!source.isReadable)
        {
            Debug.LogError("Source texture is not readable. Please enable 'Read/Write Enabled' in import settings.");
            return null;
        }
        float sourceWidth = source.width;
        float sourceHeight = source.height;
        float targetWidth = maxWidth;
        float targetHeight = maxHeight;
        float widthRatio = targetWidth / sourceWidth;
        float heightRatio = targetHeight / sourceHeight;
        // float ratio = Mathf.Min(widthRatio, heightRatio);

        int newWidth = Mathf.RoundToInt(sourceWidth * widthRatio);
        int newHeight = Mathf.RoundToInt(sourceHeight * heightRatio);

        Texture2D newTexture = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, false);

        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                float xFrac = x / (float)newWidth;
                float yFrac = y / (float)newHeight;
                Color color = source.GetPixelBilinear(xFrac, yFrac);
                newTexture.SetPixel(x, y, color);
            }
        }

        newTexture.Apply();
        return newTexture;
    }

    void MarkEdges(Texture2D texture)
    {
        if (texture == null)
        {
            Debug.LogError("Texture is null.");
            return;
        }

        if (!texture.isReadable)
        {
            Debug.LogError("Texture is not readable. Please enable 'Read/Write Enabled' in import settings.");
            return;
        }

        int width = texture.width;
        int height = texture.height;

        // 设置原点为紫色
        texture.SetPixel(0, 0, Color.magenta);
        // 设置右上角为青色
        texture.SetPixel(width - 1, height - 1, Color.cyan);
        // 设置左上角为青色
        texture.SetPixel(0, height - 1, Color.cyan);
        // 设置右下角为青色
        texture.SetPixel(width - 1, 0, Color.cyan);

        // 修改第一行和最后一行
        for (int x = 1; x < width - 1; x++)
        {
            texture.SetPixel(x, 0, Color.green);       // 第一行
            texture.SetPixel(x, height - 1, Color.green); // 最后一行
        }

        // 修改第一列和最后一列
        for (int y = 1; y < height - 1; y++)
        {
            texture.SetPixel(0, y, Color.green);        // 第一列
            texture.SetPixel(width - 1, y, Color.green);   // 最后一列
        }

        texture.Apply();
    }

    bool CheckForTransparency(Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();
        foreach (Color pixel in pixels)
        {
            if (pixel.a < 1.0f)
            {
                // Found a transparent pixel
                return true;
            }
        }

        // No transparent pixels found
        return false;
    }

    public void SaveImage()
    {
        byte[] bytes = texture.EncodeToPNG();
        // 检测文件夹是否存在
        if (!Directory.Exists($"Assets/Images/{UniqueId}"))
        {
            Directory.CreateDirectory($"Assets/Images/{UniqueId}");
        }
        string path = $"Assets/Images/{UniqueId}/save_{DateTime.Now.ToString("yyyyMMddHHmmss")}.png";
        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log("Saved Image to: " + path);
    }

    public void GenGif(string gifPath) {
        List<Texture2D> f =  LoadResources(gifPath);
        var frames = f.Select(f => new GifFrame(f, 0.1f)).ToList();
	    var gif = new Gif(frames);
		var bytes = gif.Encode();
        var path = Path.Combine(gifPath, "test.gif");
		if (path == "") return;
		File.WriteAllBytes(path, bytes);
		Debug.Log($"Saved to: {path}");
    }

    public void Reset()
    {
        texture.SetPixels(defaultTexture.GetPixels());
        Array.Clear(pixelsInfos, 0, pixelsInfos.Length);
        texture.Apply();
    }


    public void DrawCommand(int x, int y, int r, int g, int b, int camp)
    {
        MarkPixels(x, y, camp);
        DrawPixels(x, y, r, g, b);
    }
    public void DrawPixels(int x, int y , int r , int g , int b )
    {
        Color aimColor = new Color((float)r/255f, (float)g/255f, (float)b/255f);
        if (texture != null && x >= 0 && x < texture.width && y >= 0 && y < texture.height)
        {
            texture.SetPixel(x, y, aimColor);
            texture.Apply(); // 应用更改到贴图
        }
    }

    public List<(int,int)> GetLinePoints(int x, int y, int ex, int ey)
    {
        return ComputeDrawLine(x: x, y: y, ex: ex, ey: ey, isDraw: false);
    }
    public int GetLineCount(int x, int y, int ex, int ey)
    {
        return DrawLine(x: x, y: y, ex: ex, ey: ey, isDraw: false);
    }

    public void LineCommand(int x, int y, int ex, int ey, int r, int g, int b, int camp = 0)
    {
        GetLinePoints(x, y, ex, ey).ForEach(p => {
            DrawCommand(p.Item1, p.Item2, r, g, b, camp);
        });
        // DrawLine(x, y, ex, ey, r, g, b, camp);
    }
    private List<(int,int)> ComputeDrawLine(int x, int y , int ex , int ey , int r = 0, int g = 0, int b = 0, int camp = 0, bool isDraw = true) {
        List<(int,int)> points = new List<(int,int)>();
        // 使用 Bresenham 算法来计算这两点之间的像素点
        int dx = Math.Abs(ex - x);
        int dy = Math.Abs(ey - y);
        int sx = (x < ex) ? 1 : -1;
        int sy = (y < ey) ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            // if (isDraw)
            // {
            //     DrawCommand(x, y, r, g, b, camp); // 绘制像素点
            // }
            // else
            // {
            //     pixelsCount += 1;
            // }

            points.Add((x,y));


            if ((x == ex) && (y == ey))
                break;

            int e2 = 2 * err;

            if (e2 > -dy)
            {
                err = err - dy;
                x = x + sx;
            }

            if (e2 < dx)
            {
                err = err + dx;
                y = y + sy;
            }
        }

        return points;
    }
    private int DrawLine(int x, int y , int ex , int ey , int r = 0, int g = 0, int b = 0, int camp = 0, bool isDraw = true)
    {
        // 使用 Bresenham 算法来计算这两点之间的像素点
        int dx = Math.Abs(ex - x);
        int dy = Math.Abs(ey - y);
        int sx = (x < ex) ? 1 : -1;
        int sy = (y < ey) ? 1 : -1;
        int err = dx - dy;

        int pixelsCount = 0;

        while (true)
        {
            if (isDraw)
            {
                DrawCommand(x, y, r, g, b, camp); // 绘制像素点
            }
            else
            {
                pixelsCount += 1;
            }


            if ((x == ex) && (y == ey))
                break;

            int e2 = 2 * err;

            if (e2 > -dy)
            {
                err = err - dy;
                x = x + sx;
            }

            if (e2 < dx)
            {
                err = err + dx;
                y = y + sy;
            }
        }

        return pixelsCount;

    }
    public void PaintCommand(string command, int x, int y, int dx, int dy, int r, int g, int b, int camp = 0)
    {
        DrawPaint(command, x, y, dx, dy, r, g, b, camp);
    }

    void DrawPaint(string command = "/p", int x = 0, int y = 0, int dx = 0, int dy = 0, int r = 0, int g = 0, int b = 0, int camp = 0)
    {
        Color[] colors = texture.GetPixels(x, y, dx, dy);
        for (int i = x; i < x + dx; i++)
        {
            for (int j = y; j < y + dy; j++)
            {
                // DrawCommand(command,i, j , r, g, b);
                colors[(j - y) * dx + (i - x)] = new Color(r, g, b);
                MarkPixels(i, j, camp);
            }
        }
        texture.SetPixels(x, y, dx, dy, colors);
        texture.Apply(); // 应用更改到目标贴图
    }


    public int GetPaintCount(int width, int height)
    {
        return width * height;
    }


    public void GenerateImage(int sx, int sy, string prompt)
    {
        // DiffusionManager.Instance.GenerateImage(sx, sy, prompt);
        Debug.Log("正在生成,wait ...");
    }

    public void OnImageLoaded(Texture2D texture, string finishReason, long seed, int sx, int sy)
    {
        Debug.Log("生成图片完成 : " + finishReason);
        Debug.Log("seed : " + seed);
        PasteTexture(texture, sx, sy, 5);
        // Color[,] p = DefaultController.Instance.ProcessImage(texture);
        // DrawPreImage(sx, sy, p);
    }


    public void MarkPixels(int x, int y, int camp=0)
    {
        // 对x,y 处理ß
        // 记录
        int index = x + (y * width);
        pixelsInfos[index] = camp;

    }

    public bool CheckIns(Instruction ins) {
        if (ins.mode == "/draw" || ins.mode == "/d")
        {
            return ins.x < width && ins.y < height && ins.x >= 0 && ins.y >= 0;
        }
        else if (ins.mode == "/line" || ins.mode == "/l")
        {
            return ins.x < width && ins.y < height && ins.ex < width && ins.ey < height && ins.x >= 0 && ins.y >= 0 && ins.ex >= 0 && ins.ey >= 0;
        }
        else if (ins.mode == "/paint" || ins.mode == "/p")
        {
            return ins.x < width && ins.y < height && ins.dx < width && ins.dy < height && ins.x >= 0 && ins.y >= 0 && ins.dx >= 0 && ins.dy >= 0;
        }
        return false;
    }


}
