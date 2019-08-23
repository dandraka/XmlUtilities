# XmlUtilities
An XmlSlurper implementation for .Net. The idea came from [Groovy's XmlSlurper](http://groovy-lang.org/processing-xml.html) which is hugely useful.

What this does, is convert a piece of xml, e.g.

```
<card xmlns="http://businesscard.org">
   <name>John Doe</name>
   <title>CEO, Widget Inc.</title>
   <email>john.doe@widget.com</email>
   <phone>(202) 456-1414</phone>
   <logo url="widget.gif"/>
 </card>
```

to a C# object, e.g.

```
card.name
card.title
card.email
card.phone
card.logo.url
```

This is done ***without any need to declare the type*** (it actually uses [System.Dynamic.ExpandoObject](https://msdn.microsoft.com/en-us/library/system.dynamic.expandoobject(v=vs.110).aspx) behind the scenes).

Under the Release tab you can find the binaries to download.

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

## Releases: 
The first production-ready release was uploaded. Please see the release page for a known bug.

## Note: 
Although not required by the license, the author kindly asks that you share any improvements you make.
