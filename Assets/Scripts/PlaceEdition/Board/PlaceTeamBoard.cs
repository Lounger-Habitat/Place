using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgcodecsModule;
using OpenCVForUnity.UnityUtils;
using Assets.GifAssets.PowerGif;

public class PlaceTeamBoard : MonoBehaviour
{
    public bool darkMode = false;

    // for 2d canvas use ， 
    // 背景图片
    public RawImage bgImage;
    // 轮廓图片
    public RawImage contoursImage;
    // 实际图片
    public RawImage realImage;

    private Texture2D texture;
    private Color drawColor = Color.white; // 可以改为您想要的颜色

    public int height = 800;
    public int width = 600;
    // public Texture2D defaultTexture;
    public int recorderTime = 6;
    // 像素信息 ， 0 为未涂色，>0 为涂色, 数字代表队伍
    // public int[] pixelsInfos;
    // 像素用户信息， 0 为未涂色，>0 为涂色, 数字代表用户
    public int[] pixelsUserInfos;
    public int[] randomIndexs;
    Color[] currentPixels;
    int currentIndex = 0;

    public string gifPath = "";

    string test = "test";

    public string savePath = "";

    Texture2D templateTexture;
    public string teamID = "";
    public string UniqueTime;
#if UNITY_EDITOR
    string competitionsDir = "Assets/Images/Competitions/";
#else
    string competitionsDir = Application.streamingAssetsPath + "/Competitions/";
#endif
    void Start()
    {
        // 初始化画布 背景颜色，默认白色
        Color bgColor = darkMode ? new Color(64 / 255f, 64 / 255f, 64 / 255f) : Color.white;

        // 生成一张贴图
        Texture2D bgTexture = GenerateTexture(width, height, bgColor); // 可以根据需要调整尺寸和颜色
        texture = GenerateTexture(width, height, Color.clear);


        if (bgImage == null)
        {
            bgImage = GetComponent<RawImage>();
        }

        bgImage.texture = bgTexture;
        realImage.texture = texture;

        // 随机选取一张贴图
        string imagePath = LoadRandomImage();
        // ImageProcessor processor = new ImageProcessor();
        templateTexture = LoadTexture(imagePath);
        templateTexture = ScaleTextureFixed(templateTexture, width, height);
        currentPixels = templateTexture.GetPixels();
        SetRandomIndex(currentPixels.Length);
        Debug.Log("currentPixels.Length : " + currentPixels.Length);
        Texture2D contex = MakeContours(imagePath);
        contex = ScaleTextureFixed(contex, width, height);
        contoursImage.texture = contex;
        //UniqueTime =string.IsNullOrEmpty(UniqueTime)? GenerateUniqueTime():UniqueTime;
    }

