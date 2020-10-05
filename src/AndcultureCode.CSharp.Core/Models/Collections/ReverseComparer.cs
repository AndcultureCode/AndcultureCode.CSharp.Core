using System.Collections.Generic;

namespace AndcultureCode.CSharp.Core.Models.Collections
{
    /// <summary>
    /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class ReverseComparer<TKey> : IComparer<TKey>
    {
        #region Private Properties

        private IComparer<TKey> comparer;

        #endregion Private Properties

        #region Constructors

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        public ReverseComparer() : this(Comparer<TKey>.Default) { }

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        /// <param name="comparer"></param>
        public ReverseComparer(IComparer<TKey> comparer)
        {
            this.comparer = comparer;
        }

        #endregion Constructors

        #region IComparer<TKey> Members

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(TKey x, TKey y) => comparer.Compare(y, x);

        #endregion IComparer<TKey> Members
    }
}
