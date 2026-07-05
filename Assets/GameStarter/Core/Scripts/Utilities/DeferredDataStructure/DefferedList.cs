using System.Collections.Generic;

namespace UnityGameStarter.DefferedDataStructure 
{
    public class DefferedList<T>
    {
        private readonly List<T> _list = new();

        private readonly List<T> _addBuffer = new();
        private readonly HashSet<T> _removeBuffer = new();

        public int Count => _list.Count;
        public int PendingAddCount => _addBuffer.Count;
        public int PendingRemoveCount => _removeBuffer.Count;

        public T this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        public void EnqueueAdd(T item) 
        {
            _removeBuffer.Remove(item);
            _addBuffer.Add(item);
        }

        public void EnqueueRemove(T item)
        {
            _addBuffer.Remove(item);
            _removeBuffer.Add(item);
        }

        public void Flush()
        {
            ApplyRemovals();
            ApplyAdditions();
        }

        public void ApplyAdditions() 
        {
            foreach (var item in _addBuffer)
                _list.Add(item);

            _addBuffer.Clear();
        }

        public void ApplyRemovals() 
        {
            foreach (var item in _removeBuffer)
            {
                int index = _list.IndexOf(item);
                if (index < 0) continue;

                int last = _list.Count - 1;

                _list[index] = _list[last];
                _list.RemoveAt(last);
            }

            _removeBuffer.Clear();
        }

        public bool Contains(T item) => _list.Contains(item);
        public bool ContainsAdd(T item) => _addBuffer.Contains(item);
        public bool ContainsRemove(T item) => _removeBuffer.Contains(item);

        public void ClearPending()
        {
            _addBuffer.Clear();
            _removeBuffer.Clear();
        }

        public void Clear() 
        {
            _list.Clear();
            ClearPending();
        }
    }
}

