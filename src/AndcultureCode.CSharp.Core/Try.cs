using System;

namespace AndcultureCode.CSharp.Core
{
    /// <summary>
    /// Static helper class to implement the Try/Catch pattern
    /// </summary>
    public static class Try
    {
        /// <summary>
        /// Executes the given actions and returns whether or not it threw an exception.
        /// </summary>
        /// <remarks>WARNING: it hides the thrown exception</remarks>
        /// <param name="action"></param>
        /// <returns><c>true</c> if action completed, <c>false</c> otherwise</returns>
        public static bool Catch(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}