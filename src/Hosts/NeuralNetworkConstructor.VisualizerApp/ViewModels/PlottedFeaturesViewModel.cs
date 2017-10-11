using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NeuralNetworkConstructor.VisualizerApp.ViewModels
{
    public class PlottedFeaturesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ObservableCollection<FeatureCategoryViewModel> drawnCategories;
        private readonly ObservableCollection<FeatureViewModel> drawnFeatures;
        //private readonly ObservableCollection<EdgeViewModel> drawnEdges;
        //private readonly ObservableCollection<VertexViewModel> drawnVertices;

        private string info = string.Empty;

        public PlottedFeaturesViewModel()
        {
            this.drawnCategories = new ObservableCollection<FeatureCategoryViewModel>();
            this.DrawnCategories = new ReadOnlyObservableCollection<FeatureCategoryViewModel>(this.drawnCategories);

            this.drawnFeatures = new ObservableCollection<FeatureViewModel>();
            this.DrawnFeatures = new ReadOnlyObservableCollection<FeatureViewModel>(this.drawnFeatures);

            //this.drawnEdges = new ObservableCollection<EdgeViewModel>();
            //this.DrawnEdges = new ReadOnlyObservableCollection<EdgeViewModel>(this.drawnEdges);

            //this.drawnVertices = new ObservableCollection<VertexViewModel>();
            //this.DrawnVertices = new ReadOnlyObservableCollection<VertexViewModel>(this.drawnVertices);
        }

        public ReadOnlyObservableCollection<FeatureCategoryViewModel> DrawnCategories { get; private set; }

        public ReadOnlyObservableCollection<FeatureViewModel> DrawnFeatures { get; private set; }

        //public ReadOnlyObservableCollection<EdgeViewModel> DrawnEdges { get; private set; }

        //public ReadOnlyObservableCollection<VertexViewModel> DrawnVertices { get; private set; }

        public string Info
        {
            get
            {
                return this.info;
            }

            set
            {
                this.info = value;
                this.NotifyPropertyChanged("Info");
            }
        }

        public bool AddCategory(Color color)
        {
            var brush = new SolidColorBrush(color);
            var category = new FeatureCategoryViewModel(brush);

            if (this.drawnCategories.Contains(category))
            {
                return false;
            }

            this.drawnCategories.Add(category);

            return true;
        }

        public FeatureCategoryViewModel FindCategory(Color color)
        {
            return this.drawnCategories.FirstOrDefault(e => ((SolidColorBrush)e.Brush).Color == color);
        }

        public bool AddPoint(Color categoryColor, double x, double y)
        {
            var category = this.FindCategory(categoryColor);

            if (category == null)
            {
                return false;
            }

            var feature = new FeatureViewModel(x, y);

            if (this.drawnFeatures.Contains(feature))
            {
                return false;
            }

            this.drawnFeatures.Add(feature);

            if (!category.AddFeatures(feature))
            {
                return false;
            }
            
            return true;
        }

        public void ClearPoints()
        {
            this.drawnFeatures.Clear();

            foreach (var category in this.drawnCategories)
            {
                category.Clear();
            }
        }

        public void ClearCategories()
        {
            this.ClearPoints();
            this.drawnCategories.Clear();
        }

        private FeatureCategoryViewModel FindCategory(Brush brush)
        {
            var color = ((SolidColorBrush)brush).Color;

            return this.FindCategory(color);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
