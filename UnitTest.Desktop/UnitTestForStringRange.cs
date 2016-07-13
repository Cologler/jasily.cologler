using Jasily;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Desktop
{
    [TestClass]
    public class UnitTestForStringRange
    {
        [TestMethod]
        public void TestForEqual()
        {
            foreach (var text in new [] { "", "123", "123456" })
            {
                Assert.AreEqual(text, text.AsRange().ToString());

                var range = new StringRange(text);

                Assert.IsTrue(new StringRange(text).Equals(text));
                Assert.IsTrue(new StringRange(text) == text);
                Assert.IsTrue(text == new StringRange(text));

                Assert.IsTrue(new StringRange(text).Equals(range));
                Assert.IsTrue(new StringRange(text) == range);
                Assert.IsTrue(range == new StringRange(text));

                Assert.IsTrue(text.Substring(0) == text.SubRange(0));
                Assert.IsTrue(text.Substring(0, 0) == text.SubRange(0, 0));
            }

            foreach (var text in new[] { "123", "123456" })
            {
                Assert.IsTrue(text.Substring(0) == text.SubRange(0));
            }
        }
    }
}