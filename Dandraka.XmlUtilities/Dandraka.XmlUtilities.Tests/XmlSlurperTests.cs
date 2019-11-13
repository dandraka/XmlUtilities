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
        public void T01_ObjectNotNullTest()
        {
            var city = XmlSlurper.ParseText(getFile("city.xml"));

            Assert.IsNotNull(city);
        }

        [TestMethod]
        public void T02_SimpleXmlAttributesTest()
        {
            var book = XmlSlurper.ParseText(getFile("book.xml"));

            Assert.AreEqual("bk101", book.id);
            Assert.AreEqual("123456789", book.isbn);
        }

        [TestMethod]
        public void T03_SimpleXmlNodesTest()
        {
            var book = XmlSlurper.ParseText(getFile("book.xml"));

            Assert.AreEqual("Gambardella, Matthew", book.author);
            Assert.AreEqual("XML Developer's Guide", book.title);
            Assert.AreEqual("Computer", book.genre);
            Assert.AreEqual("44.95", book.price);
        }

        [TestMethod]
        public void T04_XmlMultipleLevelsNodesTest()
        {
            var settings = XmlSlurper.ParseText(getFile("settings.xml"));

            Assert.AreEqual("true", settings.view.displayIcons);
            Assert.AreEqual("false", settings.performance.additionalChecks.disk.brandOptions.toshiba.useBetaFunc);
        }

        [TestMethod]
        public void T05_ListXmlNodesTest()
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
        public void T06_BothPropertiesAndListRootXmlTest()
        {
            var nutrition = XmlSlurper.ParseText(getFile("nutrition.xml"));

            var foodList = nutrition.foodList;

            Assert.AreEqual(10, foodList.Count);

            var food1 = foodList[0];
            Assert.AreEqual("Avocado Dip", food1.name);
            Assert.AreEqual("Sunnydale", food1.mfr);
            Assert.AreEqual("11", food1.totalfat);

            Assert.AreEqual("1", food1.vitamins.a);
            Assert.AreEqual("0", food1.vitamins.c);


        }

        [TestMethod]
        public void T07_BothPropertiesAndListRecursiveXmlTest()
        {
            var city = XmlSlurper.ParseText(getFile("cityInfo.xml"));

            Assert.IsTrue(city.Mayor == "Roni Mueller");
            Assert.IsTrue(city.CityHall == "Schulstrasse 12");
            Assert.IsTrue(city.Name == "Wilen bei Wollerau");
            Assert.IsTrue(city.Gemeinde == "Freienbach");
            Assert.AreEqual(3, city.StreetList.Count);

            Assert.AreEqual("8832", city.StreetList[2].PostCode);
            Assert.AreEqual(3, city.StreetList[2].HouseNumberList.Count);
        }

        [TestMethod]
        public void T08_PrintXmlContents1()
        {
            string xml = "<book id=\"bk101\" isbn=\"123456789\"><author>Gambardella, Matthew</author><title>XML Developer Guide</title></book>";
            var book = XmlSlurper.ParseText(xml);

            // that's it, now we have everything
            Console.WriteLine("id = " + book.id);
            Console.WriteLine("isbn = " + book.isbn);
            Console.WriteLine("author = " + book.author);
            Console.WriteLine("title = " + book.title);
        }

        [TestMethod]
        public void T09_PrintXmlContents2()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                         "<nutrition>" +
                         "	<food>" +
                         "		<name>Avocado Dip</name>" +
                         "		<mfr>Sunnydale</mfr>" +
                         "		<carb>2</carb>" +
                         "		<fiber>0</fiber>" +
                         "		<protein>1</protein>" +
                         "	</food>" +
                         "	<food>" +
                         "		<name>Bagels, New York Style </name>" +
                         "		<mfr>Thompson</mfr>" +
                         "		<carb>54</carb>" +
                         "		<fiber>3</fiber>" +
                         "		<protein>11</protein>" +
                         "	</food>" +
                         "	<food>" +
                         "		<name>Beef Frankfurter, Quarter Pound </name>" +
                         "		<mfr>Armitage</mfr>" +
                         "		<carb>8</carb>" +
                         "		<fiber>0</fiber>" +
                         "		<protein>13</protein>" +
                         "	</food>" +
                         "</nutrition>";
            var nutrition = XmlSlurper.ParseText(xml);

            // since many food nodes were found, a list was generated and named foodList (common name + "List")
            Console.WriteLine("name1 = " + nutrition.foodList[0].name);
            Console.WriteLine("name2 = " + nutrition.foodList[1].name);
        }

        [TestMethod]
        public void T10_BoolIntDecimalTest()
        {
            var settings = XmlSlurper.ParseText(getFile("settings.xml"));

            Assert.AreEqual<bool?>(true, settings.view.displayIcons);
            Assert.AreEqual<bool?>(false, settings.view.showFiles);
            Assert.AreEqual<int?>(2, settings.performance.additionalChecks.disk.minFreeSpace);
            Assert.AreEqual<double?>(5.5, settings.performance.additionalChecks.disk.warnFreeSpace);
            Assert.AreEqual<decimal?>(5.5m, settings.performance.additionalChecks.disk.warnFreeSpace);

            // usage showcase
            if (!settings.view.displayIcons)
            {
                Assert.Fail();
            }
        }

        private string getFile(string fileName)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "testdata", fileName);
            return File.ReadAllText(path);
        }
    }
}
