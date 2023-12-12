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
        private readonly Dictionary<Type, Queue<TObject>> _objectPool = new();

        /// <summary>
        /// Gets an object of type <typeparamref name="TKey"/> from the pool.
        /// </summary>
        /// <typeparam name="TKey">The type of object to retrieve from the pool.</typeparam>
        /// <returns>An object from the pool.</returns>
        public TObject Get<TKey>() where TKey : TObject
        {
            return Get(typeof(TKey));
        }

        /// <summary>
        /// Gets an object of the specified type from the pool.
        /// </summary>
        /// <param name="keyType">The type of object to retrieve from the pool.</param>
        /// <returns>An object from the pool.</returns>
        public TObject Get(Type keyType)
        {
            TObject value;

            if (this._objectPool.ContainsKey(keyType))
            {
                if (this._objectPool[keyType].Count > 0)
                {
                    value = this._objectPool[keyType].Dequeue();
                    value.Reset();
                    return value;
                }
            }

            value = (TObject)Activator.CreateInstance(keyType);
            value.Reset();
            return value;
        }

        /// <summary>
        /// Adds an object to the pool for reuse.
        /// </summary>
        /// <param name="value">The object to add to the pool.</param>
        public TObject Add(TObject value)
        {
            Type valueType = value.GetType();

            if (!this._objectPool.ContainsKey(valueType))
            {
                this._objectPool.Add(valueType, new());
            }

            this._objectPool[valueType].Enqueue(value);
            return value;
        }
    }
}
