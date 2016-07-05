using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Desktop.Linq
{
    [TestClass]
    public class UnitTestForEnumerableExtensions
    {
        [TestMethod]
        public void TestForGiveup()
        {
            Assert.AreEqual(9, Enumerable.Range(0, 10).GiveUp(1).Count());
            Assert.AreEqual(0, Enumerable.Range(0, 1).GiveUp(1).Count());
            Assert.AreEqual(0, Enumerable.Empty<int>().GiveUp(1).Count());
            Assert.AreEqual(7, Enumerable.Range(0, 10).GiveUp(2).Last());

            Assert.AreEqual(9, Enumerable.Range(0, 10).ToList().GiveUp(1).Count());
            Assert.AreEqual(0, Enumerable.Range(0, 1).ToList().GiveUp(1).Count());
            Assert.AreEqual(0, Enumerable.Empty<int>().ToList().GiveUp(1).Count());
            Assert.AreEqual(7, Enumerable.Range(0, 10).ToList().GiveUp(2).Last());

            Assert.AreEqual("234", new string("12345".Skip(1).GiveUp(1).ToArray()));
            Assert.AreEqual("234", "12345".Skip(1).GiveUp(1).GetString());
        }
    }
}
