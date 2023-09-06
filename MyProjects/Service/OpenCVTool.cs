using System.Diagnostics;
using OpenCvSharp;

namespace MyProjects;

public class OpenCVTool
{
    public bool IsStopRead = false;
    public MouseCallback MouseCallbackEvent;
    public ManualResetEvent Suspend = new(true);
    public Action<Mat>? VideoUpdateAction;

    private Mat sourceImage = new();
    public VideoCapture? RecordCapture;
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
    
    public OpenCVTool()
    {
        MouseCallbackEvent = new MouseCallback(MouseDraw);
        Debug.WriteLine("程序启动");
    }

    #region 读取
    /// <summary>
    /// 鼠标操作
    /// </summary>
    /// <param name="mouseEvent"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="flags"></param>
    /// <param name="userData"></param>
    public void MouseDraw(MouseEventTypes mouseEvent, int x, int y, MouseEventFlags flags, IntPtr userData)
    {
        if (mouseEvent == MouseEventTypes.LButtonDown)
        {
            StartPoint.X = x;
            StartPoint.Y = y;
        }
        else if (mouseEvent == MouseEventTypes.MouseMove && flags == MouseEventFlags.LButton)
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
        else if (mouseEvent == MouseEventTypes.LButtonUp)
        {
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
        }
        else if (mouseEvent == MouseEventTypes.RButtonDown)
        {
            drawingImage.CopyTo(drewImage);
            window?.ShowImage(drewImage);
        }
    }
    /// <summary>
    /// 读取图片
    /// </summary>
    /// <param name="filePath">文件路径</param>
    public void ReadImage(string filePath, string windowName = "Input Image")
    {
        window = new Window(windowName);
        window.SetMouseCallback(MouseCallbackEvent);

        sourceImage = Cv2.ImRead(filePath);
        sourceImage.CopyTo(drawingImage);
        sourceImage.CopyTo(drewImage);

        window.ShowImage(drewImage);
        Cv2.WaitKey();

        sourceImage.Release();
        Cv2.DestroyAllWindows();
    }
    /// <summary>
    /// 读取视频
    /// </summary>
    /// <param name="videoPath">视频路径</param>
    /// <param name="windowName">窗口名称</param>
    /// <param name="delay">帧间隔时间</param>
    /// <param name="window">是否建立窗口</param>
    public void ReadVideo(string videoPath, string windowName, int delay = 30, bool window = true)
    {
        VideoCapture vc = new(videoPath);
        sourceImage = new Mat();
        while (vc.IsOpened())
        {
            if (vc.Read(sourceImage))
            {
                if (window)
                {
                    VideoUpdateAction?.Invoke(sourceImage);
                    Cv2.ImShow(windowName, sourceImage);
                    Cv2.WaitKey(delay);
                    Suspend.WaitOne();
                }
                else
                {
                    VideoUpdateAction?.Invoke(sourceImage);
                    Cv2.WaitKey(delay);
                    Suspend.WaitOne();
                }
            }
            else
            {
                break;
            }
            if (IsStopRead) break;
        }
        vc.Release();
        sourceImage.Release();
        Cv2.DestroyAllWindows();
    }
    /// <summary>
    /// 读取摄像头
    /// </summary>
    /// <param name="camera">摄像头号</param>
    /// <param name="windowName">窗口名称</param>
    /// <param name="delay">帧间隔时间</param>
    /// <param name="window">是否建立窗口</param>
    public void ReadCamera(int camera, string windowName, int delay = 30, bool window = true)
    {
        VideoCapture vc = new(camera);
        sourceImage = new Mat();
        while (vc.IsOpened())
        {
            if (vc.Read(sourceImage))
            {
                if (window)
                {
                    VideoUpdateAction?.Invoke(sourceImage);
                    Cv2.ImShow(windowName, sourceImage);
                    Cv2.WaitKey(delay);
                    Suspend.WaitOne();
                }
                else
                {
                    VideoUpdateAction?.Invoke(sourceImage);
                    Cv2.WaitKey(delay);
                    Suspend.WaitOne();
                }
            }
            else
            {
                break;
            }
            if (IsStopRead) break;
        }
        vc.Release();
        sourceImage.Release();
        Cv2.DestroyAllWindows();
    }
    #endregion

    #region 记录
    public void OpenCamera(int camera)
    {
        RecordCapture = new VideoCapture(camera);
    }

    public void CloseCamera()
    {
        RecordCapture?.Release();
    }

    public Mat GetCameraImage()
    {
        Mat image = new Mat();
        if (RecordCapture == null) return image;
        if (RecordCapture.IsOpened())
        {
            RecordCapture.Grab();
            //if (CameraCapture.Read(image)) return image;
            return RecordCapture.RetrieveMat();
        }
        return image;
    }

    public void SaveCameraImage(string folderPath, string fileName)
    {
        if (RecordCapture == null) return;
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
        string filePath = Path.Combine(folderPath, fileName);

        GetCameraImage().SaveImage(filePath);
        GetCameraImage().Release();
    }
    /// <summary>
    /// 记录摄像头
    /// </summary>
    /// <param name="folderPath">记录的文件夹路径</param>
    /// <param name="fileName">记录的文件名</param>
    /// <param name="fps">帧率</param>
    /// <param name="camera">相机号</param>
    /// <param name="size">视图大小</param>
    /// <param name="window">是否显示窗口</param>
    public void RecordCamera(string folderPath, string fileName, int fps, OpenCvSharp.Size size, bool window = false)
    {
        if (RecordCapture == null) return;
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
        string filePath = Path.Combine(folderPath, fileName);

        FourCC fourCC = new FourCC(FourCC.MP4V);
        VideoWriter vm = new VideoWriter(filePath, fourCC, fps, size);
        Mat image = new Mat();
        while (RecordCapture.IsOpened())
        {
            if (RecordCapture.Read(image))
            {
                VideoUpdateAction?.Invoke(sourceImage);
                vm.Write(image);
                if (window) Cv2.ImShow("Record", image);
                Cv2.WaitKey(1000 / fps);
            }
            if (IsStopRead) break;
        }
        vm.Release();
        image.Release();
        Cv2.DestroyAllWindows();
    }
    #endregion

    /// <summary>
    /// 清除窗口
    /// </summary>
    public void Clear()
    {
        sourceImage?.Release();
        Cv2.DestroyAllWindows();
    }
    /// <summary>
    /// 清除绘制痕迹
    /// </summary>
    public void ClearDraw()
    {
        sourceImage?.CopyTo(drewImage);
        if (drewImage != null) window?.ShowImage(drewImage);
    }
    
}
