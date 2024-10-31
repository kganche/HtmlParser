using HtmlParser.Contracts;

namespace HtmlParser.Models;

public class HtmlElement(string tagName) : ICompositeNode
{
    private readonly List<IHtmlNode> _children = [];

    private Dictionary<string, string> Attributes { get; set; } = [];
    
    public string TagName { get; } = tagName;

    public void Add(IHtmlNode node)
    {
        _children.Add(node);
    }

    public void Remove(IHtmlNode node)
    {
        _children.Remove(node);
    }

    public List<IHtmlNode> GetChildren()
    {
        return _children;
    }

    public void SetText(string text)
    {
        _children.Clear();

        Add(new TextElement(text));
    }

    public void SetAttribute(string name, string value)
    {
        Attributes[name] = value;
    }

    public void RemoveAttribute(string name)
    {
        Attributes.Remove(name);
    }

    public string? GetAttribute(string name) => Attributes.GetValueOrDefault(name);

    public HtmlElement? QuerySelector(string selector)
    {
        if (MatchesSelector(selector))
        {
            return this;
        }

        foreach (var child in _children.OfType<HtmlElement>())
        {
            var foundElement = child.QuerySelector(selector);
            if (foundElement != null)
            {
                return foundElement;
            }
        }

        return null;
    }

    public List<HtmlElement> QuerySelectorAll(string selector)
    {
        var matchingElements = new List<HtmlElement>();

        if (MatchesSelector(selector))
        {
            matchingElements.Add(this);
        }

        foreach (var child in _children.OfType<HtmlElement>())
        {
            matchingElements.AddRange(child.QuerySelectorAll(selector));
        }

        return matchingElements;
    }

    public string ToHtml()
    {
        var attributes = string.Join(" ", Attributes.Select(a => $"{a.Key}=\"{a.Value}\""));
        var openingTag = string.IsNullOrEmpty(attributes) ? $"<{TagName}>" : $"<{TagName} {attributes}>";
        var closingTag = IsSelfClosing() ? "" : $"</{TagName}>";
        var innerHtml = string.Join("", _children.Select(c => c.ToHtml()));

        return $"{openingTag}{innerHtml}{closingTag}";
    }

    private bool IsSelfClosing()
    {
        var selfClosingTags = new HashSet<string> { "img", "br", "hr", "input", "meta", "link" };
        return selfClosingTags.Contains(TagName);
    }

    private bool MatchesSelector(string selector)
    {
        if (selector.StartsWith("."))
        {
            var className = selector.Substring(1);
            var classAttr = GetAttribute("class");
            return classAttr != null && classAttr.Split(' ').Contains(className);
        }

        if (selector.StartsWith("#"))
        {
            var id = selector.Substring(1);
            return GetAttribute("id") == id;
        }

        if (!selector.StartsWith("[") || !selector.EndsWith("]"))
        {
            return TagName == selector;
        }
        
        var attrSelector = selector.Trim('[', ']');
        var parts = attrSelector.Split('=');
        var attrName = parts[0];
        var attrValue = parts.Length > 1 ? parts[1].Trim('"', '\'') : null;

        if (Attributes.TryGetValue(attrName, out string? value))
        {
            return attrValue == null || value == attrValue;
        }

        return false;
    }
}

