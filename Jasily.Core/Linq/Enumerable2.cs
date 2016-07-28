using System.Collections.Generic;
using JetBrains.Annotations;

namespace System.Linq
{
    /// <summary>
    /// let static IEnumerable&lt;T&gt; Func&lt;T, T2&gt;(this IEnumerable&lt;T&gt; source) => 
    /// IEnumerable&lt;T&gt; Func&lt;T2&gt;()
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Enumerable2<T>
    {
        private readonly IEnumerable<T> baseEnumerable;

        public Enumerable2(IEnumerable<T> baseEnumerable)
        {
            // can be null if next func accept null argument.
            this.baseEnumerable = baseEnumerable;
        }

        public IEnumerable<T> Ignore<TException>() where TException : Exception
            => this.baseEnumerable.Ignore<T, TException>();

        public IEnumerable<T> Ignore<TException>([NotNull] Func<TException, bool> exceptionFilter)
            where TException : Exception
            => this.baseEnumerable.Ignore<T, TException>(exceptionFilter);
    }
}