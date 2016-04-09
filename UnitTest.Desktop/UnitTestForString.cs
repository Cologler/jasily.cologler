using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest.Desktop
{
    [TestClass]
    public class UnitTestForString
    {
        [TestMethod]
        public void TestForGetString()
        {
            Assert.AreEqual("12345", "12345".ToCharArray().GetString());
        }

        [TestMethod]
        public void TestForAfterFirst()
        {
            Assert.AreEqual("2345", "012345".AfterFirst("1"));
            Assert.AreEqual("2345", "012345".AfterFirst("1"));
            Assert.AreEqual("2345", "012345".AfterFirst('1'));
            Assert.AreEqual("012345", "012345".AfterFirst('k'));
            Assert.AreEqual("345", "012345".AfterFirst('1', '2'));
        }

        [TestMethod]
        public void TestForAfterLast()
        {
            Assert.AreEqual("5", "012345".AfterLast("4"));
            Assert.AreEqual("5", "012345".AfterLast('4'));
            Assert.AreEqual("45", "012345".AfterLast('3', '0'));
        }

        [TestMethod]
        public void TestForChilds()
        {
            Assert.AreEqual("123456", "123456".Childs(null, null));
            Assert.AreEqual("123", "123456".Childs(null, 2));
            Assert.AreEqual("12345", "123456".Childs(null, -2));
            Assert.AreEqual("3456", "123456".Childs(2, null));
            Assert.AreEqual("56", "123456".Childs(-2, null));

            Assert.AreEqual("23", "123456".Childs(1, 2));
            Assert.AreEqual("", "123456".Childs(-1, 2));
            Assert.AreEqual("", "123456".Childs(-2, 2));
            Assert.AreEqual("3456", "123456".Childs(-4, -1));
        }

        [TestMethod]
        public void TestForCommonStart()
        {
            var test1 = new string[] { };
            Assert.AreEqual("", test1.CommonStart());

            var test2 = new[] { "12" };
            Assert.AreEqual("12", test2.CommonStart());

            var test3 = new[] { "", "", "" };
            Assert.AreEqual("", test3.CommonStart());

            var test4 = new[] { "", "12", "" };
            Assert.AreEqual("", test4.CommonStart());

            var test5 = new[] { "12", "12", "123" };
            Assert.AreEqual("12", test5.CommonStart());
        }

        [TestMethod]
        public void TestForCommonEnd()
        {
            var test1 = new string[] { };
            Assert.AreEqual("", test1.CommonEnd());

            var test2 = new[] { "12" };
            Assert.AreEqual("12", test2.CommonEnd());

            var test3 = new[] { "", "", "" };
            Assert.AreEqual("", test3.CommonEnd());

            var test4 = new[] { "", "12", "" };
            Assert.AreEqual("", test4.CommonEnd());

            var test5 = new[] { "12", "12", "123" };
            Assert.AreEqual("", test5.CommonEnd());

            var test6 = new[] { "012", "012", "312" };
            Assert.AreEqual("12", test6.CommonEnd());
        }

        [TestMethod]
        public void TestForStartsWith()
        {
            Assert.AreEqual(true, "".StartsWith(0, "", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(true, "123".StartsWith(0, "12", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(true, "A123".StartsWith(0, "a", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void TestForEndsWith()
        {
            Assert.AreEqual(true, "123".EndsWith(2, "2", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(true, "123".EndsWith(2, "12", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(false, "123".EndsWith(2, "22", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void TestForReplace()
        {
            Assert.AreEqual("1234-c-C6789_1234-c-C6789", "1234abcABC6789_1234abcABC6789".Replace("ab", "-", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("__123abcABC", "abcABC123abcABC".ReplaceStart("abc", "_", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("abcABC123__", "abcABC123abcABC".ReplaceEnd("abc", "_", StringComparison.OrdinalIgnoreCase));
        }
    }
}