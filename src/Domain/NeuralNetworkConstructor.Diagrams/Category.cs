using NeuralNetworkConstructor.Diagrams.Common;
using System.Collections.Generic;

namespace NeuralNetworkConstructor.Diagrams
{
    public class Category : Aggregate
    {
        private readonly List<AggregateReference<Feature>> features = new List<AggregateReference<Feature>>();

        public void Add(Feature feature)
        {
            this.features.Add(new AggregateReference<Feature>(feature));
        }
    }
}
