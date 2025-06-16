using System;
using System.Collections.Generic;
using System.Linq;

public class Parser
{
    private IEnumerator<Token> tokens;
    private Token currentToken;
    private readonly SemanticContext context = new SemanticContext();
    public List<CompilingError> Errors { get; } = new List<CompilingError>();

    public Parser(IEnumerable<Token> tokens)
    {
        this.tokens = tokens.GetEnumerator();
        this.tokens.MoveNext();
        currentToken = this.tokens.Current;
    }

    private void Eat(TokenType expectedType)
    {
        if (currentToken.Type == expectedType)
        {
            tokens.MoveNext();
            currentToken = tokens.Current;
        }
        else
        {
            Errors.Add(new CompilingError(currentToken.Line, ErrorCode.Expected, 
                $"Se esperaba {expectedType} pero se encontró {currentToken.Type}"));
            throw new ParseException($"Syntax Error: Expected {expectedType} but got {currentToken.Type} at line {currentToken.Line}");
        }
    }

    public List<Stmt> Parse()
    {
        List<Stmt> statements = new List<Stmt>();
        
        while (currentToken.Type != TokenType.EOF)
        {
            try
            {
                if (currentToken.Type == TokenType.NEWLINE)
                {
                    Eat(TokenType.NEWLINE);
                    continue;
                }

                Stmt stmt = ParseStatement();
                if (stmt != null) statements.Add(stmt);
            }
            catch (ParseException)
            {
                // Ya manejado por el contexto de errores
                Synchronize();
            }
        }
        
        return statements;
    }

    private void Synchronize()
    {
        while (currentToken.Type != TokenType.EOF && 
               currentToken.Type != TokenType.NEWLINE)
        {
            tokens.MoveNext();
            currentToken = tokens.Current;
        }
        
        if (currentToken.Type == TokenType.NEWLINE)
        {
            Eat(TokenType.NEWLINE);
        }
    }

    private Stmt ParseStatement()
    {
        if (currentToken.Type == TokenType.KEYWORD)
        {
            return ParseKeywordStatement();
        }
        
        if (currentToken.Type == TokenType.IDENTIFIER)
        {
            return ParseAssignmentOrLabel();
        }
        
        Errors.Add(new CompilingError(currentToken.Line, ErrorCode.Invalid, 
            $"Token inesperado: {currentToken.Lexeme}"));
        throw new ParseException($"Token inesperado: {currentToken.Lexeme}");
    }

    private Stmt ParseKeywordStatement()
    {
        Token keyword = currentToken;
        Eat(TokenType.KEYWORD);
        
        switch (keyword.Lexeme)
        {
            case "Spawn":
                return ParseSpawn(keyword);
                
            case "Color":
                return ParseColor(keyword);
                
            case "Size":
                return ParseSize(keyword);
                
            case "DrawLine":
                return ParseDrawLine(keyword);
                
            case "DrawCircle":
                return ParseDrawCircle(keyword);
                
            case "DrawRectangle":
                return ParseDrawRectangle(keyword);
                
            case "Fill":
                return ParseFill(keyword);
                
            case "GoTo":
                return ParseGoTo(keyword);
            case "GetActualX":
        
                
            default:
                Errors.Add(new CompilingError(keyword.Line, ErrorCode.Invalid, 
                    $"Instrucción desconocida: {keyword.Lexeme}"));
                throw new ParseException($"Instrucción desconocida: {keyword.Lexeme}");
        }
    }

    private Stmt ParseSpawn(Token keyword)
    {
        if (context.SpawnCalled)
        {
            Errors.Add(new CompilingError(keyword.Line, ErrorCode.Invalid, 
                "Spawn solo puede llamarse una vez"));
            throw new ParseException("Spawn solo puede llamarse una vez");
        }
        
        Eat(TokenType.LEFT_PAREN);
        Expr x = ParseExpression();
        Eat(TokenType.COMMA);
        Expr y = ParseExpression();
        Eat(TokenType.RIGHT_PAREN);
        
        context.SpawnCalled = true;
        return new SpawnStmt(keyword, x, y);
    }

    private Stmt ParseColor(Token keyword)
    {
        Eat(TokenType.LEFT_PAREN);
        Expr color = ParseExpression();
        Eat(TokenType.RIGHT_PAREN);
        return new ColorCommand(keyword, color);
    }

