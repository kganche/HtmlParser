using System.Text.RegularExpressions;

namespace HtmlParser;

public class HtmlDocument(string rootTag)
{
    private HtmlElement _root = new(rootTag);

    public HtmlElement Root => _root;

    public string ToHtml() => _root.ToHtml();

    public HtmlElement? QuerySelector(string selector) => _root.QuerySelector(selector);

    public List<HtmlElement> QuerySelectorAll(string selector) => _root.QuerySelectorAll(selector);

    public static HtmlDocument? Load(string html)
    {
        string pattern = @"<(?<tag>[a-zA-Z][a-zA-Z0-9]*)\s*(?<attributes>[^>]*)\/?>|<\/(?<closingTag>[a-zA-Z][a-zA-Z0-9]*)>|(?<text>[^<]+)";
        var matches = Regex.Matches(html, pattern);

        var stack = new Stack<HtmlElement>();
        HtmlDocument? doc = null;

        foreach (Match match in matches)
        {
            if (match.Groups["tag"].Success)
            {
                var tagName = match.Groups["tag"].Value;
                var attributes = match.Groups["attributes"].Value;

                var element = new HtmlElement(tagName);
                ParseAttributes(element, attributes);

                if (doc == null)
                {
                    doc = new HtmlDocument(tagName)
                    {
                        _root = element
                    };
                    stack.Push(element);
                }
                else
                {
                    if (!IsSelfClosing(tagName))
                    {
                        stack.Peek().Add(element);
                        stack.Push(element);
                    }
                    else
                    {
                        stack.Peek().Add(element);
                    }
                }
            }
            else if (match.Groups["closingTag"].Success)
            {
                var tagName = match.Groups["closingTag"].Value;

                if (stack.Count > 0 && stack.Peek().TagName == tagName)
                {
                    stack.Pop();
                }
            }
            else if (match.Groups["text"].Success)
            {
                var textContent = match.Groups["text"].Value.Trim();
                if (textContent.Length > 0)
                {
                    stack.Peek().Add(new TextNode(textContent));
                }
            }
        }

        return doc;
    }

    private static void ParseAttributes(HtmlElement element, string attributes)
    {
        string attrPattern = @"(?<name>[a-zA-Z_:][-a-zA-Z0-9_:.]*)\s*=\s*[""'](?<value>[^""']+)[""']";
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
