using System.Text.RegularExpressions;
using HtmlParser.Models;

namespace HtmlParser.Contracts;

public interface IHtmlElementParser
{
    void Parse(Match match, Stack<HtmlElement> elementStack, ref HtmlDocument? document);
}