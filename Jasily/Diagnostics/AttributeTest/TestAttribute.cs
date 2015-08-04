using System;

namespace Jasily.Diagnostics.AttributeTest
{
    public abstract class TestAttribute : Attribute
    {
        public abstract bool Test(object obj);
    }
}
