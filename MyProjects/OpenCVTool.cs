using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace MyProjects
{
    public class OpenCVTool
    {
        public ManualResetEvent Suspend = new(true);
        public bool IsStopRead = false;
        public bool IsCaptureImage = false;
        public Action<Mat>? VideoUpdateAction;
        public MouseCallback MouseCallbackEvent;

        private Mat sourceImage = new Mat();
        #region 鼠标绘制图形
        public int DrawShape = 0;
        private readonly Mat drawingImage = new Mat();
        private readonly Mat drewImage = new Mat();
        private Window? window;
        private OpenCvSharp.Point startPoint;
        private OpenCvSharp.Point endPoint;
        #endregion
        #region 测量
        private OpenCvSharp.Size kernalSize = new OpenCvSharp.Size(7, 7);
        private Mat processedImage = new Mat();
        private Mat edgeImage = new Mat();
        #endregion

        public OpenCVTool()
        {
            MouseCallbackEvent = new MouseCallback(MouseDraw);
            Debug.WriteLine("程序启动");
        }

        public void ReadImage(string filePath)
        {
            window = new Window("Input Image");
            window.SetMouseCallback(MouseCallbackEvent);

            sourceImage = Cv2.ImRead(filePath);
            sourceImage.CopyTo(drawingImage);
            sourceImage.CopyTo(drewImage);

            //Cv2.ImShow("Input Image", img);
            window.ShowImage(drewImage);
            Cv2.WaitKey();

            sourceImage.Release();
            Cv2.DestroyAllWindows();
        }

        public void GetImage(string videoPath, string windowName, int delay = 30)
        {
            VideoCapture vc = new VideoCapture(videoPath);
            sourceImage = new Mat();
            while (vc.IsOpened())
            {
                if (vc.Read(sourceImage))
                {
                    VideoUpdateAction?.Invoke(sourceImage);
                    Cv2.ImShow(windowName, sourceImage);
                    Cv2.WaitKey(delay);
                    if (IsCaptureImage) Cv2.WaitKey();
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

        public void ReadVideo(string videoPath, string windowName, int delay = 30, bool window = true)
        {
            VideoCapture vc = new VideoCapture(videoPath);
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
                        if (IsStopRead) break;
                    }
                    else
                    {
                        VideoUpdateAction?.Invoke(sourceImage);
                        Cv2.WaitKey(delay);
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

        public void ReadCamera(int camera, string windowName, int delay = 30)
        {
            VideoCapture vc = new VideoCapture(camera);
            sourceImage = new Mat();
            while (vc.IsOpened())
            {
                if (vc.Read(sourceImage))
                {
                    Cv2.ImShow(windowName, sourceImage);
                    Cv2.WaitKey(delay);
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

        public void Clear()
        {
            sourceImage?.Release();
            Cv2.DestroyAllWindows();
        }
        
        public void MouseDraw(MouseEventTypes mouseEvent, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            if (mouseEvent == MouseEventTypes.LButtonDown)
            {
                startPoint.X = x;
                startPoint.Y = y;
            }
            else if (mouseEvent == MouseEventTypes.MouseMove && flags == MouseEventFlags.LButton)
            {
                drewImage.CopyTo(drawingImage);
                endPoint.X = x;
                endPoint.Y = y;
                if (DrawShape == 0)
                {
                    Cv2.Line(drawingImage, startPoint, endPoint, Scalar.AliceBlue, 1);
                }
                else if (DrawShape == 1)
                {
                    Cv2.Rectangle(drawingImage, startPoint, endPoint, Scalar.AliceBlue, 1);
                }
                else if (DrawShape == 2)
                {
                    int a = endPoint.X - startPoint.X;
                    int b = endPoint.Y - startPoint.Y;
                    int r = (int)Math.Sqrt(a * a + b * b);
                    Cv2.Circle(drawingImage, startPoint, r, Scalar.AliceBlue, 1);
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
                endPoint.X = x;
                endPoint.Y = y;
                if (DrawShape == 0)
                {
                    Cv2.Line(drawingImage, startPoint, endPoint, Scalar.Green, 1);
                }
                else if (DrawShape == 1)
                {
                    Cv2.Rectangle(drawingImage, startPoint, endPoint, Scalar.Green, 1);
                }
                else if (DrawShape == 2)
                {
                    int a = endPoint.X - startPoint.X;
                    int b = endPoint.Y - startPoint.Y;
                    int r = (int)Math.Sqrt(a * a + b * b);
                    Cv2.Circle(drawingImage, startPoint, r, Scalar.Green, 1);
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
            
            //Debug.WriteLine("鼠标调用" + mm);
        }

        public void ClearDraw()
        {
            sourceImage?.CopyTo(drewImage);
            if (drewImage != null) window?.ShowImage(drewImage);
        }

        public void Model1(string filePath)
        {
            //window = new Window("Processed Image");
            sourceImage = Cv2.ImRead(filePath);
            Cv2.CvtColor(sourceImage, processedImage, ColorConversionCodes.BGR2GRAY);
            Cv2.GaussianBlur(processedImage, processedImage, kernalSize, 0);

            Cv2.Canny(processedImage, edgeImage, 50, 100);
            Mat element = Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(3, 3), new OpenCvSharp.Point(1, 1));
            Cv2.Dilate(edgeImage, edgeImage, element);
            Cv2.Erode(edgeImage, edgeImage, element);

            Cv2.ImShow("edgeImage", edgeImage);
            Cv2.ImShow("processedImage", processedImage);
        }
    }
}
