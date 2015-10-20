using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace Jasily.Collections.ObjectModel
{
    public class SimpleIncrementalLoadingObservableCollection<T> : IncrementalLoadingObservableCollection<T>
    {
        public SimpleIncrementalLoadingObservableCollection(Func<uint, Task<IncrementalLoadingResult<T>>> incrementalLoadingCallback)
        {
            if (incrementalLoadingCallback == null)
                throw new ArgumentNullException(nameof(incrementalLoadingCallback));

            this.IncrementalLoadingCallback = incrementalLoadingCallback;
        }

        public Func<uint, Task<IncrementalLoadingResult<T>>> IncrementalLoadingCallback { get; set; }

        /// <summary>
        /// 初始化从视图的增量加载。
        /// </summary>
        /// <returns>
        /// 加载操作的换行结果。
        /// </returns>
        /// <param name="count">要加载的项的数目。</param>
        public override IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run(async () =>
            {
                var callback = this.IncrementalLoadingCallback;

                if (callback == null)
                {
                    if (Debugger.IsAttached)
                        Debugger.Break();
                    return new LoadMoreItemsResult() { Count = 0 };
                }

                var result = await callback(count);
                this.AddRange(result.Items);
                this.HasMoreItems = result.HasMoreItems;
                return result.Result;

            }).AsAsyncOperation();
        }

        /// <summary>
        /// 获取支持增量加载实现的 Sentinel 值。
        /// </summary>
        /// <returns>
        /// 如果附加卸载项保留在视图中，则为 true；否则为 false。
        /// </returns>
        public override bool HasMoreItems { get; protected set; }
    }
}