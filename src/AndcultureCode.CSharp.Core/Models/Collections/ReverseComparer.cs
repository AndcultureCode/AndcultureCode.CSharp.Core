using System.Collections.Generic;

namespace AndcultureCode.CSharp.Core.Models.Collections
{
    public class ReverseComparer<TKey> : IComparer<TKey>
    {
        #region Private Properties

        private IComparer<TKey> comparer;

        #endregion Private Properties

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ReverseComparer()"/> class.
        /// </summary>
        public ReverseComparer() : this(Comparer<TKey>.Default) { }

        /// <summary>
        /// Creates a new instance of the <see cref="ReverseComparer()"/> class.
        /// </summary>
        /// <param name="comparer"></param>
        public ReverseComparer(IComparer<TKey> comparer)
        {
            this.comparer = comparer;
        }

        #endregion Constructors

        #region IComparer<TKey> Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(TKey x, TKey y) => comparer.Compare(y, x);

        #endregion IComparer<TKey> Members
    }
}
