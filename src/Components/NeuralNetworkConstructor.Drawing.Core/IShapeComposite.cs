using System.Windows.Media;

namespace NeuralNetworkConstructor.Drawing.Core
{
    public interface IShapeComposite
    {
        void AddLine(Brush color, double p0x, double p0y, double p1x, double p1y);

        void AddPoint(Brush color, double x, double y);
    }
}
