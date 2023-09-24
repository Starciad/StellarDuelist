using System;
using System.Collections.Generic;

namespace StardustDefender.Collections
{
    internal sealed class ObjectPool<TObject> where TObject : IPoolableObject
    {
        private readonly Dictionary<Type, Queue<TObject>> _objectPool = new();

        public TObject Get<TKey>() where TKey : TObject
        {
            return Get(typeof(TKey));
        }
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

        public void ReturnToPool(TObject value)
        {
            value.Reset();
            Type valueType = value.GetType();

            if (!_objectPool.ContainsKey(valueType))
            {
                this._objectPool.Add(valueType, new());
            }

            this._objectPool[valueType].Enqueue(value);
        }
    }
}