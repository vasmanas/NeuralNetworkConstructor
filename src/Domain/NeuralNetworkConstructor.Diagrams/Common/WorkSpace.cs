using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkConstructor.Diagrams.Common
{
    public partial class WorkSpace
    {
        private readonly Dictionary<Type, object> repositoies = new Dictionary<Type, object>();

        public IEnumerable<T> Find<T>(Func<T, bool> filter = null)
            where T : Aggregate
        {
            var repository = this.GetRepository<T>();

            if (repository == null)
            {
                throw new NullReferenceException($"Repository for type {typeof(T).Name} not found");
            }

            return repository.Find(filter);
        }

        public void Save<T>(T item)
            where T : Aggregate
        {
            var repository = this.GetOrCreateRepository<T>();

            repository.Save(item);
        }

        public void Remove<T>(T item)
            where T : Aggregate
        {
            var repository = this.GetRepository<T>();

            if (repository == null)
            {
                return;
            }

            repository.Remove(item);
        }

        private Repository<T> GetOrCreateRepository<T>()
            where T : Aggregate
        {
            var repository = this.GetRepository<T>();

            if (repository != null)
            {
                return repository;
            }

            repository = new Repository<T>();

            this.repositoies.Add(typeof(T), repository);

            return repository;
        }

        private Repository<T> GetRepository<T>()
            where T : Aggregate
        {
            if (this.repositoies.TryGetValue(typeof(T), out object repository))
            {
                return repository as Repository<T>;
            }

            return null;
        }
    }
}
