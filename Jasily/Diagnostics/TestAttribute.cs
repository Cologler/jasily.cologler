using System;

namespace Jasily.Diagnostics
{
    public abstract class TestAttribute : Attribute
    {
        public abstract bool Test(object obj);
    }
}
