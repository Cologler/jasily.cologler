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
        /// ��ʼ������ͼ���������ء�
        /// </summary>
        /// <returns>
        /// ���ز����Ļ��н����
        /// </returns>
        /// <param name="count">Ҫ���ص������Ŀ��</param>
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
        /// ��ȡ֧����������ʵ�ֵ� Sentinel ֵ��
        /// </summary>
        /// <returns>
        /// �������ж���������ͼ�У���Ϊ true������Ϊ false��
        /// </returns>
        public override bool HasMoreItems { get; protected set; }
    }
}