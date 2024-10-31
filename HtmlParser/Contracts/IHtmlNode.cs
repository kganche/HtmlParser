namespace HtmlParser.Contracts;

public interface IHtmlNode
{
    string? TagName { get; }
    
    string ToHtml();
}