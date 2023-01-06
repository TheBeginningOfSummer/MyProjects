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

        private Mat img = new Mat();

        public OpenCVTool()
        {
            Debug.WriteLine("程序启动");
            
        }

        public void ReadImage(string filePath)
        {
            img = Cv2.ImRead(filePath);
            Cv2.ImShow("Input Image", img);
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

        public void DrawShape()
        {

        }
    }
}
