using System.Collections.Generic;

namespace StardustDefender.Collections
{
    internal sealed class ObjectPool<T> where T : IPoolableObject, new()
    {
        private readonly Queue<T> objects = new();

        public T Get()
        {
            T target = default;

            if (this.objects.Count > 0)
            {
                target = this.objects.Dequeue();
                target.Reset();

                return target;
            }
            else
            {
                return target;
            }
        }
        public void ReturnToPool(T obj)
        {
            obj.Reset();
            this.objects.Enqueue(obj);
        }
    }
}