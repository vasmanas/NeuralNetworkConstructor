using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkConstructor.Diagrams.Common
{
    public class Repository<T> where T : Aggregate
    {
        private readonly Dictionary<Guid, T> items = new Dictionary<Guid, T>();

        public T Find(Guid id)
        {
            return this.Find(e => e.Id == id).FirstOrDefault();
        }

        public IEnumerable<T> Find(Func<T, bool> filter = null)
        {
            if (filter == null)
            {
                return this.items.Values;
            }

            return this.items.Values.Where(filter);
        }

        public void Save(T item)
        {
            if (this.items.ContainsKey(item.Id))
            {
                this.items[item.Id] = item;
            }
            else
            {
                this.items.Add(item.Id, item);
            }
        }
        
        public void Remove(T item)
        {
            this.items.Remove(item.Id);
        }
    }
}
