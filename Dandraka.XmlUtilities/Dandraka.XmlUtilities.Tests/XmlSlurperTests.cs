using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace Dandraka.XmlUtilities.Tests
{
    //[TestClass]
    public class XmlSlurperTests
    {
        [Fact]
        public void T01_ObjectNotNullTest()
        {
            var city = XmlSlurper.ParseText(getFile("City.xml"));

            Assert.NotNull(city);
        }

        [Fact]
        public void T02_SimpleXmlAttributesTest()
        {
            var book = XmlSlurper.ParseText(getFile("Book.xml"));

            Assert.Equal("bk101", book.id);
            Assert.Equal("123456789", book.isbn);
        }

        [Fact]
        public void T03_SimpleXmlNodesTest()
        {
            var book = XmlSlurper.ParseText(getFile("Book.xml"));

            Assert.Equal("Gambardella, Matthew", book.author);
            Assert.Equal("XML Developer's Guide", book.title);
            Assert.Equal("Computer", book.genre);
            Assert.Equal("44.95", book.price);
        }

        [Fact]
        public void T04_XmlMultipleLevelsNodesTest()
        {
            var settings = XmlSlurper.ParseText(getFile("HardwareSettings.xml"));

            Assert.Equal("true", settings.view.displayIcons);
            Assert.Equal("false", settings.performance.additionalChecks.disk.brandOptions.toshiba.useBetaFunc);
        }

        [Fact]
        public void T05_ListXmlNodesTest()
        {
            var catalog = XmlSlurper.ParseText(getFile("BookCatalog.xml"));

            var bookList = catalog.bookList;

            Assert.Equal(12, bookList.Count);

            var book1 = bookList[0];
            Assert.Equal("bk101", book1.id);
            Assert.Equal("Gambardella, Matthew", book1.author);
            Assert.Equal("XML Developer's Guide", book1.title);
            Assert.Equal("Computer", book1.genre);
            Assert.Equal("44.95", book1.price);

            var book4 = bookList[3];
            Assert.Equal("bk104", book4.id);
            Assert.Equal("Corets, Eva", book4.author);
            Assert.Equal("Oberon's Legacy", book4.title);
            Assert.Equal("Fantasy", book4.genre);
            Assert.Equal("5.95", book4.price);

            var book12 = bookList[11];
            Assert.Equal("bk112", book12.id);
            Assert.Equal("Galos, Mike", book12.author);
            Assert.Equal("Visual Studio 7: A Comprehensive Guide", book12.title);
            Assert.Equal("Computer", book12.genre);
            Assert.Equal("49.95", book12.price);
        }

        [Fact]
        public void T06_BothPropertiesAndListRootXmlTest()
        {
            var nutrition = XmlSlurper.ParseText(getFile("Nutrition.xml"));

            var foodList = nutrition.foodList;

            Assert.Equal(10, foodList.Count);

            var food1 = foodList[0];
            Assert.Equal("Avocado Dip", food1.name);
            Assert.Equal("Sunnydale", food1.mfr);
            Assert.Equal("11", food1.totalfat);

            Assert.Equal("1", food1.vitamins.a);
            Assert.Equal("0", food1.vitamins.c);


        }

        [Fact]
        public void T07_BothPropertiesAndListRecursiveXmlTest()
        {
            var city = XmlSlurper.ParseText(getFile("CityInfo.xml"));

            Assert.True(city.Mayor == "Roni Mueller");
            Assert.True(city.CityHall == "Schulstrasse 12");
            Assert.True(city.Name == "Wilen bei Wollerau");
            Assert.True(city.Gemeinde == "Freienbach");
            Assert.Equal(3, city.StreetList.Count);

            Assert.Equal("8832", city.StreetList[2].PostCode);
            Assert.Equal(3, city.StreetList[2].HouseNumberList.Count);
        }

        /// <summary>
        /// Usage showcase
        /// </summary>
        [Fact]
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

        /// <summary>
        /// Usage showcase
        /// </summary>
        [Fact]
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

        [Fact]
        public void T10_BoolIntDecimalDoubleTest()
        {
            var settings = XmlSlurper.ParseText(getFile("HardwareSettings.xml"));

            Assert.Equal<bool?>(true, settings.view.displayIcons);
            Assert.Equal<bool?>(false, settings.view.showFiles);
            Assert.Equal<int?>(2, settings.performance.additionalChecks.disk.minFreeSpace);
            Assert.Equal<double?>(5.5, settings.performance.additionalChecks.disk.warnFreeSpace);
            Assert.Equal<decimal?>(5.5m, settings.performance.additionalChecks.disk.warnFreeSpace);

            Assert.True(settings.view.displayIcons);
            Assert.False(settings.view.showFiles);
            Assert.Equal<int>(2, settings.performance.additionalChecks.disk.minFreeSpace);
            Assert.Equal<double>(5.5, settings.performance.additionalChecks.disk.warnFreeSpace);
            Assert.Equal<decimal>(5.5m, settings.performance.additionalChecks.disk.warnFreeSpace);

            // usage showcase
            if (!settings.view.displayIcons)
            {
                Assert.True(false);
            }
            int? minFreeSpace = settings.performance.additionalChecks.disk.minFreeSpace;
            if (minFreeSpace != 2)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void T11_ConversionExceptionTest()
        {
            var settings = XmlSlurper.ParseText(getFile("HardwareSettings.xml"));

            Assert.Throws<ValueConversionException>(() =>
            {
                int t = settings.view.displayIcons;
            });
            Assert.Throws<ValueConversionException>(() =>
            {
                decimal t = settings.view.displayIcons;
            });
            Assert.Throws<ValueConversionException>(() =>
            {
                double t = settings.view.displayIcons;
            });
            Assert.Throws<ValueConversionException>(() =>
            {
                bool t = settings.performance.additionalChecks.disk.minFreeSpace;
            });
        }

        [Fact]
        public void T12_CDataTest()
        {
            var cdata = XmlSlurper.ParseText(getFile("CData.xml"));

            // test cdata for single nodes
            Assert.Equal("DOCUMENTO N. 1234-9876", cdata.Title);

            // test cdata for list nodes
            dynamic attr = cdata.AttributeList[0];
            Assert.Equal("document.id", attr.Name);
            Assert.Equal("<string>DOCUMENTO N. 1234-9876</string>", attr);

            attr = cdata.AttributeList[4];
            Assert.Equal("receipt.date", attr.Name);
            Assert.Equal("<string>2020-12-28</string>", attr);

            attr = cdata.AttributeList[5];
            Assert.Equal("fcurrency", attr.Name);
            Assert.Equal("EUR", attr);
        }

        private string getFile(string fileName)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "testdata", fileName);
            return File.ReadAllText(path);
        }
    }
}
