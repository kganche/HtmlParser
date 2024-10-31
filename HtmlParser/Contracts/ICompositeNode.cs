namespace HtmlParser.Contracts;

public interface ICompositeNode : IHtmlNode
{
    void Add(IHtmlNode node);
    
    void Remove(IHtmlNode node);
    
    List<IHtmlNode> GetChildren();
}