using System;
using Jasily;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Desktop
{
    [TestClass]
    public class UnitTestForActivator
    {
        [TestMethod]
        public void TestMethodA()
        {
            var a = JasilyActivator.CreateInstance<A>();
            Assert.IsNotNull(a);
            Assert.IsInstanceOfType(a, typeof(A));
        }

        public class A { }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestMethodB()
        {
            var a = JasilyActivator.CreateInstance<B>();
            Assert.IsNotNull(a);
            Assert.IsInstanceOfType(a, typeof(B));
        }

        public class B
        {
            public B(string s)
            {

            }
        }

        [TestMethod]
        public void TestMethodC()
        {
            var a = JasilyActivator.CreateInstance<C>();
            Assert.IsNotNull(a);
            Assert.IsInstanceOfType(a, typeof(C));
            Assert.AreEqual(((C)a).S, "6");
        }

        public class C
        {
            public C(string s = "6")
            {
                this.S = s;
            }

            public string S;
        }

        [TestMethod]
        public void TestMethodD()
        {
            var a = JasilyActivator.CreateInstance<D>();
            Assert.IsNotNull(a);
            Assert.IsInstanceOfType(a, typeof(D));
            Assert.AreEqual(((D)a).S, null);
        }

        public class D
        {
            public D()
            {

            }

            public D(string s = "6")
            {
                this.S = s;
            }

            public string S;
        }

        [TestMethod]
        public void TestMethodE()
        {
            var a = JasilyActivator.CreateInstance<E>();
            Assert.IsNotNull(a);
            Assert.IsInstanceOfType(a, typeof(E));
            Assert.AreEqual(((E)a).S, "6");
        }

        public class E
        {
            public E()
            {

            }

            [JasilyActivator.EntryAttribute]
            public E(string s = "6")
            {
                this.S = s;
            }

            public string S;
        }

        [TestMethod]
        public void TestMethodF()
        {
            var a = JasilyActivator.CreateInstance<F>();
            Assert.IsNotNull(a);
            Assert.IsInstanceOfType(a, typeof(F));
            Assert.AreEqual(((F)a).S, "6");
        }

        public class F
        {
            public F()
            {

            }

            [JasilyActivator.EntryAttribute]
            public F([JasilyActivator.DefaultValue("6")] string s)
            {
                this.S = s;
            }

            public string S;
        }
    }
}
