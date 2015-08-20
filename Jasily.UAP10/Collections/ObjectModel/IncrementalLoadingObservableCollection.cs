using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace Jasily.Collections.ObjectModel
{
    public abstract class IncrementalLoadingObservableCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        /// <summary>
        /// 初始化 <see cref="T:System.Collections.ObjectModel.IncrementalLoadingObservableCollection"/> 类的新实例。
        /// </summary>
        public IncrementalLoadingObservableCollection()
        {
        }

        /// <summary>
        /// 初始化 <see cref="T:System.Collections.ObjectModel.IncrementalLoadingObservableCollection"/> 类的新实例，该类包含从指定集合中复制的元素。
        /// </summary>
        /// <param name="collection">从中复制元素的集合。</param><exception cref="T:System.ArgumentNullException"><paramref name="collection"/> 参数不能为 null。</exception>
        public IncrementalLoadingObservableCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// 初始化从视图的增量加载。
        /// </summary>
        /// <returns>
        /// 加载操作的换行结果。
        /// </returns>
        /// <param name="count">要加载的项的数目。</param>
        public abstract IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count);

        /// <summary>
        /// 获取支持增量加载实现的 Sentinel 值。
        /// </summary>
        /// <returns>
        /// 如果附加卸载项保留在视图中，则为 true；否则为 false。
        /// </returns>
        public abstract bool HasMoreItems { get; protected set; }
    }
}