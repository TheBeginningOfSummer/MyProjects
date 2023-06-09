using OpenCvSharp;

namespace MyProjects;

public class ImageProcessing
{
    public MouseCallback MouseCallbackEvent;

    private static OpenCvSharp.Size kernalSize = new(7, 7);

    #region 鼠标绘制图形
    //要画的形状
    public int DrawShape = 0;
    //绘制中的形状
    private readonly Mat drawingImage = new();
    //绘制完成的形状
    private readonly Mat drewImage = new();
    //图片显示窗口
    private Window? window;
    //起始点
    public OpenCvSharp.Point StartPoint;
    //结束点
    public OpenCvSharp.Point EndPoint;
    #endregion

    public ImageProcessing()
    {
        MouseCallbackEvent = new MouseCallback(MouseDraw);
    }

    private void MouseDraw(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userData)
    {
        if (@event == MouseEventTypes.LButtonDown)
        {
            StartPoint.X = x;
            StartPoint.Y = y;
        }
        else if (@event == MouseEventTypes.MouseMove && flags == MouseEventFlags.LButton)
        {
            drewImage.CopyTo(drawingImage);
            EndPoint.X = x;
            EndPoint.Y = y;
            if (DrawShape == 0)
            {
                Cv2.Line(drawingImage, StartPoint, EndPoint, Scalar.AliceBlue, 1);
            }
            else if (DrawShape == 1)
            {
                Cv2.Rectangle(drawingImage, StartPoint, EndPoint, Scalar.AliceBlue, 1);
            }
            else if (DrawShape == 2)
            {
                int a = EndPoint.X - StartPoint.X;
                int b = EndPoint.Y - StartPoint.Y;
                int r = (int)Math.Sqrt(a * a + b * b);
                Cv2.Circle(drawingImage, StartPoint, r, Scalar.AliceBlue, 1);
                //Cv2.Polylines
            }
            else
            {
                return;
            }
            window?.ShowImage(drawingImage);
        }
        else if (@event == MouseEventTypes.LButtonUp)
        {
            Rect rect = new(StartPoint.X, StartPoint.Y, x - StartPoint.X, y - StartPoint.Y);
            drewImage.CopyTo(drawingImage);
            EndPoint.X = x;
            EndPoint.Y = y;
            if (DrawShape == 0)
            {
                Cv2.Line(drawingImage, StartPoint, EndPoint, Scalar.Green, 1);
            }
            else if (DrawShape == 1)
            {
                Cv2.Rectangle(drawingImage, StartPoint, EndPoint, Scalar.Green, 1);
            }
            else if (DrawShape == 2)
            {
                int a = EndPoint.X - StartPoint.X;
                int b = EndPoint.Y - StartPoint.Y;
                int r = (int)Math.Sqrt(a * a + b * b);
                Cv2.Circle(drawingImage, StartPoint, r, Scalar.Green, 1);
            }
            else
            {
                return;
            }
            window?.ShowImage(drawingImage);
            Mat dst = drewImage.Clone(rect);
            Cv2.ImShow("dst", dst);
        }
        else if (@event == MouseEventTypes.RButtonDown)
        {
            drawingImage.CopyTo(drewImage);
            window?.ShowImage(drewImage);
        }
    }
    /// <summary>
    /// 读取图片
    /// </summary>
    /// <param name="filePath">文件路径</param>
    public void ReadImage(Mat sourceImage, string windowName = "Input Image")
    {
        window = new Window(windowName);
        window.SetMouseCallback(MouseCallbackEvent);

        //sourceImage = Cv2.ImRead(filePath);
        sourceImage.CopyTo(drawingImage);
        sourceImage.CopyTo(drewImage);

        window.ShowImage(drewImage);
        Cv2.WaitKey();

        sourceImage.Release();
        Cv2.DestroyAllWindows();
    }

    public static void ProcessingMethod1(string filePath)
    {
        Mat sourceImage = Cv2.ImRead(filePath);
        Mat processedImage = new();
        Mat edgeImage = new();
        //灰度转换
        Cv2.CvtColor(sourceImage, processedImage, ColorConversionCodes.BGR2GRAY);
        //高斯模糊
        Cv2.GaussianBlur(processedImage, processedImage, kernalSize, 0);
        //边缘提取
        Cv2.Canny(processedImage, edgeImage, 50, 100);
        Mat element = Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(1, 1), new OpenCvSharp.Point(0, 0));
        //膨胀
        Cv2.Dilate(edgeImage, edgeImage, element);
        //腐蚀
        Cv2.Erode(edgeImage, edgeImage, element);
        OpenCvSharp.Point[][] contours;
        HierarchyIndex[] hierarchies;
        Cv2.FindContours(edgeImage, out contours, out hierarchies, RetrievalModes.External, ContourApproximationModes.ApproxNone);

