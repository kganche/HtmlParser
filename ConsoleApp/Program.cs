using HtmlParser.Models;

string html = $@"
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Document</title>
</head>
<body>
    <div>
        <p>Hello, World!</p>
        <p class=""to-remove"">Remove me</p>
    </div>
    <form>
        <input type=""text"">
    </form>
    <img class=""remove"" src=""image.jpg"" />
</body>
</html>";

var doc = HtmlDocument.Load(html);

var div = doc.QuerySelector("div");
var divChildren = div?.GetChildren();
div?.SetAttribute("class", "my-class");

var pToRemove = doc.QuerySelector(".to-remove");
div?.Remove(pToRemove);

div?.QuerySelector("p")?.SetText("New text");

var form = doc.QuerySelector("form");
form?.Add(new HtmlElement("input"));

form?.QuerySelectorAll("input")[1].SetAttribute("type", "password");

var img = doc.QuerySelector("img");
img?.SetAttribute("src", "new_image.jpg");
img?.RemoveAttribute("class");
img?.SetAttribute("alt", "An image");

Console.WriteLine(doc.ToHtml());
Console.WriteLine(img?.GetAttribute("alt"));