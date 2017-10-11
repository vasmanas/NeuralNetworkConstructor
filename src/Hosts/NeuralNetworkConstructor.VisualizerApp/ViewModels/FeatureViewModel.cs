using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NeuralNetworkConstructor.VisualizerApp.ViewModels
{
    public class FeatureViewModel : INotifyPropertyChanged, IEquatable<FeatureViewModel>, IEqualityComparer<FeatureViewModel>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public FeatureViewModel(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get; private set; }

        public double Y { get; private set; }

        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Equals(FeatureViewModel other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public bool Equals(FeatureViewModel a, FeatureViewModel b)
        {
            return a.Equals(b);
        }

        public int GetHashCode(FeatureViewModel obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
    }
}
