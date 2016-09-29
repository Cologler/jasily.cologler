using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Desktop.Collections.Generic
{
    [TestClass]
    public class UnitTestForList
    {
        [TestMethod]
        public void TestSetValue()
        {
            var array = new[] { 1, 3, 4 };
            array.SetRangeValue(1, new[] { 2, 3 });
            Assert.IsTrue(new[] { 1, 2, 3 }.SequenceEqual(array));
        }
    }
}