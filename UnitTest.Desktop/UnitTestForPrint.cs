using System;
using System.Collections.Generic;
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
            Debug.WriteLine(new Parent()
            {
                D = new D() { Z = "6" },
                E = new E() { Z = "5" }
            }.Print(4));
        }
    }


    public class Parent : IPrintable
    {
        public IObserver<Tuple<List<D>, List<D>, List<D>>> ZNN; 

        public List<D> List = new List<D>()
        {
            new D()
            {
                D2 = new D()
                {
                    D2 = new D() {Z = "hehe"}
                }
            },
            new D()
            {
                D2 = new D()
                {
                    D2 = new D() {Z = "hehe"}
                }
            }
        };

        public E E2 { get; private set; }

        public static string 发 = "123";

        public readonly int ParentReadonlyInt32 = 11;

        public string ParentString { get; set; }

        protected int C;

        public D D { get; set; }

        public E E;

        public D D1 = new D()
        {
            D2= new D()
            {
                D2 =  new D() { Z = "hehe\r\naa" }
            }
        };
    }

    public class D : IPrintable
    {
        public string Z { get; set; }

        public D D2;
    }

    public class E
    {
        public string Z { get; set; }
    }
}
