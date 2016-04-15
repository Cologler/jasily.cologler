using Jasily.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Desktop.Collections.Generic
{
    [TestClass]
    public class UnitTestForRangeDictionary
    {
        [TestMethod]
        public void TestCriticalValue()
        {
            var a = new CriticalValue<int>(5, true);
            var b = new CriticalValue<int>(4, true);
            Assert.IsTrue(a > b);
            Assert.IsTrue(b < a);
            Assert.IsTrue(b != a);
            Assert.IsTrue(a != b);
            Assert.IsTrue(a >= b);
            Assert.IsTrue(b <= a);
            Assert.IsTrue(a.CompareTo(b) > 0);

            var c = new CriticalValue<int>(5, false);
            Assert.IsTrue(a > c);
            Assert.IsTrue(c < a);
            Assert.IsTrue(c != a);
            Assert.IsTrue(a != c);
            Assert.IsTrue(a >= c);
            Assert.IsTrue(c <= a);
            Assert.IsTrue(a.CompareTo(c) > 0);

            var d = new CriticalValue<int>(5, true);
            Assert.IsFalse(a > d);
            Assert.IsFalse(d < a);
            Assert.IsFalse(d != a);
            Assert.IsFalse(a != d);
            Assert.IsTrue(a >= d);
            Assert.IsTrue(d <= a);
            Assert.IsTrue(a.CompareTo(d) == 0);
        }

        [TestMethod]
        public void TestRange()
        {
            var range = new Range<int>(0, 5, RangeMode.IncludeMin);
            Assert.AreEqual("[0, 5)", range.ToString());

            Assert.AreEqual(false, range.Contains(-1));
            Assert.AreEqual(true, range.Contains(0));
            Assert.AreEqual(true, range.Contains(1));
            Assert.AreEqual(true, range.Contains(2));
            Assert.AreEqual(true, range.Contains(3));
            Assert.AreEqual(true, range.Contains(4));
            Assert.AreEqual(false, range.Contains(5));

            Assert.AreEqual(1, range.CompareTo(-1));
            Assert.AreEqual(0, range.CompareTo(0));
            Assert.AreEqual(0, range.CompareTo(1));
            Assert.AreEqual(0, range.CompareTo(2));
            Assert.AreEqual(0, range.CompareTo(3));
            Assert.AreEqual(0, range.CompareTo(4));
            Assert.AreEqual(-1, range.CompareTo(5));

            Assert.AreEqual(true, new Range<int>(0, 5, RangeMode.IncludeMin) > new Range<int>(-1));
            Assert.AreEqual(false, new Range<int>(0, 5, RangeMode.IncludeMin) > new Range<int>(0));
            Assert.AreEqual(false, new Range<int>(0, 5, RangeMode.IncludeMin) < new Range<int>(-1));
            Assert.AreEqual(true, new Range<int>(0, 5, RangeMode.IncludeMin) < new Range<int>(5));
        }

        [TestMethod]
        public void TestRangeDictionary()
        {
            var dict = RangeDictionary.Create<int, string>(new Range<int>(int.MinValue, int.MaxValue));
            dict.Add(new Range<int>(0, 5, RangeMode.IncludeMin), "605");

            Assert.AreEqual(false, dict.ContainsKey(-1));
            Assert.AreEqual(true, dict.ContainsKey(0));
            Assert.AreEqual(true, dict.ContainsKey(1));
            Assert.AreEqual(true, dict.ContainsKey(2));
            Assert.AreEqual(true, dict.ContainsKey(3));
            Assert.AreEqual(true, dict.ContainsKey(4));
            Assert.AreEqual(false, dict.ContainsKey(5));

            Set(dict, new Range<int>(4, 8, RangeMode.IncludeMax));
            Set(dict, new Range<int>(8, 12, RangeMode.IncludeMax));
            Set(dict, new Range<int>(-10, 1, RangeMode.IncludeMin));
            Set(dict, new Range<int>(50, 52, RangeMode.IncludeMin));
            Set(dict, new Range<int>(49, 58, RangeMode.IncludeMin));
            Assert.AreEqual(5, dict.Count);

            dict.RemoveKey(new Range<int>(int.MinValue, int.MaxValue));
            Assert.AreEqual(0, dict.Count);
        }

        private static void Set(RangeDictionary<int, string> dict, Range<int> range)
        {
            dict.Set(range, range.ToString());
        }
    }
}