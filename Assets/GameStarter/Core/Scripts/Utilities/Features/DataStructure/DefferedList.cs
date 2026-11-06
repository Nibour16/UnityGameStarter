using System.Collections.Generic;

namespace UnityGameStarter.DataStructure 
{
    /// <summary>
    /// A variation of list that delays adding/removing items, 
    /// can be applied by user customization into the collection.
    /// 
    /// This is useful when adding into a collection while other operations
    /// may attempt to add or remove items from it.
    /// 
    /// Delaying these changes prevents the collection from being modified
    /// during iteration.
    /// 
    /// Pending additions and removals can cancel each other:
    /// - Adding an item cancels its pending removal.
    /// - Removing an item cancels its pending addition.
    /// 
    /// Note: Removing items will not keep their original order.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DeferredList<T>
    {
        /// <summary>
        /// The main collection
        /// </summary>
        private readonly List<T> _list = new();

        /// <summary>
        /// The addition collection pended
        /// </summary>
        private readonly List<T> _addBuffer = new();

        /// <summary>
        /// The removal collection pended
        /// </summary>
        private readonly HashSet<T> _removeBuffer = new();

        /// <summary>
        /// Property of pended addition collection
        /// </summary>
        public IReadOnlyList<T> PendingAdds => _addBuffer;

        /// <summary>
        /// Property of pended removal collection
        /// </summary>
        public IReadOnlyCollection<T> PendingRemoves => _removeBuffer;

        // Get various Counts from the lists
        public int Count => _list.Count;
        public int PendingAddCount => _addBuffer.Count;
        public int PendingRemoveCount => _removeBuffer.Count;

        /// <summary>
        /// Note that this class is a type of data structure, which this indexer will make it like a real list
        /// </summary>
        public T this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        #region Enqueue registration
        /// <summary>
        /// Add an item into the pending collection
        /// </summary>
        public void EnqueueAdd(T item) 
        {
            _removeBuffer.Remove(item);
            _addBuffer.Add(item);
        }

        /// <summary>
        /// Remove an item into the pending collection
        /// </summary>
        public void EnqueueRemove(T item)
        {
            _addBuffer.Remove(item);
            _removeBuffer.Add(item);
        }
        #endregion

        #region Apply Equeues
        /// <summary>
        /// Apply all pendings
        /// </summary>
        public void ApplyAll()
        {
            ApplyRemovals();
            ApplyAdditions();
        }

        /// <summary>
        /// Apply all pending additions
        /// </summary>
        public void ApplyAdditions() 
        {
            foreach (var item in _addBuffer)
                _list.Add(item);

            _addBuffer.Clear();
        }

        /// <summary>
        /// Apply all pending removals
        /// </summary>
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
        #endregion

        #region Data Clear
        /// <summary>
        /// Clear all pendings
        /// </summary>
        public void ClearPending()
        {
            ClearAdditions();
            ClearRemovals();
        }

        /// <summary>
        /// Clear all pending additions
        /// </summary>
        public void ClearAdditions() => _addBuffer.Clear();

        /// <summary>
        /// Clear all pending removals
        /// </summary>
        public void ClearRemovals() => _removeBuffer.Clear();

        /// <summary>
        /// Clear the list, including the pendings
        /// </summary>
        public void Clear()
        {
            _list.Clear();
            ClearPending();
        }
        #endregion

        public bool Contains(T item) => _list.Contains(item);

        /// <summary>
        /// Check if an item is contained in the pending additions
        /// </summary>
        public bool ContainsAdd(T item) => _addBuffer.Contains(item);

        /// <summary>
        /// Check if an item is contained in the pending removals
        /// </summary>
        public bool ContainsRemove(T item) => _removeBuffer.Contains(item);
    }
}