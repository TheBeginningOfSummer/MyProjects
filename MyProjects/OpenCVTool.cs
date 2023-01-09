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
        public MouseCallback? MouseCallbackEvent;
        public int DrawShape = 1;

        private Mat img = new Mat();
        private Window? window;
        private OpenCvSharp.Point startPoint;

        public OpenCVTool()
        {
            //MouseCallbackEvent += MouseDraw;
            Debug.WriteLine("程序启动");
        }

        public void ReadImage(string filePath)
        {
            MouseCallbackEvent = new MouseCallback(MouseDraw);
            window = new Window("Input Image");
            window.SetMouseCallback(MouseCallbackEvent);

            img = Cv2.ImRead(filePath);
            //Cv2.ImShow("Input Image", img);
            window.ShowImage(img);
            Cv2.WaitKey();
            img.Release();
            Cv2.DestroyAllWindows();
        }

        public void GetImage(string videoPath, string windowName, int delay = 30)
        {
            VideoCapture vc = new VideoCapture(videoPath);
            img = new Mat();
            while (vc.IsOpened())
            {
                if (vc.Read(img))
                {
                    VideoUpdateAction?.Invoke(img);
                    Cv2.ImShow(windowName, img);
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
            img.Release();
            Cv2.DestroyAllWindows();
        }

        public void ReadVideo(string videoPath, string windowName, int delay = 30, bool window = true)
        {
            VideoCapture vc = new VideoCapture(videoPath);
            img = new Mat();
            while (vc.IsOpened())
            {
                if (vc.Read(img))
                {
                    if (window)
                    {
                        VideoUpdateAction?.Invoke(img);
                        Cv2.ImShow(windowName, img);
                        Cv2.WaitKey(delay);
                        Suspend.WaitOne();
                        if (IsStopRead) break;
                    }
                    else
                    {
                        VideoUpdateAction?.Invoke(img);
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
            img.Release();
            Cv2.DestroyAllWindows();
        }

        public void ReadCamera(int camera, string windowName, int delay = 30)
        {
            VideoCapture vc = new VideoCapture(camera);
            img = new Mat();
            while (vc.IsOpened())
            {
                if (vc.Read(img))
                {
                    Cv2.ImShow(windowName, img);
                    Cv2.WaitKey(delay);
                }
                else
                {
                    break;
                }
                if (IsStopRead) break;
            }
            vc.Release();
            img.Release();
            Cv2.DestroyAllWindows();
        }

        public void Clear()
        {
            img.Release();
            Cv2.DestroyAllWindows();
        }
        int mm;
        public void MouseDraw(MouseEventTypes mouseEvent, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            if (mouseEvent == MouseEventTypes.LButtonDown)
            {
                startPoint.X = x;
                startPoint.Y = y;
            }
            else if (mouseEvent == MouseEventTypes.MouseMove && flags == MouseEventFlags.LButton)
            {
                OpenCvSharp.Point endPoint;
                endPoint.X = x;
                endPoint.Y = y;
                if (DrawShape == 0)
                {
                    Cv2.Line(img, startPoint, endPoint, Scalar.AliceBlue, 1);
                }
                else if (DrawShape == 1)
                {
                    Cv2.Rectangle(img, startPoint, endPoint, Scalar.AliceBlue, 1);
                }
                else if (DrawShape == 2)
                {
                    int a = endPoint.X - startPoint.X;
                    int b = endPoint.Y - startPoint.Y;
                    int r = (int)Math.Sqrt(a ^ 2 + b ^ 2);
                    Cv2.Circle(img, startPoint, r, Scalar.AliceBlue, 1);
                    //Cv2.Polylines
                }
                else
                {
                    return;
                }
            }
            else if (mouseEvent == MouseEventTypes.LButtonUp)
            {
                OpenCvSharp.Point endPoint;
                endPoint.X = x;
                endPoint.Y = y;
                if (DrawShape == 0)
                {
                    Cv2.Line(img, startPoint, endPoint, Scalar.AliceBlue, 1);
                }
                else if (DrawShape == 1)
                {
                    Cv2.Rectangle(img, startPoint, endPoint, Scalar.Green, 1);
                }
                else if (DrawShape == 2)
                {
                    int a = endPoint.X - startPoint.X;
                    int b = endPoint.Y - startPoint.Y;
                    int r = (int)Math.Sqrt(a ^ 2 + b ^ 2);
                    Cv2.Circle(img, startPoint, r, Scalar.AliceBlue, 1);
                }
                else
                {
                    return;
                }
            }
            window.ShowImage(img);
            mm++;
            Debug.WriteLine("鼠标调用" + mm);
        }
    }
}
