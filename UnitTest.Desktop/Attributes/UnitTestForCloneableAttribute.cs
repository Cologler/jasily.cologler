using System;
using System.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Desktop.Attributes
{
    [TestClass]
    public class UnitTestForCloneableAttribute
    {
        [TestMethod]
        public void TestMethod1()
        {
            var accept = new AcceptClass("1", "2", "3", "4", "5", "6", "7", "8", "9");
            var acceptC = CloneableAttribute.Clone(accept,
                new AcceptClass("", "", "", "", "", "", "", "", ""));

            Assert.AreEqual(accept.GetPublicProperty(), acceptC.GetPublicProperty());
            Assert.AreEqual(accept.GetProtectedProperty(), acceptC.GetProtectedProperty());
            Assert.AreEqual(accept.GetPrivateProperty(), acceptC.GetPrivateProperty());
            Assert.AreEqual(accept.GetPublicField(), acceptC.GetPublicField());
            Assert.AreEqual(accept.GetProtectedField(), acceptC.GetProtectedField());
            Assert.AreEqual(accept.GetPrivateFirld(), acceptC.GetPrivateFirld());
            Assert.AreEqual(accept.GetPublicReadonlyField(), acceptC.GetPublicReadonlyField());
            Assert.AreEqual(accept.GetProtectedReadonlyField(), acceptC.GetProtectedReadonlyField());
            Assert.AreEqual(accept.GetPrivateReadonlyFirld(), acceptC.GetPrivateReadonlyFirld());
        }
    }

    public class AcceptClass
    {
        public AcceptClass(
            string publicProperty,
            string protectedProperty,
            string privateProperty,
            string publicField,
            string protectedField,
            string privateFirld, string publicReadonlyField, string protectedReadonlyField, string privateReadonlyFirld)
        {
            PublicProperty = publicProperty;
            ProtectedProperty = protectedProperty;
            PublicField = publicField;
            ProtectedField = protectedField;
            PrivateFirld = privateFirld;
            PublicReadonlyField = publicReadonlyField;
            ProtectedReadonlyField = protectedReadonlyField;
            PrivateReadonlyFirld = privateReadonlyFirld;
            PrivateProperty = privateProperty;
        }

        [Cloneable]
        public string PublicProperty { get; set; }

        [Cloneable]
        protected string ProtectedProperty { get; set; }

        [Cloneable]
        private string PrivateProperty { get; set; }

        [Cloneable] public string PublicField;

        [Cloneable] protected string ProtectedField;

        [Cloneable] private string PrivateFirld;

        [Cloneable] public readonly string PublicReadonlyField;

        [Cloneable] protected readonly string ProtectedReadonlyField;

        [Cloneable] private readonly string PrivateReadonlyFirld;

        public string GetPublicProperty()
        {
            return this.PublicProperty;
        }

        public string GetProtectedProperty()
        {
            return this.ProtectedProperty;
        }

        public string GetPrivateProperty()
        {
            return this.PrivateProperty;
        }

        public string GetPublicField()
        {
            return this.PublicField;
        }

        public string GetProtectedField()
        {
            return this.ProtectedField;
        }

        public string GetPrivateFirld()
        {
            return this.PrivateFirld;
        }

        public string GetPublicReadonlyField()
        {
            return this.PublicReadonlyField;
        }

        public string GetProtectedReadonlyField()
        {
            return this.ProtectedReadonlyField;
        }

        public string GetPrivateReadonlyFirld()
        {
            return this.PrivateReadonlyFirld;
        }
    }
}