    public void SetRandomIndex(int len)
    {
        // 初始化1到100的数组
        randomIndexs = new int[len];
        for (int i = 0; i < randomIndexs.Length; i++)
        {
            randomIndexs[i] = i + 1;
        }

        // 使用Random的静态方法生成随机数
        System.Random random = new System.Random();
        for (int i = randomIndexs.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1); // 随机选择一个索引
            // 交换i和j位置的元素
            var temp = randomIndexs[i];
            randomIndexs[i] = randomIndexs[j];
            randomIndexs[j] = temp;
        }
    }


    Texture2D MakeContours(string imagePath)
    {
        // check image path file is Exists and is image
        if (!File.Exists(imagePath))
        {
            Debug.LogError("File not found: " + imagePath);
            return null;
        }

        // Load image
        Mat src = Imgcodecs.imread(imagePath, Imgcodecs.IMREAD_UNCHANGED);
        Mat gray = new Mat();
        Imgproc.cvtColor(src, gray, Imgproc.COLOR_BGR2GRAY);

        // Canny
        Mat edges = new Mat();
        Imgproc.Canny(gray, edges, 50, 150, 3);

        // Find contours
        List<MatOfPoint> contours = new List<MatOfPoint>();
        Mat hierarchy = new Mat();
        Imgproc.findContours(edges, contours, hierarchy, Imgproc.RETR_TREE, Imgproc.CHAIN_APPROX_SIMPLE);


        Mat drawing = Mat.zeros(src.size(), CvType.CV_8UC4);
        Debug.Log("boundingRect.size() : " + src.size());

        Imgproc.drawContours(drawing, contours, -1, new Scalar(64, 64, 64, 255), 2);


        Texture2D contoursTexture = new Texture2D(drawing.cols(), drawing.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(drawing, contoursTexture);
        return contoursTexture;
    }

    public string LoadRandomImage(string dirPath = "default")
    {
        string dir = competitionsDir + dirPath;
        Debug.Log("dir : " + dir);
        // 加载当前目录中加载所有图片路径,并打乱顺序
        string[] files = Directory.GetFiles(dir);
        System.Random random = new System.Random(); // 随机数生成器
        files = files.OrderBy(x => random.Next()).ToArray(); // 打乱顺序

        foreach (string filePath in files)
        {
            // 检查文件是否是图片
            Debug.Log("filePath : " + filePath);
            if (IsImageFile(filePath))
            {
                // 加载图片资源并添加到List
                return filePath;
            }
        }
        return "";
    }

    void Update()
    {
    }

    public void SetWidthAndHeight(int width, int height)
    {
        this.width = width;
        this.height = height;
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

    // 生成新贴图的函数
    public Texture2D GenerateTexture(int width, int height, Color fillColor)
    {
        // 创建一个新的空白贴图
        Texture2D newTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        newTexture.filterMode = FilterMode.Point;

        // 填充贴图
        Color[] fillPixels = new Color[width * height];
        // pixelsInfos = new int[width * height];
        pixelsUserInfos = new int[width * height];
        for (int i = 0; i < fillPixels.Length; i++)
        {
            fillPixels[i] = fillColor;
            // pixelsInfos[i] = 0;
            pixelsUserInfos[i] = 0;
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

    public Texture2D ScaleTextureFixed(Texture2D source, int maxWidth, int maxHeight)
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
        float sourceWidth = source.width; // 原 宽
        float sourceHeight = source.height; // 原 高
        float targetWidth = maxWidth;       // 限制最大 宽
        float targetHeight = maxHeight;     // 限制最大 高
        float widthRatio = targetWidth / sourceWidth;   // 宽比例
        float heightRatio = targetHeight / sourceHeight; // 高比例
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
        float sourceWidth = source.width; // 原 宽
        float sourceHeight = source.height; // 原 高
        float targetWidth = maxWidth;       // 限制最大 宽
        float targetHeight = maxHeight;     // 限制最大 高
        float widthRatio = targetWidth / sourceWidth;   // 宽比例
        float heightRatio = targetHeight / sourceHeight; // 高比例
        float ratio = Mathf.Min(widthRatio, heightRatio);

        int newWidth = Mathf.RoundToInt(sourceWidth * ratio);
        int newHeight = Mathf.RoundToInt(sourceHeight * ratio);

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

    public void SaveImage(bool lastone = false)
    {

        byte[] bytes = (realImage.texture as Texture2D).EncodeToPNG();
        // 检测文件夹是否存在
#if UNITY_EDITOR
        savePath = $"Assets/Images/Log/{UniqueTime}/{teamID}";
#else
        savePath = Application.persistentDataPath;
        savePath = Path.Combine(savePath, $"Log/{UniqueTime}/{teamID}");
#endif
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        string path = $"{savePath}/save_{DateTime.Now.ToString("yyyyMMddHHmmss")}.png";
        System.IO.File.WriteAllBytes(path, bytes);
        //Debug.Log("Saved Image to: " + path);
        // if (lastone)
        // {
        //     // 创建 Sprite
        //     Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        // }
    }

    public void GenGif()
    {
        // string gifPath = $"Assets/Images/{UniqueTime}";
#if UNITY_EDITOR
        string gifPath = $"Assets/Images/Log/{test}";
#else
        string gifPath = Application.persistentDataPath;
        gifPath = Path.Combine(gifPath, $"Log/{test}");
#endif
        List<Texture2D> f = LoadResources(gifPath);
        f = Select20(f.ToArray());
        var frames = f.Select(f => new GifFrame(f, 0.5f)).ToList();
        var gif = new Gif(frames);
        var bytes = gif.Encode();
        var path = Path.Combine(gifPath, "test.gif");
        if (path == "") return;
        File.WriteAllBytes(path, bytes);
        Debug.Log($"Saved to: {path}");
    }

    private List<Texture2D> Select20(Texture2D[] fa)
    {
        int len = fa.Length;
        List<Texture2D> res = new List<Texture2D>();
        // 始终选择第一个元素
        if (len > 20)
        {
            res.Add(fa[0]);
            // 选择最后一个元素


            // 计算间隔
            int interval = (len - 2) / 18; // 18是因为我们要选20个，已经选了2个

            // 从第二个元素之后开始选择，直到倒数第二个元素之前
            for (int i = 2; i < len - 1; i += interval)
            {
                res.Add(fa[i]);
            }
            res.Add(fa[len - 1]);

            // 断言 res 一个20个
            //Assert.IsTrue(res.Count == 20);
        }
        else
        {
            // 如果列表元素少于或等于20个，则选择所有元素
            res = fa.ToList();
        }
        return res;
    }

    public void Reset()
    {
        // texture.SetPixels(defaultTexture.GetPixels());
        // // Array.Clear(pixelsInfos, 0, pixelsInfos.Length);
        // Array.Clear(pixelsUserInfos, 0, pixelsUserInfos.Length);
        // texture.Apply();
    }


    public void DrawCommand(int x, int y, int r, int g, int b, int camp, int id = 0)
    {
        MarkPixels(x, y, camp, id);
        DrawPixels(x, y, r, g, b);
    }
    public void DrawPixels(int x, int y, int r, int g, int b, int a = 255)
    {
        Color32 aimColor = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
        if (texture != null && x >= 0 && x < texture.width && y >= 0 && y < texture.height)
        {
            texture.SetPixel(x, y, aimColor);
            texture.Apply(); // 应用更改到贴图
        }
    }

    public List<(int, int)> GetLinePoints(int x, int y, int ex, int ey)
    {
        return ComputeDrawLine(x: x, y: y, ex: ex, ey: ey, isDraw: false);
    }
    public List<(int, int)> GetSquarePoints(int x, int y, int dx, int dy)
    {
        return ComputeDrawSqure(x: x, y: y, dx: dx, dy: dy);
    }

    public List<(int, int)> GetRectPoints(int x, int y, int dx, int dy)
    {
        return ComputeDrawRect(x: x, y: y, dx: dx, dy: dy);
    }
    public List<(int, int, int, int)> GetRectLines(int x, int y, int dx, int dy)
    {
        return ComputeLineRect(x: x, y: y, dx: dx, dy: dy);
    }

    private List<(int, int, int, int)> ComputeLineRect(int x, int y, int dx, int dy)
    {
        int ex = x + dx;
        int ey = y + dy;
        List<(int, int, int, int)> lines = new List<(int, int, int, int)>();
        lines.Add((x, ey, x, y)); // 左
        lines.Add((x, ey, ex, ey)); // 上
        lines.Add((ex, ey, ex, y)); // 右
        lines.Add((x, y, ex, y)); // 下
        return lines;
    }

    private List<(int, int)> ComputeDrawRect(int x, int y, int dx, int dy)
    {
        List<(int, int)> points = new List<(int, int)>();
        for (int i = x; i < x + dx; i++)
        {
            points.Add((i, y));
            points.Add((i, y + dy));
        }
        for (int j = y; j < y + dy; j++)
        {
            points.Add((x, j));
            points.Add((x + dx, j));
        }
        return points;
    }

    private List<(int, int)> ComputeDrawSqure(int x, int y, int dx, int dy)
    {
        List<(int, int)> points = new List<(int, int)>();
        for (int i = x; i < x + dx; i++)
        {
            for (int j = y; j < y + dy; j++)
            {
                points.Add((i, j));
            }
        }
        return points;
    }

    public List<(int, int)> GetCirclePoints(int x, int y, int r)
    {
        // 合法检测 TODO
        return ComputeDrawCircle(ox: x, oy: y, r: r);
    }

    public List<(int, int)> GetFillPoints(int x, int y, int uid)
    {
        // 合法检测 TODO
        return ComputeFillArea(x: x, y: y, uid: uid);
    }

    private List<(int, int)> ComputeFillArea(int x, int y, int uid)
    {
        throw new NotImplementedException();
    }

    private List<(int, int)> ComputeDrawCircle(int ox, int oy, int r)
    {

        List<(int, int)> points = new List<(int, int)>();
        int x, y, p; // x, y为圆上的动态坐标点，p为判别式
        x = 0;      // 初始化x坐标
        y = r;      // 初始化y坐标为半径
        p = 1 - r;  // 初始化判别式p，这里的1/4被省略，因为计算机中1/4的值可以忽略不计

        Draw8Points(ox, oy, x, y, points); // 绘制初始点

        while (x <= y) // 当x小于或等于y时，继续循环
        {
            x++; // x坐标向右移动

            if (p < 0) // 如果判别式p小于0，说明当前点在圆上方
            {
                p += 2 * x + 3; // 更新判别式p
            }
            else // 如果判别式p大于或等于0，说明当前点在圆下方
            {
                p += 2 * (x - y) + 5; // 更新判别式p
                y--; // y坐标向下移动
            }

            Draw8Points(ox, oy, x, y, points); // 绘制新的点
        }

        return points;
    }

    public void Draw8Points(int xo, int yo, int x, int y, List<(int, int)> points)
    {
        // 同时画八个象限的点
        points.Add((xo + x, yo + y)); // 第1象限
        points.Add((xo + y, yo + x)); // 第2象限
        points.Add((xo - y, yo + x)); // 第3象限
        points.Add((xo - x, yo + y)); // 第4象限
        points.Add((xo - x, yo - y)); // 第5象限
        points.Add((xo - y, yo - x)); // 第6象限
        points.Add((xo + x, yo - y)); // 第7象限
        points.Add((xo + y, yo - x)); // 第8象限
    }

    public int GetLineCount(int x, int y, int ex, int ey)
    {
        return DrawLine(x: x, y: y, ex: ex, ey: ey, isDraw: false);
    }

    public void LineCommand(int x, int y, int ex, int ey, int r, int g, int b, int camp = 0, int id = 0)
    {
        GetLinePoints(x, y, ex, ey).ForEach(p =>
        {
            DrawCommand(p.Item1, p.Item2, r, g, b, camp, id);
        });
        // DrawLine(x, y, ex, ey, r, g, b, camp);
    }
    private List<(int, int)> ComputeDrawLine(int x, int y, int ex, int ey, int r = 0, int g = 0, int b = 0, int camp = 0, bool isDraw = true)
    {
        List<(int, int)> points = new List<(int, int)>();
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

            points.Add((x, y));


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
    private int DrawLine(int x, int y, int ex, int ey, int r = 0, int g = 0, int b = 0, int camp = 0, bool isDraw = true)
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


    public void MarkPixels(int x, int y, int camp = 0, int id = 0)
    {
        // 对x,y 处理ß
        // 记录
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Debug.Log($"越界 : {x},{y}");
            return;
        }
        int index = x + (y * width);
        // pixelsInfos[index] = camp;
        pixelsUserInfos[index] = id;
        if (id!=0)
        {
            socre++;
        }

    }

    public int socre=0;

    public bool CheckIns(Instruction ins)
    {
        if (ins.mode == "/draw" || ins.mode == "/d")
        {
            return ins.x < width && ins.y < height && ins.x >= 0 && ins.y >= 0;
        }
        else if (ins.mode == "/line" || ins.mode == "/l")
        {
            return ins.x < width && ins.y < height && ins.ex < width && ins.ey < height && ins.x >= 0 && ins.y >= 0 && ins.ex >= 0 && ins.ey >= 0;
        }
        else if (ins.mode == "/paint" || ins.mode == "/p" || ins.mode == "/rect" || ins.mode == "/r")
        {
            return ins.x < width && ins.y < height && ins.dx < width - ins.x && ins.dy < height - ins.y && ins.x >= 0 && ins.y >= 0 && ins.dx >= 0 && ins.dy >= 0;
        }
        return false;
    }


    // ========= gif part =========
    // pro gif lib
    private ProGifTexturesToGIF tex2Gif = null;
    private List<Texture2D> tex2DList = null;
    public RawImage dpImage;
    public void Clear()
    {
        if (tex2Gif != null) tex2Gif.Clear();

        // if (dpImage != null && dpImage.sprite != null && dpImage.sprite.texture != null)
        // {
        //     // Texture2D.Destroy(dpImage.sprite.texture);
        //     dpImage.sprite = null;
        // }

        //Clear texture
        if (tex2DList != null)
        {
            foreach (Texture2D tex in tex2DList)
            {
                if (tex != null)
                {
                    Destroy(tex);
                }
            }
            tex2DList = null;
        }
    }
    public void ConvertTex2DToGIF(Action action = null)
    {

        OnTexture2GifOk = action;
        Clear();
        tex2Gif = ProGifTexturesToGIF.Instance; // 我还是怀疑这里？？,别分开了，不行就 mamanger 里 用一个，处理 这两个？或者你刚才说搞两个 是啥意思
        // int ad = &tex2Gif;
        // Debug.Log("address : " + ad);

        //Set file extensions for loading images
        tex2Gif.SetFileExtension(new List<string> { ".jpg", ".png" });
        //tex2Gif.SetFileExtension(new List<string>{".jpg"});

        // string loadImagePath = Application.streamingAssetnidosPath;
#if UNITY_EDITOR
        string loadImagePath = $"Assets/Images/Log/{UniqueTime}/{teamID}";
#else
        string loadImagePath = Application.persistentDataPath;
        loadImagePath = Path.Combine(loadImagePath, $"Log/{UniqueTime}/{teamID}");
#endif



        //Load images as texture2D list from target directory
        tex2DList = tex2Gif.LoadImages(loadImagePath);
        // tex2DList = tex2Gif.LoadImages(loadImagePath2); 
        tex2Gif.LoadImagesFromResourcesFolder(); // 这是干嘛的？

        if (tex2DList != null && tex2DList.Count > 0)
        {
            //Save the provided texture2Ds to a GIF file with settings

            // tex2Gif.m_Rotation = m_Rotation; // no rotation

            //Set auto detect transparent pixels for imported images
            tex2Gif.SetTransparent(true);

            //tex2Gif.m_MaxNumberOfThreads = 6;
            Debug.Log("tex2DList.Count : " + tex2DList.Count);
            tex2Gif.Save(tex2DList, width, height, 10, 0, 1, OnFileSaved, OnFileSaveProgress, ProGifTexturesToGIF.ResolutionHandle.ResizeKeepRatio, autoClear: false);
            // tex2Gif.Save(tex2DList2, width, height, 10, 0, 1, OnFileSaved2, OnFileSaveProgress2, ProGifTexturesToGIF.ResolutionHandle.ResizeKeepRatio, autoClear: 
            // 这样行不行？
            Debug.Log("Load images and start convert/save GIF..");//就打印了这个
        }
        else
        {
            Debug.LogWarning("No image/texture found at: " + loadImagePath);
        }
    }

    private Action OnTexture2GifOk;
    private void OnFileSaved(int id, string path)
    {
        //PlaceUIManager.Instance.GetEndUi().OnSaveGifOk();
        Debug.Log("On file saved: " + path);//这都没打印
        // text1.text = "GIF saved: " + path;
#if UNITY_EDITOR
        string sourceFolder = Application.dataPath;
        string destinationFolder = Path.Combine(sourceFolder, $"Images/Log/{UniqueTime}/{teamID}");
#else
        string sourceFolder = Application.persistentDataPath;
        string destinationFolder = Path.Combine(sourceFolder, $"Log/{UniqueTime}/{teamID}");
#endif
        // 目标文件夹路径
        if (!Directory.Exists(destinationFolder))
        {
            Directory.CreateDirectory(destinationFolder);
        }

        string fileName = Path.GetFileName(path);

        string destinationGifFile = Path.Combine(destinationFolder, fileName);
        string destinationJsonFile = Path.Combine(destinationFolder, "Art.json");

        // public ArtInfo(string artName, int score, int drawTimes, float price, List<string> contributors, string artPath, string PUID, string dir)
        ArtInfo artInfo = new ArtInfo(
            "null",
            PlaceCenter.Instance.users.Values.Sum(user => user.score),
            PlaceCenter.Instance.users.Values.Sum(user => user.drawTimes),
            PlaceCenter.Instance.users.Values.Sum(user => user.usePowerCount),
            PlaceCenter.Instance.AllMemberName(),
            artPath: destinationGifFile,
            PUID: "null",
            artTexturePath: Directory.GetFiles(destinationFolder, "*.png", SearchOption.AllDirectories).Last<string>()
        );
        SaveJson(destinationJsonFile, artInfo);


        File.Copy(path, destinationGifFile);

        // 显示
        ShowGIF(path);

        //dpImage.sprite = tex2Gif.GetSprite(0);
        OnTexture2GifOk?.Invoke();  // 这个位置 合适吗
        // dpImage.SetNativeSize();
        PlaceTeamBoardManager.Instance.OnSaveOK(teamID);
    }

    private void SaveJson(string path, ArtInfo artInfo)
    {
        string json = JsonUtility.ToJson(artInfo);
        File.WriteAllText(path, json);
        Debug.Log("Saved Json to: " + path);

    }

    private void OnFileSaveProgress(int id, float progress)
    {
        int progressInt = Mathf.CeilToInt(progress * 100);
        //Debug.Log("On file save progress: " + $"{progressInt}" + "%");
        // text1.text = "Save progress: " + Mathf.CeilToInt(progress * 100) + "%";
        //PlaceUIManager.Instance.GetEndUi().OnSaveGifLoading(progressInt);
        PlaceTeamBoardManager.Instance.OnfileProgress(teamID, progressInt);
    }

    void ShowGIF(string path)
    {
        // ProGifManager.Instance.m_OptimizeMemoryUsage = true;
        //
        // //Open the Pro GIF player to show the converted GIF
        // ProGifManager.Instance.PlayGif(path, dpImage, (loadProgress) =>
        // {
        //     // if(loadProgress < 1f)
        //     // {
        //     // 	dpImage.SetNativeSize();
        //     // }
        // });
        PGif.iPlayGif(path, dpImage.gameObject, "MyGifPlayerName 01", (texture2D) =>
        {
            // get and display the decoded texture here:
            dpImage.texture = texture2D;
        });
    }

    internal Texture2D MergeTexture(Texture2D userTex, Texture2D tex)
    {
        // 获取tex对userTex的要求
        string format = tex.name;
        string[] formats = format.Split('-');
        int centerX = int.Parse(formats[1]);
        int centerY = int.Parse(formats[2]); // base left top ,but need left bottom,so use tex.height - centerY
        centerY = tex.height - centerY; // base left top ,but need left bottom,so use tex.height - centerY
        int radius = int.Parse(formats[3]);


        // 合并两个贴图的像素
        Texture2D newTexture = MergeTwoTexture(tex, userTex, centerX - radius, centerY - radius, radius);

        return newTexture;
    }

    public Texture2D MergeTwoTexture(Texture2D target, Texture2D source, int posX, int posY, int radius = 10)
    {
        // 创建一个新的空白贴图
        Texture2D newTexture = new Texture2D(target.width, target.height, TextureFormat.RGBA32, false);
        // 裁剪 userTex
        Texture2D scaleTexture = ScaleTextureProportionally(source, (int)(radius * 2 * 1.1), (int)(radius * 2 * 1.1));

        // 检查贴图是否超出画布范围
        if (posX + scaleTexture.width > target.width || posY + scaleTexture.height > target.height)
        {
            Debug.Log("The source texture is too large"); // 应该不会
        }

        Color[] newTexturePixels = target.GetPixels();
        Color[] sourcePixels = scaleTexture.GetPixels();
        Color[] tempPixels = target.GetPixels(posX, posY, scaleTexture.width, scaleTexture.height);

        for (int y = 0; y < scaleTexture.height; y++)
        {
            for (int x = 0; x < scaleTexture.width; x++)
            {
                int targetIndex = y * scaleTexture.width + x;
                Color sourcePixel = sourcePixels[targetIndex];

                if (tempPixels[targetIndex].a == 0)  // 检查透明度
                {
                    tempPixels[targetIndex] = sourcePixel;
                }
            }
        }

        newTexture.SetPixels(newTexturePixels);
        newTexture.SetPixels(posX, posY, scaleTexture.width, scaleTexture.height, tempPixels);
        newTexture.Apply(); // 应用更改到目标贴图
        return newTexture;
    }

    public bool take_random = true;
    public bool take_sequence = false;
    // 竞赛取出像素
    public void TakeIns(User user)
    {
        string drawIns = "";
        int take_count = user.maxCarryingInsCount - user.currentCarryingInsCount;
        for (int i = 0; i < take_count && currentIndex < currentPixels.Length; i++)
        {
            int x,y,tempIndex;
            Color32 c;
            // 顺序取
            if (take_sequence)
            {
                c = currentPixels[currentIndex];
                tempIndex = currentIndex;
                currentIndex++;
            }
            // 随机取
            else {
                int index = randomIndexs[currentIndex];
                currentIndex++;
                try
                {
                    c = currentPixels[index];
                    tempIndex = index;
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    continue;
                }
                
            }
            
            // if (take_random) {
            //     int index = randomIndexs[currentIndex];
            //     c = currentPixels[index];
            // }


            
            if (c.a > 0)
            {
                x = tempIndex % templateTexture.width;
                y = tempIndex / templateTexture.width;
                drawIns = $"/d {x} {y} {c.r} {c.g} {c.b}";
                PlaceTeamInstructionManager.Instance.DefaultRunChatCommand(user, drawIns);
            }
        }
    }
}
