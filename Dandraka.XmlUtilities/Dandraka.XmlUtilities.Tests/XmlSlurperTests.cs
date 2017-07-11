using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dandraka.XmlUtilities.Tests
{
    [TestClass]
    public class XmlSlurperTests
    {
        [TestMethod]
        public void ObjectNotNullTest()
        {
            var book = XmlSlurper.ParseText(getFile("book.xml"));

            Assert.IsNotNull(book);
        }

        [TestMethod]
        public void SimpleXmlAttributesTest()
        {
            var book = XmlSlurper.ParseText(getFile("book.xml"));

            Assert.AreEqual("bk101", book.id);
            Assert.AreEqual("123456789", book.isbn);
        }

        [TestMethod]
        public void SimpleXmlNodesTest()
        {
            var book = XmlSlurper.ParseText(getFile("book.xml"));

            Assert.AreEqual("Gambardella, Matthew", book.author);
            Assert.AreEqual("XML Developer's Guide", book.title);
            Assert.AreEqual("Computer", book.genre);
            Assert.AreEqual("44.95", book.price);
        }

        [TestMethod]
        public void ListXmlNodesTest()
        {
            var catalog = XmlSlurper.ParseText(getFile("bookcatalog.xml"));

            var bookList = catalog.bookList;

            Assert.AreEqual(12, bookList.Count);

            var book1 = bookList[0];
            Assert.AreEqual("bk101", book1.id);
            Assert.AreEqual("Gambardella, Matthew", book1.author);
            Assert.AreEqual("XML Developer's Guide", book1.title);
            Assert.AreEqual("Computer", book1.genre);
            Assert.AreEqual("44.95", book1.price);

            var book4 = bookList[3];
            Assert.AreEqual("bk104", book4.id);
            Assert.AreEqual("Corets, Eva", book4.author);
            Assert.AreEqual("Oberon's Legacy", book4.title);
            Assert.AreEqual("Fantasy", book4.genre);
            Assert.AreEqual("5.95", book4.price);

            var book12 = bookList[11];
            Assert.AreEqual("bk112", book12.id);
            Assert.AreEqual("Galos, Mike", book12.author);
            Assert.AreEqual("Visual Studio 7: A Comprehensive Guide", book12.title);
            Assert.AreEqual("Computer", book12.genre);
            Assert.AreEqual("49.95", book12.price);
        }

        [TestMethod]
        public void BothPropertiesAndListXmlTest()
        {
            var nutrition = XmlSlurper.ParseText(getFile("nutrition.xml"));

            var foodList = nutrition.bookList;

            Assert.AreEqual(12, foodList.Count);

            var food1 = foodList[0];
            Assert.AreEqual("Avocado Dip", food1.name);
            Assert.AreEqual("Sunnydale", food1.mfr);
            /*Assert.AreEqual("XML Developer's Guide", food1.total-fat);
            Assert.AreEqual("Computer", food1.genre);
            Assert.AreEqual("44.95", food1.price);*/


        }

        private string getFile(string fileName)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "files", fileName);
            return File.ReadAllText(path);
        }
    }
}
