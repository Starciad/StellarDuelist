using System;
using System.Collections.Generic;

namespace StardustDefender.Core.Collections
{
    /// <summary>
    /// A generic object pool for managing reusable objects that implement the <see cref="IPoolableObject"/> interface.
    /// </summary>
    /// <typeparam name="TObject">The type of objects stored in the pool.</typeparam>
    public sealed class ObjectPool<TObject> where TObject : IPoolableObject
    {
        private readonly Dictionary<Type, Queue<TObject>> _objectPool = new();

        /// <summary>
        /// Gets an object of type <typeparamref name="TKey"/> from the pool, if available.
        /// </summary>
        /// <typeparam name="TKey">The type of object to retrieve from the pool.</typeparam>
        /// <returns>An object from the pool, or the default value of <typeparamref name="TObject"/> if the pool is empty.</returns>
        public TObject Get<TKey>() where TKey : TObject
        {
            return Get(typeof(TKey));
        }

        /// <summary>
        /// Gets an object of the specified type from the pool, if available.
        /// </summary>
        /// <param name="keyType">The type of object to retrieve from the pool.</param>
        /// <returns>An object from the pool, or the default value of <typeparamref name="TObject"/> if the pool is empty.</returns>
        public TObject Get(Type keyType)
        {
            TObject target = default;

            if (this._objectPool.ContainsKey(keyType))
            {
                Queue<TObject> objects = this._objectPool[keyType];
                if (objects.Count > 0)
                {
                    target = objects.Dequeue();
                    target.Reset();
                    return target;
                }
            }

            return target;
        }

        /// <summary>
        /// Returns an object to the pool for reuse.
        /// </summary>
        /// <param name="value">The object to return to the pool.</param>
        public void ReturnToPool(TObject value)
        {
            value.Reset();
            Type valueType = value.GetType();

            if (!this._objectPool.ContainsKey(valueType))
            {
                this._objectPool.Add(valueType, new());
            }

            this._objectPool[valueType].Enqueue(value);
        }
    }
}