    private Stmt ParseSize(Token keyword)
    {
        Eat(TokenType.LEFT_PAREN);
        Expr size = ParseExpression();
        Eat(TokenType.RIGHT_PAREN);
        return new SizeStmt(keyword, size);
    }

    private Stmt ParseDrawLine(Token keyword)
    {
        Eat(TokenType.LEFT_PAREN);
        Expr dirX = ParseExpression();
        Eat(TokenType.COMMA);
        Expr dirY = ParseExpression();
        Eat(TokenType.COMMA);
        Expr distance = ParseExpression();
        Eat(TokenType.RIGHT_PAREN);
        return new DrawLineStmt(keyword, dirX, dirY, distance);
    }

    private Stmt ParseDrawCircle(Token keyword)
    {
        Eat(TokenType.LEFT_PAREN);
        Expr dirX = ParseExpression();
        Eat(TokenType.COMMA);
        Expr dirY = ParseExpression();
        Eat(TokenType.COMMA);
        Expr radius = ParseExpression();
        Eat(TokenType.RIGHT_PAREN);
        return new DrawCircleStmt(keyword, dirX, dirY, radius);
    }

    private Stmt ParseDrawRectangle(Token keyword)
    {
        Eat(TokenType.LEFT_PAREN);
        Expr dirX = ParseExpression();
        Eat(TokenType.COMMA);
        Expr dirY = ParseExpression();
        Eat(TokenType.COMMA);
        Expr distance = ParseExpression();
        Eat(TokenType.COMMA);
        Expr width = ParseExpression();
        Eat(TokenType.COMMA);
        Expr height = ParseExpression();
        Eat(TokenType.RIGHT_PAREN);
        return new DrawRectangleStmt(keyword, dirX, dirY, distance, width, height);
    }

    private Stmt ParseFill(Token keyword)
    {
        Eat(TokenType.LEFT_PAREN);
        Eat(TokenType.RIGHT_PAREN);
        return new FillStmt(keyword);
    }

    private Stmt ParseGoTo(Token keyword)
    {
        Eat(TokenType.LEFT_BRACKET);
        Token labelToken = currentToken;
        Eat(TokenType.IDENTIFIER);
        Eat(TokenType.RIGHT_BRACKET);
        Eat(TokenType.LEFT_PAREN);
        Expr condition = ParseExpression();
        Eat(TokenType.RIGHT_PAREN);
        return new GoTo(labelToken.Lexeme, condition, keyword);
    }

    private Stmt ParseAssignmentOrLabel()
    {
        Token identifier = currentToken;
        Eat(TokenType.IDENTIFIER);

        if (currentToken.Type == TokenType.ASSIGNMENT)
        {
            Eat(TokenType.ASSIGNMENT);
            Expr expr = ParseExpression();
            return new AssignmentStmt(identifier, expr);
        }
        else
        {
            return new Label(identifier.Lexeme, identifier);
        }
    }

    // Continuación con los métodos de análisis de expresiones...

