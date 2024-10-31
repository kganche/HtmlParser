using System.Text.RegularExpressions;
using HtmlParser.Contracts;
using HtmlParser.Models;

namespace HtmlParser.Services;

public class ClosingTagParser : IHtmlElementParser
{
    public void Parse(Match match, Stack<HtmlElement> elementStack, ref HtmlDocument? document)
    {
        var tagName = match.Groups["closingTag"].Value;

        if (elementStack.Count > 0 && elementStack.Peek().TagName == tagName)
        {
            elementStack.Pop();
        }
    }
}