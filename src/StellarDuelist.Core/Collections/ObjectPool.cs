using System;
using System.Collections;
using System.Collections.Generic;

namespace StellarDuelist.Core.Collections
{
    /// <summary>
    /// An object pool for managing reusable objects that implement the IPoolableObject interface.
    /// </summary>
    public sealed class ObjectPool
    {
        public int Count => _pool.Count;

        private readonly Queue<IPoolableObject> _pool = new();

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

        public void Add(IPoolableObject value)
        {
            _pool.Enqueue(value);
        }
    }
}