using System.Collections.Generic;
using System.Threading;

namespace System.Linq
{
    public static class JasilyLinq
    {
        /// <summary>
        /// 从 System.Collections.Generic.IEnumerable&lt;T&gt; 创建一个数组。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">要从其创建数组的 System.Collections.Generic.IEnumerable&lt;T&gt;。</param>
        /// <param name="token">取消标记</param>
        /// <exception cref="System.OperationCanceledException">此任务被取消</exception>
        /// <exception cref="System.ArgumentNullException">source 为 null。</exception>
        /// <returns>一个包含输入序列中的元素的数组。</returns>
        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source, CancellationToken token)
        {
            return source.ToList(token).ToArray();
        }

        /// <summary>
        /// 根据指定的键选择器函数，从 System.Collections.Generic.IEnumerable&lt;T&gt; 创建一个 System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <typeparam name="TKey">keySelector 返回的键的类型。</typeparam>
        /// <typeparam name="TElement">elementSelector 返回的值的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;T&gt;，将从它创建一个 System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;。</param>
        /// <param name="keySelector">用于从每个元素中提取键的函数。</param>
        /// <param name="token">取消标记</param>
        /// <exception cref="System.OperationCanceledException">此任务被取消</exception>
        /// <exception cref="System.ArgumentNullException">source 或 keySelector 或 elementSelector 为 null。 - 或 - keySelector 产生了一个 null 键。</exception>
        /// <exception cref="System.ArgumentException">keySelector 为两个元素产生了重复键。</exception>
        /// <returns>一个 System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;，包含从输入序列中选择的类型为 TElement的值。</returns>
        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken token)
        {
            var result = new Dictionary<TKey, TSource>();
            foreach (var item in source)
            {
                token.ThrowIfCancellationRequested();
                result.Add(keySelector(item), item);
            }
            return result;
        }
        /// <summary>
        /// 根据指定的键选择器和元素选择器函数，从 System.Collections.Generic.IEnumerable&lt;T&gt; 创建一个 System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <typeparam name="TKey">keySelector 返回的键的类型。</typeparam>
        /// <typeparam name="TElement">elementSelector 返回的值的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;T&gt;，将从它创建一个 System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;。</param>
        /// <param name="keySelector">用于从每个元素中提取键的函数。</param>
        /// <param name="elementSelector">用于从每个元素产生结果元素值的转换函数。</param>
        /// <param name="token">取消标记</param>
        /// <exception cref="System.OperationCanceledException">此任务被取消</exception>
        /// <exception cref="System.ArgumentNullException">source 或 keySelector 或 elementSelector 为 null。 - 或 - keySelector 产生了一个 null 键。</exception>
        /// <exception cref="System.ArgumentException">keySelector 为两个元素产生了重复键。</exception>
        /// <returns>一个 System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;，包含从输入序列中选择的类型为 TElement的值。</returns>
        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken token)
        {
            var result = new Dictionary<TKey, TElement>();
            foreach (var item in source)
            {
                token.ThrowIfCancellationRequested();
                result.Add(keySelector(item), elementSelector(item));
            }
            return result;
        }
        /// <summary>
        /// 根据指定的键选择器函数、比较器函数从 System.Collections.Generic.IEnumerable&lt;T&gt; 创建一个 System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <typeparam name="TKey">keySelector 返回的键的类型。</typeparam>
        /// <typeparam name="TElement">elementSelector 返回的值的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;T&gt;，将从它创建一个 System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;。</param>
        /// <param name="keySelector">用于从每个元素中提取键的函数。</param>
        /// <param name="comparer">一个用于对键进行比较的 System.Collections.Generic.IEqualityComparer&lt;T&gt;。</param>
        /// <param name="token">取消标记</param>
        /// <exception cref="System.OperationCanceledException">此任务被取消</exception>
        /// <exception cref="System.ArgumentNullException">source 或 keySelector 或 elementSelector 为 null。 - 或 - keySelector 产生了一个 null 键。</exception>
        /// <exception cref="System.ArgumentException">keySelector 为两个元素产生了重复键。</exception>
        /// <returns>一个 System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;，包含从输入序列中选择的类型为 TElement的值。</returns>
        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken token)
        {
            var result = new Dictionary<TKey, TSource>(comparer);
            foreach (var item in source)
            {
                token.ThrowIfCancellationRequested();
                result.Add(keySelector(item), item);
            }
            return result;
        }
        /// <summary>
        /// 根据指定的键选择器函数、比较器和元素选择器函数从 System.Collections.Generic.IEnumerable&lt;T&gt; 创建一个 System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <typeparam name="TKey">keySelector 返回的键的类型。</typeparam>
        /// <typeparam name="TElement">elementSelector 返回的值的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;T&gt;，将从它创建一个 System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;。</param>
        /// <param name="keySelector">用于从每个元素中提取键的函数。</param>
        /// <param name="elementSelector">用于从每个元素产生结果元素值的转换函数。</param>
        /// <param name="comparer">一个用于对键进行比较的 System.Collections.Generic.IEqualityComparer&lt;T&gt;。</param>
        /// <param name="token">取消标记</param>
        /// <exception cref="System.OperationCanceledException">此任务被取消</exception>
        /// <exception cref="System.ArgumentNullException">source 或 keySelector 或 elementSelector 为 null。 - 或 - keySelector 产生了一个 null 键。</exception>
        /// <exception cref="System.ArgumentException">keySelector 为两个元素产生了重复键。</exception>
        /// <returns>一个 System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;，包含从输入序列中选择的类型为 TElement的值。</returns>
        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken token)
        {
            var result = new Dictionary<TKey, TElement>(comparer);
            foreach (var item in source)
            {
                token.ThrowIfCancellationRequested();
                result.Add(keySelector(item), elementSelector(item));
            }
            return result;
        }
        
        /// <summary>
        /// 从 System.Collections.Generic.IEnumerable&lt;T&gt; 创建一个 System.Collections.Generic.List&lt;T&gt;。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">要从其创建 System.Collections.Generic.List&lt;T&gt; 的 System.Collections.Generic.IEnumerable&lt;T&gt;。</param>
        /// <param name="token">取消标记</param>
        /// <exception cref="System.OperationCanceledException">此任务被取消</exception>
        /// <returns></returns>
        public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source, CancellationToken token)
        {
            var result = new List<TSource>();
            foreach (var item in source)
            {
                token.ThrowIfCancellationRequested();
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// 从 System.Collections.Generic.IEnumerable&lt;T&gt; 创建指定步长的多个 System.Collections.Generic.IEnumerable&lt;T&gt;
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">要从其创建多个 System.Collections.Generic.IEnumerable&lt;T&gt; 的 System.Collections.Generic.IEnumerable&lt;T&gt;。</param>
        /// <param name="chunkSize">步长</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source, int chunkSize)
        {
            return Split(source, chunkSize, source.Count());
        }

        /// <summary>
        /// 从 System.Collections.Generic.IEnumerable&lt;T&gt; 创建指定步长的多个 System.Collections.Generic.IEnumerable&lt;T&gt;
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">要从其创建多个 System.Collections.Generic.IEnumerable&lt;T&gt; 的 System.Collections.Generic.IEnumerable&lt;T&gt;。</param>
        /// <param name="chunkSize">步长</param>
        /// <param name="count">source 元素中的数量。使用此参数有助于提高性能。</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source, int chunkSize, int count)
        {
            var len = count / chunkSize + (count % chunkSize != 0 ? 1 : 0);
            return Enumerable.Range(0, len).Select(i => source.Skip(i * chunkSize).Take(chunkSize));
        }

        /// <summary>
        /// return a single item array.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> GetIEnumerable<TSource>(this TSource source)
        {
            return new[] {source};
        }
    }
}
