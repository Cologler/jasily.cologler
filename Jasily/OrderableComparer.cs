using System.Collections.Generic;

namespace Jasily
{
    public class OrderableComparer : Comparer<IOrderable>
    {
        #region Overrides of Comparer<IOrderable>

        /// <summary>
        /// 比较两个对象并返回一个值，该值指示一个对象小于、等于还是大于另一个对象。
        /// </summary>
        /// <returns>
        /// 一个有符号整数，指示 <paramref name="x"/> 与 <paramref name="y"/> 的相对值，如下表所示。 
        /// 值 	含义 	
        /// 小于零 	<paramref name="x"/> 小于 <paramref name="y"/>。 	
        /// 零 	<paramref name="x"/> 等于 <paramref name="y"/>。 	
        /// 大于零 	<paramref name="x"/> 大于 <paramref name="y"/>。
        /// </returns>
        /// <param name="x">要比较的第一个对象。</param><param name="y">要比较的第二个对象。</param>
        public override int Compare(IOrderable x, IOrderable y) => x.GetOrderCode().CompareTo(y.GetOrderCode());

        #endregion
    }
}