using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Desktop.Reflection
{
    [TestClass]
    public class UnitTestForGetterAndSetter
    {
        public class T1
        {
            public int A { get; set; }

            public int B;
        }

        public class T2 : T1
        {

        }

        public class X1
        {
            public T2 A { get; set; }

            public T2 B;
        }

        [TestMethod]
        public void TestMethod_Field_Equal_Spec()
        {
            var obj = new T1();
            var getter = obj.GetType().GetField("B").CompileGetter<T1, int>();
            var setter = obj.GetType().GetField("B").CompileSetter<T1, int>();
            Assert.AreEqual(0, getter[obj]);
            setter[obj] = 8;
            Assert.AreEqual(8, getter[obj]);
        }

        [TestMethod]
        public void TestMethod_Field_Assignable_Any()
        {
            var tobjt = new T1();
            var tgetter = tobjt.GetType().GetField("B").CompileGetter();
            var tsetter = tobjt.GetType().GetField("B").CompileSetter();
            Assert.AreEqual(0, tgetter[tobjt]);
            tsetter[tobjt] = 8;
            Assert.AreEqual(8, tgetter[tobjt]);

            var xobj = new X1();
            var xgetter = xobj.GetType().GetField("B").CompileGetter();
            var xsetter = xobj.GetType().GetField("B").CompileSetter();
            Assert.IsNull(xgetter[xobj]);
            xsetter[xobj] = new T2();
            Assert.IsNotNull(xgetter[xobj]);
        }

        [TestMethod]
        public void TestMethod_Property_Equal_Spec()
        {
            var obj = new T1();
            var getter = obj.GetType().GetProperty("A").CompileGetter<T1, int>();
            var setter = obj.GetType().GetProperty("A").CompileSetter<T1, int>();
            Assert.AreEqual(0, getter[obj]);
            setter[obj] = 8;
            Assert.AreEqual(8, getter[obj]);
        }

        [TestMethod]
        public void TestMethod_Property_Assignable_Any()
        {
            var tobjt = new T1();
            var tgetter = tobjt.GetType().GetProperty("A").CompileGetter();
            var tsetter = tobjt.GetType().GetProperty("A").CompileSetter();
            Assert.AreEqual(0, tgetter[tobjt]);
            tsetter[tobjt] = 8;
            Assert.AreEqual(8, tgetter[tobjt]);

            var xobj = new X1();
            var xgetter = xobj.GetType().GetProperty("A").CompileGetter();
            var xsetter = xobj.GetType().GetProperty("A").CompileSetter();
            Assert.IsNull(xgetter[xobj]);
            xsetter[xobj] = new T2();
            Assert.IsNotNull(xgetter[xobj]);
        }
    }
}
