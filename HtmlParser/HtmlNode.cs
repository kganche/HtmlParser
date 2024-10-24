namespace HtmlParser;

public abstract class HtmlNode
{
    public abstract string? TagName { get; }

    public virtual void Add(HtmlNode node) { }

    public virtual void Remove(HtmlNode node) { }

    public virtual List<HtmlNode> GetChildren() => [];

    public abstract string ToHtml();
}