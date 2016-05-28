using System;

namespace Jasily.ComponentModel.Editable
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public abstract class WriteToObjectConditionAttribute : Attribute
    {
        public abstract bool IsMatch(object value);
    }
}