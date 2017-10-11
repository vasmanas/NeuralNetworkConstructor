using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NeuralNetworkConstructor.VisualizerApp
{
    /// <summary>
    /// Interaction logic for NeuralNetworkTreeWindow.xaml
    /// </summary>
    public partial class NeuralNetworkTreeWindow : Window
    {
        private const string Multiplication = "*";
        private const string Addition = "+";
        private const string Inversion = "!";
        private const string Border = ">";

        private const double Size = 20;
        private const double Space = 5;

        public NeuralNetworkTreeWindow(Diagrams.NeuronLevel level)
        {
            InitializeComponent();

            this.DrawLevel(level, 0, 0);
        }

        private double DrawLevel(Diagrams.NeuronLevel level, double startTop, double startLeft)
        {
            // top (1st number) goes deeper
            // left (2nd number) goes to side
            // start:0 > width:20 > space:5 > width:20
            
            if (level == null)
            {
                return startTop;
            }

            this.DrawNode(Multiplication, startTop, startLeft);

            startLeft += Size + Space;

            double childrenTop = startTop;
            double mainTop = startTop;

            if (level.Excludes.Any())
            {
                this.DrawNode(Inversion, childrenTop, startLeft);
                this.DrawNode(Addition, childrenTop, startLeft + Size + Space);

                foreach (var l in level.Excludes)
                {
                    childrenTop = this.DrawLevel(l, childrenTop, startLeft + 2 * (Size + Space));
                }

                mainTop += Size + Space;
            }

            Diagrams.Point point = null;

            while ((point = level.Include.NextVertex(point)) != null)
            {
                this.DrawNode(Border, mainTop, startLeft);

                mainTop += Size + Space;
            }

            var maxTop = childrenTop > mainTop ? childrenTop : mainTop;

            this.Surface.Height = this.Surface.Height > maxTop ? this.Surface.Height : maxTop;

            return maxTop;
        }

        private void DrawNode(string content, double top, double left)
        {
            this.DrawText(content, top, left);
            this.DrawCircle(top, left);
        }

        private void DrawText(string content, double top, double left)
        {
            var text = new TextBlock
            {
                Text = content,
                Width = Size,
                Height = Size,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
            };

            this.DrawElemenet(text, top, left);
        }

        private void DrawCircle(double top, double left)
        {
            var ellipse = new Ellipse
            {
                Stroke = Brushes.Black,
                Width = Size,
                Height = Size
            };

            this.DrawElemenet(ellipse, top, left);
        }

        private void DrawElemenet(UIElement shape, double top, double left)
        {
            this.Surface.Children.Add(shape);

            Canvas.SetTop(shape, top);
            Canvas.SetLeft(shape, left);
        }
    }
}
