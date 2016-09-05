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

        [TestMethod]
        public void Insert()
        {
            var array = new int[0].Insert(0, 1).ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(1, array[0]);

            array = new[] { 2 }.Insert(0, 1).ToArray();
            Assert.AreEqual(2, array.Length);
            Assert.AreEqual(1, array[0]);

            array = new int[0].InsertToStart(1).ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(1, array[0]);

            array = new[] { 2 }.InsertToStart(1).ToArray();
            Assert.AreEqual(2, array.Length);
            Assert.AreEqual(1, array[0]);

            array = new int[0].InsertToEnd(1).ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(1, array[0]);

            array = new[] { 2 }.InsertToEnd(1).ToArray();
            Assert.AreEqual(2, array.Length);
            Assert.AreEqual(1, array[1]);
        }

        [TestMethod]
        public void Set()
        {
            var array = new[] { 2 }.Set(0, 1).ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(1, array[0]);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void Set_ArgumentOutOfRangeException()
        {
            new[] { 2 }.Set(1, 1).ToArray();
        }
    }
}
