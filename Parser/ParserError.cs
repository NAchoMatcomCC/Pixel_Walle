public class ParseError : Exception
{
    public Token Token { get; }
    public ParseError(Token token, string message) : base($"Línea {token.Line}: {message}")
    {
        Token = token;
    }
}