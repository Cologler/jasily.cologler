using System;

namespace Jasily.Diagnostics
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class CanBeNullAttribute : TestAttribute
    {
        public CanBeNullAttribute(bool canBeNull)
        {
            this.CanBeNull = canBeNull;
        }

        public bool CanBeNull { get; }

        public override bool Test(object obj)
            => this.CanBeNull || !ReferenceEquals(null, obj);
    }
}
