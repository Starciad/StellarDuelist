using System.Collections.Generic;

namespace StellarDuelist.Core.Collections
{
    /// <summary>
    /// An object pool for managing reusable objects that implement the IPoolableObject interface.
    /// </summary>
    public sealed class ObjectPool
    {
        /// <summary>
        /// Gets the number of objects currently in the pool.
        /// </summary>
        public int Count => _pool.Count;

        private readonly Queue<IPoolableObject> _pool = new();

        /// <summary>
        /// Retrieves an object from the pool.
        /// </summary>
        /// <returns>The retrieved object.</returns>
        public IPoolableObject Get()
        {
            IPoolableObject value;

            if (_pool.Count > 0)
            {
                value = _pool.Dequeue();
                value.Reset();
                return value;
            }

            return default;
        }

        /// <summary>
        /// Adds an object to the pool.
        /// </summary>
        /// <param name="value">The object to add to the pool.</param>
        public void Add(IPoolableObject value)
        {
            _pool.Enqueue(value);
        }
    }
}