using System;
using System.Collections.Generic;
using Jasily.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Assert.AreEqual("a2C", TextReplacer.Replace(origin, new Dictionary<string, string>()
            {
                ["bcA"] = "1",
                ["bcAB"] = "2"
            }));
        }

        [TestMethod]
        public void ConflictNoPadding()
        {
            const string origin = "bcAB";
            Assert.AreEqual("2", TextReplacer.Replace(origin, new Dictionary<string, string>()
            {
                ["bcA"] = "1",
                ["bcAB"] = "2"
            }));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ArgumentException()
        {
            const string origin = "bcAB";
            Assert.AreEqual(null, TextReplacer.Replace(origin, new Dictionary<string, string>()
            {
                ["bcA"] = "1",
                ["bca"] = "2"
            }, StringComparison.OrdinalIgnoreCase));
        }
    }
}