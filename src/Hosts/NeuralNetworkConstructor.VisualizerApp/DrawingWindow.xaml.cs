using Microsoft.Win32;
using NeuralNetworkConstructor.Algorithms;
using NeuralNetworkConstructor.Core.Messaging;
using NeuralNetworkConstructor.Diagrams;
using NeuralNetworkConstructor.DispatcherLoggerComponent;
using NeuralNetworkConstructor.Drawing.Core;
using NeuralNetworkConstructor.Drawing.SingleDraw;
using NeuralNetworkConstructor.Drawing.SingleImage;
using NeuralNetworkConstructor.VisualizerApp.Handlers;
using NeuralNetworkConstructor.VisualizerApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace NeuralNetworkConstructor.VisualizerApp
{
    /// <summary>
    /// Interaction logic for DrawingWindow.xaml
    /// </summary>
    public partial class DrawingWindow : Window
    {
        private readonly PlottedFeaturesViewModel viewModel = new PlottedFeaturesViewModel();
        
        public DrawingWindow()
        {
            InitializeComponent();

            this.DataContext = viewModel;
            viewModel.AddCategory(Colors.DarkBlue);
            viewModel.AddCategory(Colors.DarkGreen);
            viewModel.AddCategory(Colors.Red);
            viewModel.AddCategory(Colors.Violet);
            this.CategoriesView.SelectedIndex = 0;
        }

        private void Surface_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var category = this.CategoriesView.SelectedItem as FeatureCategoryViewModel;

            if (category == null)
            {
                return;
            }

            var pos = e.GetPosition(this);

            if (this.viewModel.AddPoint(category.Color, pos.X, pos.Y))
            {
                var host = new ShapesHost();
                host.AddPoint(category.Brush, pos.X, pos.Y);

                this.Surface.Children.Add(host);
            }
        }

        private async void MakeRandom_Click(object sender, RoutedEventArgs e)
        {
            var category = this.CategoriesView.SelectedItem as FeatureCategoryViewModel;

            if (category == null)
            {
                return;
            }

            var txt = this.RandomPointCount.Text;

            if (!int.TryParse(txt, out int count))
            {
                return;
            }

            var dispatcher = Dispatcher.CurrentDispatcher;
            var logger = new DispatcherLogger(dispatcher, s => this.viewModel.Info = s);
            var placedPoints = this.viewModel.DrawnFeatures.Select(f => new Diagrams.Point(f.X, f.Y)).ToList();

            var points = await Bus.Query(new GenerateRandomPoints(count, this.Surface.ActualWidth, this.Surface.ActualHeight, placedPoints, dispatcher, logger));

            points.ForEach(p => this.viewModel.AddPoint(category.Color, p.X, p.Y));

            var features =
                this.viewModel.DrawnCategories
                .Select(c => new KeyValuePair<Brush, List<Diagrams.Point>>(c.Brush, c.Features.Select(f => new Diagrams.Point(f.X, f.Y)).ToList()))
                .ToDictionary(k => k.Key, v => v.Value);

            var host = await Bus.Query(new CreateShapesContainer(this.Surface.ActualWidth, this.Surface.ActualHeight));
            await Bus.Send(new DrawPoints(features, host as IShapeComposite));

            this.Surface.Children.Clear();
            this.Surface.Children.Add(host);
        }

        private async void ShowConvexHull_Click(object sender, RoutedEventArgs e)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            var logger = new DispatcherLogger(dispatcher, s => this.viewModel.Info = s);
            var points = this.viewModel.DrawnFeatures.Select(p => new Diagrams.Point(p.X, p.Y)).ToList();

            var features =
                this.viewModel.DrawnCategories
                .Select(c => new KeyValuePair<Brush, List<Diagrams.Point>>(c.Brush, c.Features.Select(f => new Diagrams.Point(f.X, f.Y)).ToList()))
                .ToDictionary(k => k.Key, v => v.Value);

            var host = await Bus.Query(new CreateShapesContainer(this.Surface.ActualWidth, this.Surface.ActualHeight));
            var lineSegments = await Bus.Query(new MakeConvexHull(points, dispatcher, logger));
            await Bus.Send(new DrawLineSegments(new Dictionary<Brush, List<Diagrams.LineSegment>> { { Brushes.Blue, lineSegments } }, host as IShapeComposite));
            await Bus.Send(new DrawPoints(features, host as IShapeComposite));

            this.Surface.Children.Clear();
            this.Surface.Children.Add(host);
        }

        private async void Triangulate_Click(object sender, RoutedEventArgs e)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            var logger = new DispatcherLogger(dispatcher, s => this.viewModel.Info = s);
            var features =
                this.viewModel.DrawnCategories
                .Select(c => new KeyValuePair<Brush, List<Diagrams.Point>>(c.Brush, c.Features.Select(f => new Diagrams.Point(f.X, f.Y)).ToList()))
                .ToDictionary(k => k.Key, v => v.Value);

            var host = await Bus.Query(new CreateShapesContainer(this.Surface.ActualWidth, this.Surface.ActualHeight));

            if (this.SingleColor.IsChecked ?? false)
            {
                logger.Info("Making points");

                var points = this.viewModel.DrawnFeatures.Select(p => new Diagrams.Point(p.X, p.Y)).ToList();
                
                logger.Info("Sending points");

                await Bus.Send(new MakeTriangulation(points, host as IShapeComposite, dispatcher, logger));
            }
            else
            {
                logger.Info("Sending points");

                await Bus.Send(new MakeMultiColorTriangulation(features, host as IShapeComposite, dispatcher, logger));
            }

            await Bus.Send(new DrawPoints(features, host as IShapeComposite));

            this.Surface.Children.Clear();
            this.Surface.Children.Add(host);
        }

        private async void ShowVoronoiDiagram_Click(object sender, RoutedEventArgs e)
        {
            var points = this.viewModel.DrawnFeatures.Select(p => new Diagrams.Point(p.X, p.Y)).ToList();
            var features =
                this.viewModel.DrawnCategories
                .Select(c => new KeyValuePair<Brush, List<Diagrams.Point>>(c.Brush, c.Features.Select(f => new Diagrams.Point(f.X, f.Y)).ToList()))
                .ToDictionary(k => k.Key, v => v.Value);

            var edges = await Bus.Query(new CalculateVoronoiEdges(points, this.Surface.ActualWidth, this.Surface.ActualHeight));
            var host = await Bus.Query(new CreateShapesContainer(this.Surface.ActualWidth, this.Surface.ActualHeight));
            await Bus.Send(new DrawPoints(features, host as IShapeComposite));
            await Bus.Send(new DrawEdges(edges, host as IShapeComposite));

            this.Surface.Children.Clear();
            this.Surface.Children.Add(host);
        }

        private async void ShowVoronoiCategoryDiagram_Click(object sender, RoutedEventArgs e)
        {
            var points = this.viewModel.DrawnFeatures.Select(p => new Diagrams.Point(p.X, p.Y)).ToList();
            var categories =
                this.viewModel.DrawnCategories
                .Select(c => c.Features.Select(f => new Diagrams.Point(f.X, f.Y)).ToList()).ToList();
            var features =
                this.viewModel.DrawnCategories
                .Select(c => new KeyValuePair<Brush, List<Diagrams.Point>>(c.Brush, c.Features.Select(f => new Diagrams.Point(f.X, f.Y)).ToList()))
                .ToDictionary(k => k.Key, v => v.Value);

            var edges = await Bus.Query(new CalculateVoronoiEdges(points, this.Surface.ActualWidth, this.Surface.ActualHeight));
            var frontEdges = await Bus.Query(new FilterEdgesTouchingSameCategory(edges, categories));
            var host = await Bus.Query(new CreateShapesContainer(this.Surface.ActualWidth, this.Surface.ActualHeight));
            await Bus.Send(new DrawPoints(features, host as IShapeComposite));
            await Bus.Send(new DrawEdges(frontEdges, host as IShapeComposite));

            this.Surface.Children.Clear();
            this.Surface.Children.Add(host);
        }

        private async void ShowNNModelDiagram_Click(object sender, RoutedEventArgs e)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            var logger = new DispatcherLogger(dispatcher, s => this.viewModel.Info = s);

            var points = this.viewModel.DrawnFeatures.Select(p => new Diagrams.Point(p.X, p.Y)).ToList();
            var categories =
                this.viewModel.DrawnCategories
                .Select(c => c.Features.Select(f => new Diagrams.Point(f.X, f.Y)).ToList()).ToList();
            var features =
                this.viewModel.DrawnCategories
                .Select(c => new KeyValuePair<Brush, List<Diagrams.Point>>(c.Brush, c.Features.Select(f => new Diagrams.Point(f.X, f.Y)).ToList()))
                .ToDictionary(k => k.Key, v => v.Value);

            var edges = await Bus.Query(new CalculateVoronoiEdges(points, this.Surface.ActualWidth, this.Surface.ActualHeight));
            var frontEdges = await Bus.Query(new FilterEdgesTouchingSameCategory(edges, categories));

            var host = await Bus.Query(new CreateShapesContainer(this.Surface.ActualWidth, this.Surface.ActualHeight));
            await Bus.Send(new DrawPoints(features, host as IShapeComposite));

            var frontEdgesLines = new List<Diagrams.LineSegment>();

            foreach (var edge in frontEdges)
            {
                frontEdgesLines.Add(new Diagrams.LineSegment(edge.Start, edge.End));
            }

            //// Draw front edges
            //await Bus.Send(new DrawPolygon(Brushes.Orange, new Diagrams.Polygon(frontEdgesLines), host as IShapeComposite));

            var lvl = await this.Define(new Polygon(frontEdgesLines));
            
            await this.DrawLevel(lvl, host as IShapeComposite);

            this.Surface.Children.Clear();
            this.Surface.Children.Add(host);

            var w = new NeuralNetworkTreeWindow(lvl)
            {
                ShowInTaskbar = false,
                Owner = Application.Current.MainWindow,
            };
            w.Show();
        }

        private async Task DrawLevel(NeuronLevel level, IShapeComposite composite)
        {
            // Draw outer hull
            await Bus.Send(new DrawPolygon(Brushes.DodgerBlue, level.Include, composite));

            // Draw cut outs
            foreach (var cutOut in level.Excludes)
            {
                await this.DrawLevel(cutOut, composite);
            }
        }

        private async Task<NeuronLevel> Define(Diagrams.Polygon front)
        {
            var frontPoints = front.ToPoints();

            // Initial hull
            var dispatcher = Dispatcher.CurrentDispatcher;
            var logger = new DispatcherLogger(dispatcher, s => this.viewModel.Info = s);

            var hullSegments = await Bus.Query(new MakeConvexHull(frontPoints, dispatcher, logger));
            var hull = new Diagrams.Polygon(hullSegments);

            var level = new NeuronLevel { Include = hull };

            // Make sub hulls: hull - front
            var diffs = Diagrams.PolygonOperations.Difference(hull, front);

            foreach (var diff in diffs)
            {
                var l = await this.Define(diff);

                level.Excludes.Add(l);
            }

            return level;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));

                if (IsTextAllowed(text))
                {
                    return;
                }
            }

            e.CancelCommand();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            this.Clear(this.ClearCategories.IsChecked ?? true, this.ClearPoints.IsChecked ?? true);
        }

        private void NewCategoryColor_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (!this.NewCategoryColor.SelectedColor.HasValue)
            {
                return;
            }

            this.viewModel.AddCategory(this.NewCategoryColor.SelectedColor.Value);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog()
            {
                Filter = "Model Files (*.model)|*.model|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            /// Log to file points
            using (var writer = System.IO.File.CreateText(dialog.FileName))
            {
                foreach (var category in this.viewModel.DrawnCategories)
                {
                    var color = (category.Brush as SolidColorBrush).Color;

                    foreach (var feature in category.Features)
                    {
                        writer.WriteLine("{0};{1};{2};{3};{4};{5}", color.A, color.R, color.G, color.B, feature.X, feature.Y);
                    }
                }
            }
        }

        private async void Load_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "Model Files (*.model)|*.model|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            /// Clear everything befor upload
            this.Clear(true, true);

            using (System.IO.TextReader reader = System.IO.File.OpenText(dialog.FileName))
            {
                var line = reader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var parts = line.Split(';');

                    var color = Color.FromArgb(byte.Parse(parts[0]), byte.Parse(parts[1]), byte.Parse(parts[2]), byte.Parse(parts[3]));

                    this.viewModel.AddCategory(color);

                    var category = this.viewModel.FindCategory(color);

                    var x = double.Parse(parts[4]);
                    var y = double.Parse(parts[5]);

                    this.viewModel.AddPoint(category.Color, x, y);
                    
                    line = reader.ReadLine();
                }
            }

            var features =
                this.viewModel.DrawnCategories
                .Select(c => new KeyValuePair<Brush, List<Diagrams.Point>>(c.Brush, c.Features.Select(f => new Diagrams.Point(f.X, f.Y)).ToList()))
                .ToDictionary(k => k.Key, v => v.Value);

            var host = await Bus.Query(new CreateShapesContainer(this.Surface.ActualWidth, this.Surface.ActualHeight));
            await Bus.Send(new DrawPoints(features, host as IShapeComposite));

            this.Surface.Children.Clear();
            this.Surface.Children.Add(host);
        }

        private static bool IsTextAllowed(string text)
        {
            /// Regex that matches disallowed text
            var regex = new Regex("[^0-9]+");

            return !regex.IsMatch(text);
        }

        private void Clear(bool clearCategoroes, bool clearPoints)
        {
            this.Surface.Children.Clear();

            if (clearCategoroes)
            {
                this.viewModel.ClearCategories();
            }
            else
            {
                if (clearPoints)
                {
                    this.viewModel.ClearPoints();
                }
                else
                {
                    this.DrawPoints().Wait();
                }
            }
        }

        private async Task DrawPoints()
        {
            var features =
                this.viewModel.DrawnCategories
                .Select(c => new KeyValuePair<Brush, List<Diagrams.Point>>(c.Brush, c.Features.Select(f => new Diagrams.Point(f.X, f.Y)).ToList()))
                .ToDictionary(k => k.Key, v => v.Value);

            var hull = await Bus.Query(new CreateShapesContainer(this.Surface.ActualWidth, this.Surface.ActualHeight));
            await Bus.Send(new DrawPoints(features, hull as IShapeComposite));

            this.Surface.Children.Clear();
            this.Surface.Children.Add(hull);
        }
    }
}
