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
    }
}