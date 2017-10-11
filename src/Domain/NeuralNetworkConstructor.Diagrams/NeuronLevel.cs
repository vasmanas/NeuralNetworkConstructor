using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkConstructor.Diagrams
{
    public class NeuronLevel
    {
        public NeuronLevel()
        {
            this.Excludes = new List<NeuronLevel>();
        }

        public Polygon Include { get; set; }

        public List<NeuronLevel> Excludes { get; private set; }
    }
}
