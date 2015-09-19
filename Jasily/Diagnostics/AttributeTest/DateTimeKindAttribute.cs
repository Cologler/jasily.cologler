using System;

namespace Jasily.Diagnostics.AttributeTest
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class DateTimeKindAttribute : TestAttribute
    {
        public DateTimeKindAttribute(DateTimeKind kind)
        {
            this.Kind = kind;
        }

        public DateTimeKind Kind { get; }

        public bool Test(DateTime dt)
            => dt.CastWith(z => z.Kind == this.Kind);

        public override bool Test(object obj)
            => this.CanNull && ReferenceEquals(obj, null) || obj is DateTime && this.Test((DateTime) obj);
    }
}
