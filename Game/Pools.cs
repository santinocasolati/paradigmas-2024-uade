using System;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public class ObjectPool<T>
    {
        private Queue<T> _objects;
        private Func<T> _objectGenerator;

        public ObjectPool(Func<T> objectGenerator)
        {
            _objects = new Queue<T>();
            _objectGenerator = objectGenerator;
        }

        public T GetObject()
        {
            T item = default;

            if (_objects.Count > 0)
            {
                item = _objects.Dequeue();
            } else {
                item = _objectGenerator();
            }

            return item;
        }

        public void ReleaseObject(T obj)
        {
            _objects.Enqueue(obj);
        }
    }
}
