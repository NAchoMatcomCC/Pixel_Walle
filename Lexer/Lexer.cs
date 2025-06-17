class Lexer
{
    private string source;
    public List<Token> tokens = new List<Token>();
    private int start = 0;
    private int current = 0;
    private int line = 1;

    public Lexer(string source)
    {
        this.source = source;
    }

    private static readonly Dictionary<string, TokenType> Keywords = new()
    {
        {"Spawn", TokenType.KEYWORD},
        {"Color", TokenType.KEYWORD},
        {"DrawLine", TokenType.KEYWORD},
        {"DrawCircle", TokenType.KEYWORD},
        {"DrawRectangle", TokenType.KEYWORD},
        {"Size", TokenType.KEYWORD},
        {"Fill", TokenType.KEYWORD},
        {"GoTo", TokenType.KEYWORD},
        {"Label", TokenType.KEYWORD},
        {"GetActualX", TokenType.KEYWORD},
        {"GetActualY", TokenType.KEYWORD},
        {"GetCanvasSize", TokenType.KEYWORD},
        {"GetColorCount", TokenType.KEYWORD},
        {"IsBrushColor", TokenType.KEYWORD},
        {"IsBrushSize", TokenType.KEYWORD},
        {"IsCanvasColor", TokenType.KEYWORD},
        {"true", TokenType.TRUE},
        {"false", TokenType.FALSE}
    };

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            start = current;
            ScanToken();
        }
        tokens.Add(new Token(TokenType.EOF, "", "", line));
        return tokens;
    }

    private void ScanToken()
    {
        char c = Advance();
        switch (c)
        {
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '[': AddToken(TokenType.LEFT_BRACKET); break;
            case ']': AddToken(TokenType.RIGHT_BRACKET); break;
            case ':': AddToken(TokenType.COLON); break;
            case '+': AddToken(TokenType.PLUS); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '*': AddToken(Match('*') ? TokenType.CARET : TokenType.STAR); break;
            case '/': AddToken(TokenType.SLASH); break;
            case '%': AddToken(TokenType.PERCENT); break;
            case '=': AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
            case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
            case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : Match('-')? TokenType.ASSIGNMENT :TokenType.LESS); break;
            case '&': if (Match('&')) AddToken(TokenType.AND); break;
            case '|': if (Match('|')) AddToken(TokenType.OR); break;
            case ' ':
            case '\r':
            case '\t':
                break;
            case '\n':
                line++;
                AddToken(TokenType.NEWLINE);
                break;
            case '"': ReadString(); break;
            case '!':
                AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                break;
            default:
                if (IsDigit(c)) ReadNumber();
                else if (IsAlpha(c)) ReadIdentifier();
                
                else throw new Exception($"Carácter inesperado en línea {line}: '{c}'");
                break;
        }
    }

    private bool IsAtEnd() => current >= source.Length;
    private char Advance() => source[current++];
    private bool Match(char expected)
    {
        if (IsAtEnd() || source[current] != expected) return false;
        current++;
        return true;
    }

    private void AddToken(TokenType type, object? literal = null)
    {
        string text = source.Substring(start, current - start);
        tokens.Add(new Token(type, text, literal ?? "", line)); // Se asegura que literal no sea nulo
    }

    private void ReadNumber()
    {
        while (IsDigit(Peek())) Advance();
        string value = source.Substring(start, current - start);
        AddToken(TokenType.NUMBER, int.Parse(value));
    }

    private void ReadString()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n') line++;
            Advance();
        }

        if (IsAtEnd()) throw new Exception($"Cadena no cerrada en línea {line}");
        Advance();

        string value = source.Substring(start + 1, current - start - 2);
        AddToken(TokenType.STRING, value);
    }

    private void ReadIdentifier()
    {
        while (IsAlphaNumeric(Peek())) Advance();
        string text = source.Substring(start, current - start);
        TokenType type = Keywords.TryGetValue(text, out TokenType kwType) ? kwType : TokenType.IDENTIFIER;
        AddToken(type);
    }

    private bool IsDigit(char c) => c >= '0' && c <= '9';
    private bool IsAlpha(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c == '_' && current != start + 1);
    private bool IsAlphaNumeric(char c) => IsAlpha(c) || IsDigit(c);
    private char Peek() => IsAtEnd() ? '\0' : source[current];
}