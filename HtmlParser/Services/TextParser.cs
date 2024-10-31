using System.Text.RegularExpressions;
using HtmlParser.Contracts;
using HtmlParser.Models;

namespace HtmlParser.Services;

public class TextParser : IHtmlElementParser
{
    public void Parse(Match match, Stack<HtmlElement> elementStack, ref HtmlDocument? document)
    {
        var textContent = match.Groups["text"].Value.Trim();
        if (textContent.Length > 0)
        {
            elementStack.Peek().Add(new TextElement(textContent));
        }
    }
}