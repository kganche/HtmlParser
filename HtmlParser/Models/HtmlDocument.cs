using System.Text.RegularExpressions;
using HtmlParser.Contracts;
using HtmlParser.Exceptions;
using HtmlParser.Services;

namespace HtmlParser.Models;

public class HtmlDocument(string rootTag)
{
    public HtmlElement Root { get; init; } = new(rootTag);

    public string ToHtml() => Root.ToHtml();

    public HtmlElement? QuerySelector(string selector) => Root.QuerySelector(selector);

    public List<HtmlElement> QuerySelectorAll(string selector) => Root.QuerySelectorAll(selector);

    public static HtmlDocument Load(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            throw new HtmlParsingException("Html cannot be null, empty or whitespace!");
        }
        
        var pattern = @"<(?<tag>[a-zA-Z][a-zA-Z0-9]*)\s*(?<attributes>[^>]*)\/?>|<\/(?<closingTag>[a-zA-Z][a-zA-Z0-9]*)>|(?<text>[^<]+)";
        var matches = Regex.Matches(html, pattern);

        var stack = new Stack<HtmlElement>();
        HtmlDocument? doc = null;
        IHtmlElementParser? parser = null;

        try
        {
            foreach (Match match in matches)
            {
                if (match.Groups["tag"].Success)
                {
                    parser = new OpeningTagParser();
                }
                else if (match.Groups["closingTag"].Success)
                {
                    parser = new ClosingTagParser();
                }
                else if (match.Groups["text"].Success)
                {
                    parser = new TextParser();
                }
            
                parser?.Parse(match, stack, ref doc);
            }
            
            if (doc is null || stack.Count > 0) 
            {
                throw new HtmlParsingException("Parsing failed: unmatched tags found in the HTML.!");
            }
        }
        catch (Exception ex)
        {
            throw new HtmlParsingException("Html parsing failed due to an error!", ex);
        }

        return doc;
    }
}
