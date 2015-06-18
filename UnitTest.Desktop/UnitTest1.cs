using System;
using System.Attributes;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Desktop
{
    [TestClass]
    public class UnitTestForPrint
    {
        [TestMethod]
        public void TestMethod1()
        {
            
            Debug.WriteLine(new A()
            {
                D = new D() { Z = "6" },
                E = new E() { Z = "5" }
            }.Print());
        }
    }


    public class A : IPrint
    {
        public static string 发 = "123";

        public readonly int s = 11;

        public string B { get; set; }

        public int C;

        public D D { get; set; }

        public E E;

        public D D1 = new D()
        {
            D2= new D()
            {
                D2 =  new D() { Z = "hehe" }
            }
        };
    }

    public class D : IPrint
    {
        public string Z { get; set; }

        public D D2;
    }

    public class E
    {
        public string Z { get; set; }
    }
}
