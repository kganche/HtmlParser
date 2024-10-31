using FluentAssertions;
using HtmlParser.Exceptions;
using HtmlParser.Models;

namespace HtmlParserTests;

public class HtmlDocumentTests
{
    [Fact] 
    public void Load_ValidHtml_ReturnsHtmlDocument()
    {
        var html = "<html><body><p>Hello World!</p></body></html>";

        var document = HtmlDocument.Load(html);

        document.Should().NotBeNull();
        document.ToHtml().Should().Be(html);
    }
    
    [Fact]
    public void Load_InvalidHtml_ThrowsParsingException()
    {
        var invalidHtml = "<html><body><p>Hello World!</body></html>";

        Action act = () => HtmlDocument.Load(invalidHtml);

        act.Should().Throw<HtmlParsingException>()
            .WithMessage("Html parsing failed due to an error!")
            .WithInnerException<HtmlParsingException>("Parsing failed: unmatched tags found in the HTML.!");
    }
    
    [Fact]
    public void Load_EmptyHtml_ThrowsParsingException()
    {
        var emptyHtml = "";

        Action act = () => HtmlDocument.Load(emptyHtml);

        act.Should().Throw<HtmlParsingException>()
            .WithMessage("Html cannot be null, empty or whitespace!");
    }
    
    [Fact]
    public void Load_NullHtml_ThrowsParsingException()
    {
        string? emptyHtml = null;

        Action act = () => HtmlDocument.Load(emptyHtml);

        act.Should().Throw<HtmlParsingException>()
            .WithMessage("Html cannot be null, empty or whitespace!");
    }
    
    [Fact]
    public void Load_WhitespaceHtml_ThrowsParsingException()
    {
        var emptyHtml = " ";

        Action act = () => HtmlDocument.Load(emptyHtml);

        act.Should().Throw<HtmlParsingException>()
            .WithMessage("Html cannot be null, empty or whitespace!");
    }
}