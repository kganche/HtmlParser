using HtmlParser.Contracts;

namespace HtmlParser.Models;

public class TextElement(string text) : IHtmlNode
{
    private string Text { get; } = text;
    
    public string? TagName => null;
    
    public string ToHtml() => Text;
}

