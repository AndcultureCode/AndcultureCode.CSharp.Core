using System.Collections.Generic;

namespace AndcultureCode.CSharp.Core.Models.Collections
{
    public class OrderedList<TKey, TValue> : IDictionary<TKey, ICollection<TValue>>, IEnumerable<TValue>
    {
        #region Private Properties

        private IDictionary<TKey, ICollection<TValue>> items;

        #endregion Private Properties

        #region Constructors

        public OrderedList()
        {
            items = new SortedDictionary<TKey, ICollection<TValue>>();
        }

        public OrderedList(IComparer<TKey> comparer)
        {
            items = new SortedDictionary<TKey, ICollection<TValue>>(comparer);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            ICollection<TValue> values;
            if (!items.TryGetValue(key, out values))
            {
                items.Add(key, values = new List<TValue>());
            }

            values.Add(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Remove(TKey key, TValue value)
        {
            if (EqualityComparer<TKey>.Default.Equals(key, default(TKey)))
            {
                foreach (var item in items)
                {
                    if (item.Value.Remove(value))
                    {
                        break;
                    }
                }

                return;
            }

            if (EqualityComparer<TValue>.Default.Equals(value, default(TValue)))
            {
                items.Remove(key);
                return;
            }

            ICollection<TValue> values;
            if (items.TryGetValue(key, out values))
            {
                values.Remove(value);
                if (values.Count == 0)
                {
                    items.Remove(key);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TValue> GetEnumerator()
        {
            foreach (KeyValuePair<TKey, ICollection<TValue>> values in items)
            {
                foreach (TValue value in values.Value)
                {
                    yield return value;
                }
            }
        }

        #region IDictionary<TKey,IEnumerable<TValue>> Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void IDictionary<TKey, ICollection<TValue>>.Add(TKey key, ICollection<TValue> value) => items.Add(key, value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key) => items.ContainsKey(key);

        /// <summary>
        /// 
        /// </summary>
        public ICollection<TKey> Keys { get => items.Keys; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IDictionary<TKey, ICollection<TValue>>.Remove(TKey key) => items.Remove(key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out ICollection<TValue> value) => items.TryGetValue(key, out value);

        /// <summary>
        /// 
        /// </summary>
        ICollection<ICollection<TValue>> IDictionary<TKey, ICollection<TValue>>.Values { get => items.Values; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ICollection<TValue> this[TKey key]
        {
            get => items[key];
            set => items[key] = value;
        }

        #endregion IDictionary<TKey,IEnumerable<TValue>> Members

        #region ICollection<KeyValuePair<TKey,IEnumerable<TValue>>> Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void ICollection<KeyValuePair<TKey, ICollection<TValue>>>.Add(KeyValuePair<TKey, ICollection<TValue>> item) => items.Add(item);

        /// <summary>
        /// 
        /// </summary>
        public void Clear() => items.Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool ICollection<KeyValuePair<TKey, ICollection<TValue>>>.Contains(KeyValuePair<TKey, ICollection<TValue>> item) => items.Contains(item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        void ICollection<KeyValuePair<TKey, ICollection<TValue>>>.CopyTo(KeyValuePair<TKey, ICollection<TValue>>[] array, int arrayIndex)
            => items.CopyTo(array, arrayIndex);

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count { get => items.Count; }

        /// <summary>
        /// Gets if the instance is readonly.
        /// </summary>
        public bool IsReadOnly { get => items.IsReadOnly; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool ICollection<KeyValuePair<TKey, ICollection<TValue>>>.Remove(KeyValuePair<TKey, ICollection<TValue>> item) => items.Remove(item);

        #endregion ICollection<KeyValuePair<TKey,IEnumerable<TValue>>> Members

        #region IEnumerable<KeyValuePair<TKey,IEnumerable<TValue>>> Members

        IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> IEnumerable<KeyValuePair<TKey, ICollection<TValue>>>.GetEnumerator()
            => items.GetEnumerator();

        #endregion IEnumerable<KeyValuePair<TKey,IEnumerable<TValue>>> Members

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => items.GetEnumerator();

        #endregion IEnumerable Members

        #endregion Public Methods
    }
}