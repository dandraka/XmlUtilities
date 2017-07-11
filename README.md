# XmlUtilities
An XmlSlurper implementation for .Net.

## Usage:

```
using Dandraka.XmlUtilities;

public void PrintXmlContents1()
{
	string xml = "<book id=\"bk101\" isbn=\"123456789\"><author>Gambardella, Matthew</author><title>XML Developer Guide</title></book>";
	var book = XmlSlurper.ParseText(xml);

	// that's it, now we have everything
	Console.WriteLine("id = " + book.id);
	Console.WriteLine("isbn = " + book.isbn);
	Console.WriteLine("author = " + book.author);
	Console.WriteLine("title = " + book.title);
}

public void PrintXmlContents2()
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
```

## Note: 
Although not required by the license, the author kindly asks that you share any improvements you made.
