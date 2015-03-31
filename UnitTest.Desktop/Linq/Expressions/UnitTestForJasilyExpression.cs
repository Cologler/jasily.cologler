using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Desktop.Linq.Expressions
{
    [TestClass]
    public class UnitTestForJasilyExpression
    {
        [TestMethod]
        public void TestForParsePathFromPropertySelector()
        {
            Assert.AreEqual(JasilyExpression.PropertySelector<int[]>(
                z => z.Length),
                "Length");
            Assert.AreEqual(JasilyExpression.PropertySelector<Tuple<Tuple<string, string>, string>>(
                z => z.Item1.Item1),
                "Item1.Item1");
            Assert.AreEqual(JasilyExpression.PropertySelector<Tuple<Tuple<int[], string>, string>>(
                z => z.Item1.Item1 as int[]).Length,
                "Item1.Item1.Length");
            Assert.AreEqual(JasilyExpression.PropertySelector<Tuple<Tuple<int[], string>, string>>(
                z => (int[])z.Item1.Item1 as int[]).Length,
                "Item1.Item1.Length");
            Assert.AreEqual(JasilyExpression.PropertySelector<Tuple<Tuple<int[], string>, string>>(
                z => (int[])((Tuple<int[], string>)z.Item1).Item1 as int[]).Length,
                "Item1.Item1.Length");

            // exception

            try
            {
                object nulobj = null;
                JasilyExpression.PropertySelector<object>(z => nulobj.Equals(null));
                Assert.Fail("should throw.");
            }
            catch (ArgumentException) { }

            try
            {
                object single = new object();
                JasilyExpression.PropertySelector<object>(z => single.Equals(null));
                Assert.Fail("should throw.");
            }
            catch (ArgumentException) { }
        }
    }
}