    private Expr ParseExpression()
{
    return ParseLogicalOr();
}

private Expr ParseLogicalOr()
{
    Expr expr = ParseLogicalAnd();

    while (currentToken.Type == TokenType.OR)
    {
        Token op = currentToken;
        Eat(TokenType.OR);
        Expr right = ParseLogicalAnd();
        expr = new OrExpr(expr, op, right);
    }

    return expr;
}

private Expr ParseLogicalAnd()
{
    Expr expr = ParseEquality();

    while (currentToken.Type == TokenType.AND)
    {
        Token op = currentToken;
        Eat(TokenType.AND);
        Expr right = ParseEquality();
        expr = new AndExpr(expr, op, right);
    }

    return expr;
}

private Expr ParseEquality()
{
    Expr expr = ParseComparison();

    while (currentToken.Type == TokenType.EQUAL_EQUAL || currentToken.Type == TokenType.BANG_EQUAL)
    {
        Token op = currentToken;
        Eat(currentToken.Type);
        Expr right = ParseComparison();
        
        expr = op.Type == TokenType.EQUAL_EQUAL ? 
            (Expr)new EqualExpr(expr, op, right) : 
            new NotEqualExpr(expr, op, right);
    }

    return expr;
}

private Expr ParseComparison()
{
    Expr expr = ParseTerm();

    while (currentToken.Type == TokenType.GREATER || currentToken.Type == TokenType.GREATER_EQUAL ||
           currentToken.Type == TokenType.LESS || currentToken.Type == TokenType.LESS_EQUAL)
    {
        Token op = currentToken;
        Eat(currentToken.Type);
        Expr right = ParseTerm();
        
        expr = op.Type switch
        {
            TokenType.GREATER => new GreaterExpr(expr, op, right),
            TokenType.GREATER_EQUAL => new GreaterEqualExpr(expr, op, right),
            TokenType.LESS => new LessExpr(expr, op, right),
            TokenType.LESS_EQUAL => new LessEqualExpr(expr, op, right),
            _ => expr
        };
    }

    return expr;
}

private Expr ParseTerm()
{
    Expr expr = ParseFactor();

    while (currentToken.Type == TokenType.PLUS || currentToken.Type == TokenType.MINUS)
    {
        Token op = currentToken;
        Eat(currentToken.Type);
        Expr right = ParseFactor();
        expr = op.Type == TokenType.PLUS ? 
            (Expr)new AddExpr(expr, op, right) : 
            new SubtractExpr(expr, op, right);
    }

    return expr;
}

private Expr ParseFactor()
{
    Expr expr = ParseUnary();

    while (currentToken.Type == TokenType.STAR || currentToken.Type == TokenType.SLASH || 
           currentToken.Type == TokenType.PERCENT)
    {
        Token op = currentToken;
        Eat(currentToken.Type);
        Expr right = ParseUnary();
        
        expr = op.Type switch
        {
            TokenType.STAR => new MultiplyExpr(expr, op, right),
            TokenType.SLASH => new DivideExpr(expr, op, right),
            TokenType.PERCENT => new ModuloExpr(expr, op, right),
            _ => expr
        };
    }

    return expr;
}

private Expr ParseUnary()
{
    if (currentToken.Type == TokenType.BANG || currentToken.Type == TokenType.MINUS)
    {
        Token op = currentToken;
        Eat(currentToken.Type);
        Expr right = ParseUnary();
        
        return op.Type == TokenType.BANG ? 
            (Expr)new NotExpr(right, op) : 
            new NegateExpr(right, op);
    }

    return ParsePrimary();
}

private Expr ParsePrimary()
{
    if (currentToken.Type == TokenType.NUMBER)
    {
        int value = int.Parse(currentToken.Lexeme);
        Token token = currentToken;
        Eat(TokenType.NUMBER);
        return new Literal(value, token);
    }
    
    if (currentToken.Type == TokenType.STRING)
    {
        string value = (string)currentToken.Literal;
        Token token = currentToken;
        Eat(TokenType.STRING);
        return new Literal(value, token);
    }
    
    if (currentToken.Type == TokenType.TRUE || currentToken.Type == TokenType.FALSE)
    {
        bool value = currentToken.Type == TokenType.TRUE;
        Token token = currentToken;
        Eat(currentToken.Type);
        return new Literal(value, token);
    }
    
    if (currentToken.Type == TokenType.IDENTIFIER)
    {
        string identifier = currentToken.Lexeme;
        Token token = currentToken;
        Eat(TokenType.IDENTIFIER);

        if (currentToken.Type == TokenType.LEFT_PAREN)
        {
            Eat(TokenType.LEFT_PAREN);
            List<Expr> args = new List<Expr>();

            if (currentToken.Type != TokenType.RIGHT_PAREN)
            {
                args.Add(ParseExpression());
                while (currentToken.Type == TokenType.COMMA)
                {
                    Eat(TokenType.COMMA);
                    args.Add(ParseExpression());
                }
            }
            Eat(TokenType.RIGHT_PAREN);
            return new FunctionCall(identifier, args, token);
        }

        return new Var(identifier, token);
    }
    
    if (currentToken.Type == TokenType.LEFT_PAREN)
    {
        Eat(TokenType.LEFT_PAREN);
        Expr expr = ParseExpression();
        Eat(TokenType.RIGHT_PAREN);
        return new Grouping(expr, currentToken);
    }
    
    Errors.Add(new CompilingError(currentToken.Line, ErrorCode.Invalid, 
        $"Expresión inválida: {currentToken.Lexeme}"));
    throw new ParseException($"Expresión inválida: {currentToken.Lexeme}");
}
}

public class ParseException : Exception
{
    public ParseException(string message) : base(message) { }
}