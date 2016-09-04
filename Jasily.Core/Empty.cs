using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace System
{
    public static class Empty<T>
    {
        public static readonly T[] Array = (T[])Linq.Enumerable.Empty<T>();

        public static IEnumerable<T> Enumerable => Linq.Enumerable.Empty<T>();

        static Empty()
        {
            Debug.Assert(Array != null);
            Debug.Assert(Enumerable != null);
        }

        public struct EmptyEnumerable : IEnumerable<T>
        {
            public EmptyEnumerator GetEnumerator() => new EmptyEnumerator();

            #region Implementation of IEnumerable

            /// <summary>����һ��ѭ�����ʼ��ϵ�ö������</summary>
            /// <returns>������ѭ�����ʼ��ϵ� <see cref="T:System.Collections.Generic.IEnumerator`1" />��</returns>
            IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();

            /// <summary>����һ��ѭ�����ʼ��ϵ�ö������</summary>
            /// <returns>������ѭ�����ʼ��ϵ� <see cref="T:System.Collections.IEnumerator" /> ����</returns>
            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            #endregion

            public struct EmptyEnumerator : IEnumerator<T>
            {
                #region Implementation of IEnumerator

                /// <summary>��ö�����ƽ������ϵ���һ��Ԫ�ء�</summary>
                /// <returns>���ö�����ɹ����ƽ�����һ��Ԫ�أ���Ϊ true�����ö����Խ�����ϵĽ�β����Ϊ false��</returns>
                /// <exception cref="T:System.InvalidOperationException">�ڴ�����ö�����󼯺ϱ��޸��ˡ�</exception>
                public bool MoveNext() => false;

                /// <summary>��ö��������Ϊ���ʼλ�ã���λ��λ�ڼ����е�һ��Ԫ��֮ǰ��</summary>
                /// <exception cref="T:System.InvalidOperationException">�ڴ�����ö�����󼯺ϱ��޸��ˡ�</exception>
                public void Reset() { }

                /// <summary>��ȡ������λ��ö������ǰλ�õ�Ԫ�ء�</summary>
                /// <returns>������λ��ö������ǰλ�õ�Ԫ�ء�</returns>
                public T Current => default(T);

                /// <summary>��ȡ�����еĵ�ǰԪ�ء�</summary>
                /// <returns>�����еĵ�ǰԪ�ء�</returns>
                object IEnumerator.Current => this.Current;

                #endregion

                #region Implementation of IDisposable

                /// <summary>ִ�����ͷŻ����÷��й���Դ��ص�Ӧ�ó����������</summary>
                public void Dispose() { }

                #endregion
            }
        }
    }

    public static class Empty
    {
        #region null -> []

        public static T[] EmptyIfNull<T>(this T[] array) => array ?? Empty<T>.Array;

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumerable)
            => enumerable ?? Empty<T>.Enumerable;

        public static string EmptyIfNull<T>(this string str) => str ?? string.Empty;

        #endregion

        #region [] -> null

        [CanBeNull]
        public static List<T> NullIfEmpty<T>([CanBeNull] this List<T> item) => NullIfEmpty<List<T>, T>(item);

        [CanBeNull]
        public static T[] NullIfEmpty<T>([CanBeNull] this T[] item) => NullIfEmpty<T[], T>(item);

        [CanBeNull]
        public static TCollection NullIfEmpty<TCollection, T>([CanBeNull] TCollection item)
            where TCollection : class, ICollection<T>
            => item == null || item.Count == 0 ? null : item;

        #endregion
    }
}