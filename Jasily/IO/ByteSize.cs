using System;

namespace Jasily.IO
{
    public struct ByteSize
    {
        public long OriginValue { get; }

        public ByteSizeType Unit { get; }

        public double Value { get; }

        public ByteSize(ByteSizeType unit, long originValue)
            : this()
        {
            this.Value = Convert.ToDouble(this.OriginValue);
            var level = (int)unit - (int)ByteSizeType.Byte;
            while (level > 0)
            {
                level--;
                this.Value /= 1024;
            }

            this.OriginValue = originValue;
            this.Unit = unit;
        }

        public ByteSize(long originValue)
        {
            var l = Convert.ToDouble(originValue);
            var level = 0;
            while (l > 1024)
            {
                level++;
                l /= 1024;
            }

            this.OriginValue = originValue;
            this.Value = l;
            this.Unit = (ByteSizeType)level;
        }

        /// <summary>
        /// 返回该实例的完全限定类型名。
        /// </summary>
        /// <returns>
        /// 包含完全限定类型名的 <see cref="T:System.String"/>。
        /// </returns>
        public override string ToString() => string.Format("{0,-6:##0.000} {1}", this.Value, this.Unit);

        public ByteSize ConvertTo(ByteSizeType unit)
        {
            if (this.Unit == unit) return this;
            return new ByteSize(unit, this.OriginValue);
        }

        public ByteSize ToResize() => new ByteSize(this.OriginValue);

        public static implicit operator ByteSize(long length) => new ByteSize(length);
    }
}