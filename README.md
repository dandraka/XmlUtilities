# XmlUtilities
An XmlSlurper implementation for .Net

Usage:

```
using Dandraka.XmlUtilities;

private void doStuff()
{
  string xml = "<book id="bk101" isbn="123456789"><author>Gambardella, Matthew</author><title>XML Developer Guide</title></book>";
  var book = XmlSlurper.ParseText(xml);  
  
  // that's it, now we have everything
  Console.WriteLine("id = " + book.id);
  Console.WriteLine("isbn = " + book.isbn);
  Console.WriteLine("author = " + book.author);
  Console.WriteLine("title = " + book.title);
}
```
