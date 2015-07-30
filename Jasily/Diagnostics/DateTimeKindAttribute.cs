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

            return obj.TryCast<DateTime>()?.Select(z => z.Kind == this.Kind) ?? false;
        }
    }
}
