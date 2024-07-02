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

        public int Length
        {
            get { return _objects.Count; }
        }

        public T GetObject()
        {
            T item = default;

            if (Length > 0)
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

        public void ClearPool()
        {
            while (Length > 0)
            {
                _objects.Dequeue();
            }
        }
    }
}
