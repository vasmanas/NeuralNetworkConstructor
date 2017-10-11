using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkConstructor.Diagrams.Common
{
    public partial class WorkSpace
    {
		public static class Factory
        {
			public static WorkSpace Create()
            {
                return new WorkSpace();
            }
        }
    }
}
