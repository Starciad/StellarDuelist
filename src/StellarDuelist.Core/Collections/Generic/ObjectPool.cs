using System;
using System.Collections.Generic;

namespace StellarDuelist.Core.Collections.Generic
{
    /// <summary>
    /// A generic object pool for managing reusable objects that implement the <see cref="IPoolableObject"/> interface.
    /// </summary>
    /// <typeparam name="TObject">The type of objects stored in the pool.</typeparam>
    public sealed class ObjectPool<TObject> where TObject : IPoolableObject
    {
        /// <summary>
        /// Gets the number of objects currently in the pool.
        /// </summary>
        public int Count => this._pool.Count;

        private readonly Queue<TObject> _pool = new();

        /// <summary>
        /// Retrieves an object from the pool.
        /// </summary>
        /// <returns>The retrieved object.</returns>
        public TObject Get()
        {
            TObject value = this._pool.Count > 0 ? this._pool.Dequeue() : Activator.CreateInstance<TObject>();

            value.Reset();
            return value;
        }

        /// <summary>
        /// Adds an object to the pool.
        /// </summary>
        /// <param name="value">The object to add to the pool.</param>
        public void Add(TObject value)
        {
            this._pool.Enqueue(value);
        }
    }
}