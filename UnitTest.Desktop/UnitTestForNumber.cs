using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTest.Desktop
{
    [TestClass]
    public class UnitTestForNumber
    {
        [TestMethod]
        public void TestMethodForTryBinarySqrt()
        {
            foreach (var i in Enumerable.Range(0, 256))
            {
                if (i == 1)
                    Assert.AreEqual(i.TryBinarySqrt(), 0);
                else if (i == 2)
                    Assert.AreEqual(i.TryBinarySqrt(), 1);
                else if (i == 4)
                    Assert.AreEqual(i.TryBinarySqrt(), 2);
                else if (i == 8)
                    Assert.AreEqual(i.TryBinarySqrt(), 3);
                else if (i == 16)
                    Assert.AreEqual(i.TryBinarySqrt(), 4);
                else if (i == 32)
                    Assert.AreEqual(i.TryBinarySqrt(), 5);
                else if (i == 64)
                    Assert.AreEqual(i.TryBinarySqrt(), 6);
                else if (i == 128)
                    Assert.AreEqual(i.TryBinarySqrt(), 7);
                else
                    Assert.AreEqual(i.TryBinarySqrt(), null);
            }
        }

        [TestMethod]
        public void TestMethodForSplitHighestPlace()
        {
            // x10
            var t = 10.SplitHighestPlace(10);
            Assert.AreEqual(1, t.Item1);
            Assert.AreEqual(0, t.Item2);

            t = 124.SplitHighestPlace(10);
            Assert.AreEqual(1, t.Item1);
            Assert.AreEqual(24, t.Item2);

            t = 12.4.SplitHighestPlace(10);
            Assert.AreEqual(1, t.Item1);
            Assert.AreEqual(2, t.Item2);

            // x2
            t = 2.SplitHighestPlace(2); // 10
            Assert.AreEqual(1, t.Item1);
            Assert.AreEqual(0, t.Item2);

            // x8
            t = 8.SplitHighestPlace(8); // 10
            Assert.AreEqual(1, t.Item1);
            Assert.AreEqual(0, t.Item2);

            t = 9.SplitHighestPlace(8); // 11
            Assert.AreEqual(1, t.Item1);
            Assert.AreEqual(1, t.Item2);

            // x16
            t = 16.SplitHighestPlace(16); // 10
            Assert.AreEqual(1, t.Item1);
            Assert.AreEqual(0, t.Item2);

            t = 0x1441.SplitHighestPlace(16);
            Assert.AreEqual(1, t.Item1);
            Assert.AreEqual(0x441, t.Item2);
        }
    }
}