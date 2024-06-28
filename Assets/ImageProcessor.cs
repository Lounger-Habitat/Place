using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgcodecsModule;
using UnityEngine.UI;
using OpenCVForUnity.UnityUtils;
using TMPro;

public class ImageProcessor : MonoBehaviour
{
    public int camp;
    public int width = 100;
    public int height = 100;
    public string imagePath = "";

    public Color32 defaultColor = new Color32(255, 255, 255, 255);
    public RawImage bg;
    public RawImage paint;

    public Texture2D team1;
    public Texture2D team2;

    public TMP_InputField inputField;

    public Color32 currentColor = new Color32(255, 0, 0, 255);

    Color32[] t1c;
    Color32[] t2c;

    // 光标
    public (int, int) cursor = (10, 10);

    // Start is called before the first frame update
    void Start()
    {
        GenTexture(bg, width, height, defaultColor);
        GenTexture(paint, width, height, Color.clear);
        t1c = team1.GetPixels32();
        t2c = team2.GetPixels32();
    }
    void GenTexture(RawImage img, int w, int h, Color32 color)
    {
        Texture2D newTexture = new Texture2D(w, h, TextureFormat.RGBA32, false);
        newTexture.filterMode = FilterMode.Point;
        // 设置贴图颜色
        Color[] colors = new Color[w * h];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }
        newTexture.SetPixels(colors);
        newTexture.Apply();
        img.texture = newTexture;
        // rawImage.SetNativeSize();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("MakeContours");
            MakeContours(imagePath);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(StartPaint(team1));
            StartCoroutine(StartPaint(team2));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetPoint();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Mat src = Imgcodecs.imread(imagePath, Imgcodecs.IMREAD_UNCHANGED);
            Texture2D texture = new Texture2D(src.cols(), src.rows(), TextureFormat.RGBA32, false);
            Utils.matToTexture2D(src, texture);
            PasteImage(0, 0, texture);
        }

        // 按键 上下左右
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("UpArrow");
            cursor.Item2 += 1;
            SetPoint(cursor.Item1, cursor.Item2);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("DownArrow");
            cursor.Item2 -= 1;
            SetPoint(cursor.Item1, cursor.Item2);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("LeftArrow");
            cursor.Item1 -= 1;
            SetPoint(cursor.Item1, cursor.Item2);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("RightArrow");
            cursor.Item1 += 1;
            SetPoint(cursor.Item1, cursor.Item2);
        }
    }

    private void SetPoint()
    {
        if (camp == 1)
        {
            int x = Random.Range(0, team1.width);
            int y = Random.Range(0, team1.height);
            int index = y * width + x;
            Color32 c = t1c[index];
            Texture2D t2d = paint.texture as Texture2D;
            t2d.SetPixel(x, y, c);
            t2d.Apply();
        }
        else if (camp == 2)
        {
            int x = Random.Range(0, team2.width);
            int y = Random.Range(0, team2.height);
            int index = y * width + x;
            Color32 c = t2c[index];
            Texture2D t2d = paint.texture as Texture2D;
            t2d.SetPixel(x, y, c);
            t2d.Apply();
        }
    }

    private void SetPoint(int x, int y)
    {
        Color32 c = currentColor;
        Texture2D t2d = paint.texture as Texture2D;
        t2d.SetPixel(x, y, c);
        t2d.Apply();
    }

    IEnumerator StartPaint(Texture2D t)
    {
        Color32[] tc = t.GetPixels32();
        int originx = 0;
        int originy = 0;
        if (t == team1)
        {
            originx = 0;
            originy = 0;
        }
        else
        {
            originx = 0;
            originy = t.width;
        }

        while (true)
        {
            int x = Random.Range(0, t.width);
            int y = Random.Range(0, t.height);
            int index = y * width + x;
            Color32 c = tc[index];
            Texture2D t2d = paint.texture as Texture2D;
            t2d.SetPixel(x, y, c);
            t2d.Apply();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void MakeContours(string imagePath)
    {
        // check image path file is Exists and is image
        if (!System.IO.File.Exists(imagePath))
        {
            Debug.LogError("File not found: " + imagePath);
            return;
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


        Texture2D texture = new Texture2D(drawing.cols(), drawing.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(drawing, texture);
    }

    void OldMakeContours(string imagePath)
    {
        // check image path file is Exists and is image
        if (!System.IO.File.Exists(imagePath))
        {
            Debug.LogError("File not found: " + imagePath);
            return;
        }

        // Load image
        Mat src = Imgcodecs.imread(imagePath, Imgcodecs.IMREAD_UNCHANGED);
        Mat gray = new Mat();
        Imgproc.cvtColor(src, gray, Imgproc.COLOR_BGR2GRAY);

        // Canny
        Mat edges = new Mat();
        Imgproc.Canny(gray, edges, 50, 150);

        // Find contours
        List<MatOfPoint> contours = new List<MatOfPoint>();
        Mat hierarchy = new Mat();
        Imgproc.findContours(edges, contours, hierarchy, Imgproc.RETR_TREE, Imgproc.CHAIN_APPROX_SIMPLE);



        List<Point> allPoints = new List<Point>();
        foreach (MatOfPoint contour in contours)
        {
            foreach (Point point in contour.toArray())
            {
                allPoints.Add(point);
            }
        }

        MatOfPoint2f allPointsMat = new MatOfPoint2f();
        allPointsMat.fromList(allPoints);
        OpenCVForUnity.CoreModule.Rect boundingRect = Imgproc.boundingRect(new MatOfPoint(allPointsMat.toArray()));

        List<MatOfPoint> shiftedContours = new List<MatOfPoint>();
        foreach (MatOfPoint contour in contours)
        {
            List<Point> shiftedContour = new List<Point>();
            foreach (Point point in contour.toArray())
            {
                shiftedContour.Add(new Point(point.x - boundingRect.x, point.y - boundingRect.y));
            }
            MatOfPoint shiftedContourMat = new MatOfPoint();
            shiftedContourMat.fromList(shiftedContour);
            shiftedContours.Add(shiftedContourMat);
            // Imgproc.drawContours(contourImgMat, new List<MatOfPoint> { shiftedContourMat }, -1, new Scalar(0, 255, 0, 255), 1);
        }
        // Draw contours on transparent image
        Mat drawing = Mat.zeros(boundingRect.size(), CvType.CV_8UC4);
        Debug.Log("boundingRect.size() : " + boundingRect.size());

        Imgproc.drawContours(drawing, shiftedContours, -1, new Scalar(0, 255, 0, 255), 1);

        // contours trans to mat

        // Bounding rect
        // for (int i = 0; i < contours.Count; i++)
        // {
        //     OpenCVForUnity.CoreModule.Rect rect = Imgproc.boundingRect(contours[i]);
        //     Imgproc.rectangle(drawing, rect.tl(), rect.br(), new Scalar(0, 255, 0, 255), 2);
        // }

        // OpenCVForUnity.CoreModule.Rect rect = Imgproc.boundingRect(drawing);

        // Debug.Log("rect: " + rect);


        // Imgproc.rectangle(drawing, rect.tl(), rect.br(), new Scalar(0, 255, 255, 255), 1);




        // Save image
        // Imgcodecs.imwrite("Assets/Output.png", drawing);

        // trans to unity texture

        Texture2D texture = new Texture2D(drawing.cols(), drawing.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(drawing, texture);



        // PasteImage(0, 0, texture);
    }



    void PasteImage(int x, int y, Texture2D texture)
    {
        Texture2D originTexture = paint.texture as Texture2D;
        if (texture == null || originTexture == null)
        {
            Debug.LogError("Source or Destination Texture is not set.");
            return;
        }

        // 确保复制操作在目标贴图范围内
        if (x + texture.width > originTexture.width || y + texture.height > originTexture.height)
        {
            Debug.LogError("Source texture is too large for the given position in the destination texture.");
            return;
        }

        // 获取源贴图像素
        Debug.Log("texture.width: " + texture.width + " texture.height: " + texture.height);
        Color[] sourcePixels = texture.GetPixels();

        // 将源贴图像素复制到目标贴图
        originTexture.SetPixels(x, y, texture.width, texture.height, sourcePixels);
        originTexture.Apply();
        paint.texture = originTexture;
        // rawImage.SetNativeSize();
    }

    public void ReceiveIns()
    {
        string ins = inputField.text;
    }

    void InsM(string ins)
    {
        // string[] parts = ins.Split(' ');
        // if (parts.Length == 2)
        // {
        //     int x, y, r, g, b;
        //     string s, dc;
        //     s = parts[1]; // seq
        //     dc = parts[2];
        //     (x, y) = user.lastPoint;
        //     if (colorDict.ContainsKey(dc))
        //     {
        //         color = colorDict[dc];
        //         r = color.r; // r
        //         g = color.g; // g
        //         b = color.b; // b
        //     }
        //     else
        //     {
        //         Debug.Log("抱歉此颜色目前未包含在,可联系管理员申请新增颜色");
        //         // UI 提示
        //         PlaceUIManager.Instance.AddTips(new TipsItem()
        //         {
        //             text = $"尊敬的{user.Name},抱歉此颜色({dc})目前未包含在,可联系管理员申请新增颜色"
        //         });
        //         break;
        //     }
        //     for (int i = 0; i < s.Length; i++)
        //     {
        //         char digitIns = s[i];
        //         (x, y) = ComputeQuickIns(digitIns, x, y);
        //         Instruction drawIns = new Instruction("/d", x, y, r: r, g: g, b: b);
        //         if (!PlaceBoardManager.Instance.CheckIns(drawIns))
        //         {
        //             Debug.Log($"{user.Name} , /m 指令不合法,{drawIns}");
        //             PlaceUIManager.Instance.AddTips(new TipsItem()
        //             {
        //                 text = $"尊敬的{user.Name},输入的指令不合法"
        //             });
        //             continue;
        //         }
        //         user.instructionQueue.Enqueue(drawIns);
        //     }
        // }
    }

    public Dictionary<string, Color32> colorDict = new Dictionary<string, Color32>(){
        // 无色彩系
        {"白", new Color32(255, 255, 255, 255)},
        {"银", new Color32(192, 192, 192, 255)},
        {"灰", new Color32(128, 128, 128, 255)},
        {"黑", new Color32(0, 0, 0, 255)},

        // 红
        {"红", new Color32(255, 0, 0, 255)},
        {"洋红", new Color32(255, 0, 255, 255)},
        {"橙", new Color32(255, 102, 0, 255)},
        {"粉", new Color32(255, 192, 203, 255)},

        // 绿
        {"绿", new Color32(0, 128, 0, 255)},
        {"浅绿", new Color32(144, 238, 144, 255)},
        {"鲜绿", new Color32(0, 255, 0, 255)},

        // 蓝
        {"蓝", new Color32(0, 0, 255, 255)},
        {"浅蓝", new Color32(173, 216, 230, 255)},
        {"蔚蓝", new Color32(0, 123, 167, 255)},
        {"天蓝", new Color32(0, 127, 255, 255)},

        {"黄", new Color32(255, 255, 0, 255)},
        {"紫", new Color32(106,  13  ,173, 255)},
        {"青", new Color32(0, 255, 255, 255)},


        {"棕", new Color32(165, 42, 42, 255)},
        {"金", new Color32(255, 215, 0, 255)},

    };
}

// 对图片的处理
// preprocess ： 裁剪、背景透明、等比缩放、轮廓提取

// 预制，制作轮廓图
// Input ： 处理过的图片

// Output ：轮廓图 