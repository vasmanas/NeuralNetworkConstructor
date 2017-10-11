using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkConstructor.Diagrams.Common
{
    public class AggregateReference<T>
        where T : Aggregate
    {
        public AggregateReference()
            : this(Guid.Empty)
        {
        }

        public AggregateReference(Guid id)
        {
            this.Id = id;
        }

        public AggregateReference(T aggregate)
        {
            if (aggregate == null)
            {
                throw new ArgumentNullException(nameof(aggregate));
            }

            this.Id = aggregate.Id;
        }

        public Guid Id { get; set; }
    }
}
