using JetBrains.Annotations;
using System;

namespace Jasily.Diagnostics.AttributeTest
{
    public abstract class TestAttribute : Attribute
    {
        public bool CanNull { get; set; }

        public bool Test(object obj) => obj == null ? this.CanNull : this.TestCore(obj);

        protected abstract bool TestCore([NotNull] object obj);
    }
}
