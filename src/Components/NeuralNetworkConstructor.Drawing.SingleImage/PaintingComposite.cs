using NeuralNetworkConstructor.Drawing.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NeuralNetworkConstructor.Drawing.SingleImage
{
    public class PaintingComposite : Image, IShapeComposite
    {
        private readonly WriteableBitmap bmp;

        public PaintingComposite(double width, double height)
        {
            /// Initialize the WriteableBitmap with size 512x512 and set it as source of an Image control
            this.bmp = BitmapFactory.New((int)width + 1, (int)height + 1);

            using (this.bmp.GetBitmapContext())
            {
                /// Clear the WriteableBitmap with white color
                this.bmp.Clear(Colors.White);
            }

            this.Source = this.bmp;
        }

        public void AddLine(Brush color, double p0x, double p0y, double p1x, double p1y)
        {
            using (this.bmp.GetBitmapContext())
            {
                this.bmp.DrawLine((int)p0x, (int)p0y, (int)p1x, (int)p1y, ((SolidColorBrush)color).Color);
            }
        }

        public void AddPoint(Brush color, double x, double y)
        {
            using (this.bmp.GetBitmapContext())
            {
                this.bmp.DrawEllipseCentered((int)x, (int)y, 1, 1, ((SolidColorBrush)color).Color);
                //this.bmp.SetPixel((int)x, (int)y, ((SolidColorBrush)color).Color);
            }
        }
    }
}
