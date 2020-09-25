using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Common
{
    public class DrawTools
    {
        public static Image CreateWaterMark(string content, int width = 200, int height = 200)
        {
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.Clear(Color.White);
                Font font = new Font("微乳雅黑", 14, GraphicsUnit.Pixel);
                SizeF size = g.MeasureString(content, font);
                Matrix matrix = g.Transform;
                matrix.RotateAt(-45, new PointF(width / 2, height / 2));
                g.Transform = matrix;

                g.DrawString(content, font, new SolidBrush(ColorTranslator.FromHtml("#DDDDDD")), new PointF((width - size.Width) / 2, (height - size.Height) / 2));
            }
            return bitmap;
        }
    }
}
