using System;

namespace Jasily.Reflection
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ReadableMemberAttribute : Attribute
    {
        public ReadableMemberAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}