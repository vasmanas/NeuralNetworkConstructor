using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NeuralNetworkConstructor.VisualizerApp.ViewModels
{
    public class VertexViewModel : INotifyPropertyChanged, IEquatable<VertexViewModel>, IEqualityComparer<VertexViewModel>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public VertexViewModel(double x, double y)
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

        public bool Equals(VertexViewModel other)
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

        public bool Equals(VertexViewModel a, VertexViewModel b)
        {
            return a.Equals(b);
        }

        public int GetHashCode(VertexViewModel obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
    }
}
