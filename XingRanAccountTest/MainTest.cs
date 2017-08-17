using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using MCS.Library.Office.OpenXml.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XingRanAccountTest
{

    [TestClass]
    public class MainTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            WorkBook sourceBook = WorkBook.Load("Excel\\source.xlsx");
            WorkBook scopeBook = WorkBook.Load("Excel\\template.xlsx");

            string msg;
            bool pass = ExcelDo.Do(sourceBook, scopeBook, out msg);
            Assert.IsTrue(pass);
            Console.Out.WriteLine(msg);
            scopeBook.Save("Excel\\out.xlsx");
        }
    }
}
