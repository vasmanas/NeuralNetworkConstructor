using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkConstructor.VisualizerApp.ViewModels
{
    public class EdgeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public EdgeViewModel(FeatureViewModel f1, FeatureViewModel f2, VertexViewModel v1, VertexViewModel v2)
        {
            this.Feature1 = f1 ?? throw new ArgumentNullException(nameof(f1));
            this.Feature2 = f2 ?? throw new ArgumentNullException(nameof(f2));
            this.Vertex1 = v1 ?? throw new ArgumentNullException(nameof(v1));
            this.Vertex2 = v2 ?? throw new ArgumentNullException(nameof(v2));
        }

        public FeatureViewModel Feature1 { get; private set; }

        public FeatureViewModel Feature2 { get; private set; }

        public VertexViewModel Vertex1 { get; private set; }

        public VertexViewModel Vertex2 { get; private set; }

        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
