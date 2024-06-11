using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgcodecsModule;
using UnityEngine.UI;
using OpenCVForUnity.UnityUtils;

public class ImageTest : MonoBehaviour
{
    public string imagePath = "";
    public RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {
        // Imgproc.cvtColor
        // Imgproc.Canny
        // Imgproc.findContours
        // Imgproc.rectangle
        // Imgproc.boundingRect
        // Imgproc.drawContours

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("MakeContours");
            MakeContours(imagePath);
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

        

        PasteImage(10,10,texture);
    }

    void PasteImage(int x, int y, Texture2D texture)
    {
        Texture2D originTexture = rawImage.texture as Texture2D;
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
        rawImage.texture = originTexture;
        rawImage.SetNativeSize();
    }
}
