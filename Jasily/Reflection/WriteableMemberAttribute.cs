using System;

namespace Jasily.Reflection
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class WriteableMemberAttribute : Attribute
    {
        public WriteableMemberAttribute(string name = null)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}