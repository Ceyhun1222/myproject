using System;
using System.Collections.Generic;

namespace PVT.Graphics
{
    public abstract class DrawOperation<T>
    {

        public abstract void Draw(T obj);
        public abstract void Clean(Guid id);
        public abstract void Clean();
    }

    public class DrawObject
    {
        private readonly List<int> _handlers;
        public Guid Id { get; }

        public DrawObject(Guid id)
        {
            Id = id;
            _handlers = new List<int>();
        }

        public int Count => _handlers.Count;

        public void Add(int handler)
        {
            _handlers.Add(handler);
        }

        public int Get(int index)
        {
            return _handlers[index];
        }

        public void Clear()
        {
            _handlers.Clear();
        }


    }
}
