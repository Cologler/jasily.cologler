using System;

namespace Jasily.IO
{
    public struct ByteSize
    {
        public long OriginValue { get; }

        public ByteSizeType Unit { get; }

        public double Value { get; }

        public ByteSize(ByteSizeType unit, double value, long originValue)
            : this()
        {
            this.Value = value;
            this.OriginValue = originValue;
            this.Unit = unit;
        }

        /// <summary>
        /// 返回该实例的完全限定类型名。
        /// </summary>
        /// <returns>
        /// 包含完全限定类型名的 <see cref="T:System.String"/>。
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0,-6:##0.000} {1}", this.Value, this.Unit);
        }
    }
}