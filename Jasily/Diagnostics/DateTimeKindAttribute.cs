namespace System.Diagnostics
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class DateTimeKindAttribute : TestAttribute
    {
        public DateTimeKindAttribute(DateTimeKind kind)
        {
            this.Kind = kind;
        }

        public DateTimeKind Kind { get; }

        public override bool Test(object obj)
        {
            if (ReferenceEquals(obj, null))
                return true;

            if (obj is DateTime)
            {
                return ((DateTime)obj).Kind == this.Kind;
            }
            
            var ndt = obj as DateTime?;
            return ndt.HasValue && ndt.Value.Kind == this.Kind;
        }
    }
}
