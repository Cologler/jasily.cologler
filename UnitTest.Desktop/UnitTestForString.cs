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
    }
}