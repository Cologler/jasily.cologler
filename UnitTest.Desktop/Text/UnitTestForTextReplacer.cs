using Jasily.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTest.Desktop.Text
{
    [TestClass]
    public class UnitTestForTextReplacer
    {
        [TestMethod]
        public void Default()
        {
            const string origin = "0abc0ABC0";
            var replacements = new Dictionary<string, string>()
            {
                ["abc"] = "_"
            };
            Assert.AreEqual("0_0ABC0", TextReplacer.Replace(origin, replacements));
            Assert.AreEqual("0_0_0", TextReplacer.Replace(origin, replacements, StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void DefaultNoPadding()
        {
            const string origin = "abcABC";
            var replacements = new Dictionary<string, string>()
            {
                ["abc"] = "_"
            };
            Assert.AreEqual("_ABC", TextReplacer.Replace(origin, replacements));
            Assert.AreEqual("__", TextReplacer.Replace(origin, replacements, StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void Conflict()
        {
            const string origin = "abcABC";
            var replacements = new Dictionary<string, string>()
            {
                ["bcA"] = "1",
                ["bcAB"] = "2"
            };
            Assert.AreEqual("a2C", TextReplacer.Replace(origin, replacements));
        }

        [TestMethod]
        public void ConflictNoPadding()
        {
            const string origin = "bcAB";
            var replacements = new Dictionary<string, string>()
            {
                ["bcA"] = "1",
                ["bcAB"] = "2"
            };
            Assert.AreEqual("2", TextReplacer.Replace(origin, replacements));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ArgumentException()
        {
            const string origin = "bcAB";
            var replacements = new Dictionary<string, string>()
            {
                ["bcA"] = "1",
                ["bca"] = "2"
            };
            Assert.AreEqual(null, TextReplacer.Replace(origin, replacements, StringComparison.OrdinalIgnoreCase));
        }
    }
}