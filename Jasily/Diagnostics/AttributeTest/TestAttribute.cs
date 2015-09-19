using System;

namespace Jasily.Diagnostics.AttributeTest
{
    public abstract class TestAttribute : Attribute
    {
        public bool CanNull { get; set; }

        public abstract bool Test(object obj);
    }
}
