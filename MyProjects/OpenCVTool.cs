using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace MyProjects
{
    public class OpenCVTool
    {
        public static bool IsStopRead = false;

        public OpenCVTool()
        {

        }

        public static void ReadImage(string filePath)
        {
            Mat img = Cv2.ImRead(filePath);
            Cv2.ImShow("Input Image", img);
            //Cv2.WaitKey();
            img.Release();
            Cv2.DestroyAllWindows();
        }

        public static void ReadVideo(string videoPath, string windowName)
        {
            VideoCapture vc = new VideoCapture(videoPath);
            Mat img = new Mat();
            while (vc.IsOpened())
            {
                if (vc.Read(img))
                {
                    Cv2.ImShow(windowName, img);
                    Cv2.WaitKey(30);
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

        public static void ReadVideo(int camera, string windowName)
        {
            VideoCapture vc = new VideoCapture(camera);
            Mat img = new Mat();
            while (vc.IsOpened())
            {
                if (vc.Read(img))
                {
                    Cv2.ImShow(windowName, img);
                    Cv2.WaitKey(30);
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
    }
}
