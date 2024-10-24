namespace HtmlParser;

public class TextNode(string text) : HtmlNode
{
    public string Text { get; set; } = text;

    public override string? TagName => null;

    public override string ToHtml()
    {
        return Text;
    }
}

