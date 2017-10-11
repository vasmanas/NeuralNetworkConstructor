using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace NeuralNetworkConstructor.VisualizerApp.ViewModels
{
    public class FeatureCategoryViewModel : INotifyPropertyChanged, IEquatable<FeatureCategoryViewModel>, IEqualityComparer<FeatureCategoryViewModel>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ObservableCollection<FeatureViewModel> features;

        public FeatureCategoryViewModel(Brush color)
        {
            this.Brush = color;

            this.features = new ObservableCollection<FeatureViewModel>();
            this.Features = new ReadOnlyObservableCollection<FeatureViewModel>(this.features);
        }

        public Brush Brush { get; private set; }

        public Color Color
        {
            get
            {
                return ((SolidColorBrush)this.Brush).Color;
            }
        }

        public ReadOnlyObservableCollection<FeatureViewModel> Features { get; private set; }

        public bool AddFeatures(FeatureViewModel feature)
        {
            if (this.features.Contains(feature))
            {
                return false;
            }

            this.features.Add(feature);

            return true;
        }

        public void Clear()
        {
            this.features.Clear();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Equals(FeatureCategoryViewModel other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            var brush = this.Brush as SolidColorBrush;
            var otherBrush = other.Brush as SolidColorBrush;

            return brush.Color.Equals(otherBrush.Color)
                && brush.Opacity.Equals(otherBrush.Opacity);
        }

        public bool Equals(FeatureCategoryViewModel a, FeatureCategoryViewModel b)
        {
            return a.Equals(b);
        }

        public int GetHashCode(FeatureCategoryViewModel obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            var brush = this.Brush as SolidColorBrush;
            
            return brush.Color.GetHashCode() ^ brush.Opacity.GetHashCode();
        }
    }
}
