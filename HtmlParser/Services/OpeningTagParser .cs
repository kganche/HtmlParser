using System.Text.RegularExpressions;
using HtmlParser.Contracts;
using HtmlParser.Models;

namespace HtmlParser.Services;

public class OpeningTagParser  : IHtmlElementParser
{
    public void Parse(Match match, Stack<HtmlElement> elementStack, ref HtmlDocument? document)
    {
        var tagName = match.Groups["tag"].Value;
        var attributes = match.Groups["attributes"].Value;
        
        var element = new HtmlElement(tagName);
        ParseAttributes(element, attributes);

        if (document == null)
        {
            document = new HtmlDocument(tagName)
            {
                Root = element
            };
            elementStack.Push(element);
        }
        else
        {
            if (!IsSelfClosing(tagName))
            {
                elementStack.Peek().Add(element);
                elementStack.Push(element);
            }
            else
            {
                elementStack.Peek().Add(element);
            }
        }
    }
    
    private static void ParseAttributes(HtmlElement element, string attributes)
    {
        var attrPattern = @"(?<name>[a-zA-Z_:][-a-zA-Z0-9_:.]*)\s*=\s*[""'](?<value>[^""']+)[""']";
        var matches = Regex.Matches(attributes, attrPattern);

        foreach (Match match in matches)
        {
            var name = match.Groups["name"].Value;
            var value = match.Groups["value"].Value;
            element.SetAttribute(name, value);
        }
    }

    private static bool IsSelfClosing(string tagName)
    {
        var selfClosingTags = new HashSet<string> { "img", "br", "hr", "input", "meta", "link" };
        return selfClosingTags.Contains(tagName);
    }
}
