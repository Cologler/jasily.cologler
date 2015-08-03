using System;

namespace Jasily.Reflection
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ReadableMemberAttribute : Attribute
    {
        public ReadableMemberAttribute()
        {
        }

        public ReadableMemberAttribute(string name = null)
            : this()
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}