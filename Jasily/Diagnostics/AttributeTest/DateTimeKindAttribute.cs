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

        protected override bool TestCore(object obj)
            => obj is DateTime && ((DateTime)obj).Kind == this.Kind;
    }
}
