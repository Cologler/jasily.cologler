using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Jasily.ComponentModel;

namespace UnitTest.Desktop.Linq.Expressions
{
    [TestClass]
    public class UnitTestForJasilyExpression
    {
        [TestMethod]
        public void TestForParsePathFromPropertySelector()
        {
            Assert.AreEqual("Length",
                PropertySelector<int[]>.Start(z => z.Length));
            Assert.AreEqual("Item1.Item1",
                PropertySelector<Tuple<Tuple<string, string>>>.Start(z => z.Item1.Item1));
            Assert.AreEqual("Item1.Item1.Length",
                PropertySelector<Tuple<Tuple<int[], string>>>.Start(z => (((object)z.Item1.Item1) as int[]).Length));
            Assert.AreEqual("Item1.Item1.Length",
                PropertySelector<Tuple<Tuple<int[], string>>>.Start(z => ((int[])(object)z.Item1.Item1).Length));
            Assert.AreEqual("Item1.Item1.Length",
                PropertySelector<Tuple<Tuple<int[], string>>>.Start(z => ((int[])((Tuple<int[], string>)z.Item1).Item1 as int[]).Length));
            Assert.AreEqual("Item1.Length", PropertySelector<Tuple<string[], string>>.Start(z => z.Item1).SelectMany(z => z).Select(z => z.Length));
        }
    }
}