        Cv2.ImShow("edgeImage", edgeImage);
        Cv2.ImShow("processedImage", processedImage);
    }

    public static void Positioning(Mat sourceImage)
    {
        //处理的图片
        Mat processedImage = new();
        //边缘图片
        Mat edgeImage = new();
        //灰度转换
        Cv2.CvtColor(sourceImage, processedImage, ColorConversionCodes.BGR2GRAY);
        //高斯模糊
        Cv2.GaussianBlur(processedImage, processedImage, kernalSize, 0);
        //边缘提取
        Cv2.Canny(processedImage, edgeImage, 50, 100);
        //找出轮廓
        Cv2.FindContours(edgeImage, out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchies, RetrievalModes.External, ContourApproximationModes.ApproxNone);

        Cv2.ImShow("edgeImage", edgeImage);

        //在原图的复制体上画出轮廓
        Mat contoursImage = new();
        sourceImage.CopyTo(contoursImage);
        Cv2.DrawContours(contoursImage, contours, -1, Scalar.White);
        Cv2.ImShow("contours", contoursImage);
        //定义原点
        Cv2.Circle(contoursImage, 100, 100, 2, Scalar.Green, 3);

        OpenCvSharp.Point[] approx;
        for (int i = 0; i < contours.Length; i++)
        {
            approx = Cv2.ApproxPolyDP(contours[i], Cv2.ArcLength(contours[i], true) * 0.04, true);
            if (Cv2.ContourArea(contours[i]) < 600 || Cv2.IsContourConvex(approx))
                continue;
            if (approx.Length == 6)
                continue;//六边形
        }
    }

    public static void Cut(Mat sourceImage)
    {
        Cv2.ImShow("原图", sourceImage);
        Rect rect = new(300, 0, 600, 300);
        Mat dst = sourceImage.Clone(rect);
        Cv2.ImShow("dst", dst);
    }
}

abstract class ROI
{
    public RotatedRect RotatedRect { get; set; }
    public Rect BoundingRect => Cv2.BoundingRect(GetPoints());
    public OpenCvSharp.Point Center
    {
        get
        {
            OpenCvSharp.Point point = new();
            foreach (var pp in GetPoints())
            {
                point += pp;
            }
            point *= (1.0 / GetPoints().Length);
            return point;
        }
    }

    public abstract OpenCvSharp.Point[] GetPoints();
    public abstract Mat DrawROI(Mat src, Scalar scalar, int thinckness = 1);

    public Mat GetMaskFloodFill(OpenCvSharp.Size size)
    {
        if (size.Width < BoundingRect.Size.Width || size.Height < BoundingRect.Size.Height)
            size = BoundingRect.Size;
        Mat mask = Mat.Zeros(size, MatType.CV_8UC3);
        mask = DrawROI(mask, Scalar.Red);

        OpenCvSharp.Point pt = new(Center.X, Center.Y);
        Cv2.FloodFill(mask, pt, Scalar.Red);
        mask.ConvertTo(mask, MatType.CV_8UC1);
        return mask;
    }

    public Mat ExtractROI(Mat src)
    {
        Mat mask = GetMaskFloodFill(src.Size());
        Rect rect = BoundingRect;

        src = new Mat(src, rect);
        Mat maskROI = new(mask, rect);

        Cv2.CvtColor(maskROI, maskROI, ColorConversionCodes.BGR2GRAY);
        Mat dst = new();
        Cv2.BitwiseAnd(src, src, dst, maskROI);
        return dst;
    }

    public Mat InPaintROI(Mat src)
    {
        Mat mask = GetMaskFloodFill(src.Size());
        Cv2.CvtColor(mask, mask, ColorConversionCodes.BGR2GRAY);
        Mat dst = new();
        Cv2.Inpaint(src, mask, dst, 1, InpaintMethod.NS);
        return dst;
    }

    public void Reset()
    {
        RotatedRect = new();
    }

    public bool OverRun(Mat src, int gap = 3)
    {
        bool en = true;
        Rect rect = RotatedRect.BoundingRect();

        en = en && rect.X >= gap;
        en = en && rect.Y >= gap;
        en = en && rect.Right <= src.Width - gap;
        en = en && rect.Bottom <= src.Height - gap;
        return !en;
    }

    public bool IsNull => RotatedRect.Size == new Size2f(0, 0);


}
