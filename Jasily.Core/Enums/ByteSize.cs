namespace System.Enums
{
    public struct ByteSize
    {
        private readonly ByteSizeType unit;
        private readonly double value;
        private readonly long originValue;

        public long OriginValue
        {
            get { return this.originValue; }
        }

        public ByteSizeType Unit
        {
            get { return this.unit; }
        }

        public double Value
        {
            get { return this.value; }
        }

        public ByteSize(ByteSizeType unit, double value, long originValue)
            : this()
        {
            this.value = value;
            this.originValue = originValue;
            this.unit = unit;
        }

        /// <summary>
        /// 返回该实例的完全限定类型名。
        /// </summary>
        /// <returns>
        /// 包含完全限定类型名的 <see cref="T:System.String"/>。
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0,-6:###.000} {1}", this.Value, this.Unit);
        }
    }
}