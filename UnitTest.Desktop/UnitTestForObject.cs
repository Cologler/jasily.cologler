using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Desktop
{
    [TestClass]
    public class UnitTestForObject
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(1.NormalEquals(1));
            Assert.IsFalse(1.NormalEquals(2));
            Assert.IsTrue("1".NormalEquals("1"));
            Assert.IsFalse("1".NormalEquals("2"));
            Assert.IsTrue(new TestClassA() { Id = 5 }.NormalEquals(new TestClassA() { Id = 5 }));
            Assert.IsFalse(new TestClassB() { Id = 5 }.NormalEquals(new TestClassB() { Id = 5 }));
            Assert.IsTrue(new TestStructA() { Oid = 8, Id = 5 }.NormalEquals(new TestStructA() { Id = 5 }));
            Assert.IsFalse(new TestStructB() { Oid = 8, Id = 5 }.NormalEquals(new TestStructB() { Id = 5 }));

            Debug.WriteLine(typeof(List<string>).GetCSharpName());
        }

        class TestClassA : IEquatable<TestClassA>
        {
            public int Id;

            public bool Equals(TestClassA other)
            {
                return other != null && this.Id == other.Id;
            }
        }

        class TestClassB
        {
            public int Id;

            public bool Equals(TestClassB other)
            {
                return other != null && this.Id == other.Id;
            }
        }

        struct TestStructA : IEquatable<TestStructA>
        {
            public int Oid;
            public int Id;

            public bool Equals(TestStructA other)
            {
                return this.Id == other.Id;
            }
        }

        struct TestStructB
        {
            public int Oid;
            public int Id;

            public bool Equals(TestStructB other)
            {
                return this.Id == other.Id;
            }
        }
    }
}
