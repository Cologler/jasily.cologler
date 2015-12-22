using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace UnitTest.Desktop.Linq
{
    [TestClass]
    public class UnitTestForEnumerableExtensions
    {
        [TestMethod]
        public void TestForGiveup()
        {
            Assert.AreEqual(9, Enumerable.Range(0, 10).Giveup(1).Count());
            Assert.AreEqual(0, Enumerable.Range(0, 1).Giveup(1).Count());
            Assert.AreEqual(0, Enumerable.Empty<int>().Giveup(1).Count());
            Assert.AreEqual(7, Enumerable.Range(0, 10).Giveup(2).Last());

            Assert.AreEqual(9, Enumerable.Range(0, 10).ToList().Giveup(1).Count());
            Assert.AreEqual(0, Enumerable.Range(0, 1).ToList().Giveup(1).Count());
            Assert.AreEqual(0, Enumerable.Empty<int>().ToList().Giveup(1).Count());
            Assert.AreEqual(7, Enumerable.Range(0, 10).ToList().Giveup(2).Last());

            Assert.AreEqual("234", new string("12345".Skip(1).Giveup(1).ToArray()));
            Assert.AreEqual("234", "12345".Skip(1).Giveup(1).GetString());
        }
    }
}
