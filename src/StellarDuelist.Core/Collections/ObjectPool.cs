using System;
using System.Collections.Generic;

namespace StellarDuelist.Core.Collections
{
    /// <summary>
    /// A generic object pool for managing reusable objects that implement the <see cref="IPoolableObject"/> interface.
    /// </summary>
    /// <typeparam name="TObject">The type of objects stored in the pool.</typeparam>
    public sealed class ObjectPool<TObject> where TObject : IPoolableObject
    {
        private readonly Queue<TObject> _pool = new();

        public TObject Get()
        {
            TObject value;

            if (this._pool.Count > 0)
            {
                value = this._pool.Dequeue();
            }
            else
            {
                value = Activator.CreateInstance<TObject>();
            }

            value.Reset();
            return value;
        }

        public void Add(TObject value)
        {
            this._pool.Enqueue(value);
        }
    }
}
