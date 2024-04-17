using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsLibrary
{
    public class ControlTool
    {
        public static void ControlInvoke(Control control, Action method)
        {
            if (control.IsHandleCreated)
                control.Invoke(method);
            else
                method();
        }

        public static void ControlSaveToBitmap(Control control, string path, string name)
        {
            Bitmap bitmap = new(control.Width, control.Height);
            control.DrawToBitmap(bitmap, new Rectangle(0, 0, control.Width, control.Height));
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bitmap.Save(path + "\\" + name + ".bmp");
        }

        public static Label AddLabel(Control.ControlCollection controls, Point point, string name, string text, Size size, Color color, string tag)
        {
            Label label = new()
            {
                Name = $"LB{name}",
                Location = point,
                Text = text,
                Size = size,
                BackColor = color,
                Tag = tag,
                AutoSize = false,
            };
            controls.Add(label);
            return label;
        }

        public static TextBox AddTextBox(Control.ControlCollection controls, Point point, string name, string text, Size size, Color color, string tag)
        {
            TextBox textBox = new()
            {
                Name = $"TB{name}",
                Location = point,
                Text = text,
                Size = size,
                BackColor = color,
                Tag = tag,
                AutoSize = false,
            };
            controls.Add(textBox);
            return textBox;
        }
    }

    public class ControlLayout
    {
        /// <summary>
        /// 得到一个矩形阵列的坐标
        /// </summary>
        /// <param name="x">阵列起始X坐标</param>
        /// <param name="y">阵列起始Y坐标</param>
        /// <param name="count">阵列元素个数</param>
        /// <param name="length">每行的元素个数</param>
        /// <param name="xInterval">阵列坐标x方向间距</param>
        /// <param name="yInterval">阵列坐标y方向间距</param>
        /// <returns>阵列坐标列表</returns>
        public static List<Point> SetLocation(int x, int y, int count, int length, int xInterval, int yInterval)
        {
            int o = x;
            List<Point> locationList = new();
            for (int i = 0; i < count; i++)
            {
                locationList.Add(new Point(x, y));
                x += xInterval;
                if ((i + 1) % length == 0)
                {
                    x = o;
                    y += yInterval;
                }
            }
            return locationList;
        }


    }
}
