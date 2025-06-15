public enum TokenType
{
    ASSIGNMENT ,KEYWORD ,SPAWN, COLOR, DRAW_LINE, DRAW_CIRCLE, DRAW_RECTANGLE, FILL, GOTO, BANG, BANG_EQUAL,
    GET_ACTUAL_X, GET_ACTUAL_Y, GET_CANVAS_SIZE, GET_COLOR_COUNT, 
    IS_BRUSH_COLOR, IS_BRUSH_SIZE, IS_CANVAS_COLOR,
    NUMBER, STRING, BOOLEAN, IDENTIFIER,
    LEFT_PAREN, RIGHT_PAREN, COMMA, LEFT_BRACKET, RIGHT_BRACKET,
    COLON, ARROW, EQUAL, EQUAL_EQUAL, GREATER_EQUAL, LESS_EQUAL, GREATER, LESS, 
    PLUS, MINUS, STAR, SLASH, PERCENT, CARET, AND, OR,
    TRUE, FALSE, EOF, NEWLINE
}

public class Token
{
    public TokenType Type { get; }
    public string Lexeme { get; }
    public object Literal { get; }
    public int Line { get; }

    public Token(TokenType type, string lexeme, object literal, int line)
    {
        Type = type;
        Lexeme = lexeme;
        Literal = literal;
        Line = line;
    }

    public override string ToString()
    {
        return $"{Type} {Lexeme} {Literal ?? "null"} (Línea: {Line})";
    }
}