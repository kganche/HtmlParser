namespace HtmlParser.Exceptions;

public class HtmlParsingException : Exception
{
    public HtmlParsingException() { }
    
    public HtmlParsingException(string message) 
        : base(message) { }
    
    public HtmlParsingException(string message, Exception innerException) 
        : base(message, innerException) { }
}