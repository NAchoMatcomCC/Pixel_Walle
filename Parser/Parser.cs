public class PixelParser
{
    public TokenStream Stream { get; private set; }

    public PixelParser(TokenStream stream)
    {
        Stream = stream;
    }

    public List<ASTNode> ParseProgram(List<CompilingError> errors)
    {
        List<ASTNode> statements = new();

        while (Stream.CanLookAhead(0))
        {
            ASTNode? stmt = ParseStatement(errors);
            if (stmt != null)
                statements.Add(stmt);
            else
                break;
        }

        return statements;
    }

    private ASTNode? ParseStatement(List<CompilingError> errors)
    {
        Token current = Stream.LookAhead();

        if (current.Type == TokenType.IDENTIFIER && Stream.LookAhead(1).Lexeme == "<-")
            return ParseAssignment(errors);

        if (current.Type == TokenType.KEYWORD)
        {
            switch (current.Lexeme)
            {
                case "Spawn": return ParseSpawn(errors);
                case "Color": return ParseColor(errors);
                case "Size": return ParseSize(errors);
                case "DrawLine": return ParseDrawLine(errors);
                case "DrawCircle": return ParseDrawCircle(errors);
                case "DrawRectangle": return ParseDrawRectangle(errors);
                case "Fill": return ParseFill(errors);
                case "Goto": return ParseGoto(errors);
                case "Label": return ParseLabel(errors);
                default:
                    errors.Add(new CompilingError(current.Line, ErrorCode.Invalid, $"Unknown command '{current.Lexeme}'"));
                    return null;
            }
        }

        errors.Add(new CompilingError(current.Line, ErrorCode.Invalid, $"Unexpected token '{current.Lexeme}'"));
        return null;
    }

    private AssignmentStmt ParseAssignment(List<CompilingError> errors)
    {
        string name = Stream.LookAhead().Lexeme;
        Stream.Next(); // consume identifier

        if (!Stream.Next("<-"))
            errors.Add(new CompilingError(Stream.LookAhead().Line, ErrorCode.Expected, "'<-' expected in assignment."));

        Expr value = ParseExpression(errors);
        return new AssignmentStmt(Stream.LookAhead(), value);
    }

    private SpawnStmt ParseSpawn(List<CompilingError> errors)
    {
        Stream.Next(); // consume 'Spawn'
        Expect("(", errors);
        Expr x = ParseExpression(errors);
        Expect(",", errors);
        Expr y = ParseExpression(errors);
        Expect(")", errors);
        return new SpawnStmt(Stream.LookAhead(),x, y);
    }

    private ColorCommand ParseColor(List<CompilingError> errors)
    {
        Stream.Next(); // consume 'Color'
        Expect("(", errors);
        if (!Stream.Next(TokenType.STRING))
            errors.Add(new CompilingError(Stream.LookAhead().Line, ErrorCode.Expected, "Expected color string"));
        string color = Stream.LookAhead().Lexeme;
        Expect(")", errors);
        return new ColorCommand(Stream.LookAhead(),color);
    }

    private SizeStmt ParseSize(List<CompilingError> errors)
    {
        Stream.Next(); // consume 'Size'
        Expect("(", errors);
        Expr size = ParseExpression(errors);
        Expect(")", errors);
        return new SizeStmt(Stream.LookAhead(),size);
    }

    private DrawLineStmt ParseDrawLine(List<CompilingError> errors)
    {
        Stream.Next();
        Expect("(", errors);
        var dx = ParseExpression(errors);
        Expect(",", errors);
        var dy = ParseExpression(errors);
        Expect(",", errors);
        var dist = ParseExpression(errors);
        Expect(")", errors);
        return new DrawLineStmt(Stream.LookAhead(),dx, dy, dist);
    }

    private DrawCircleStmt ParseDrawCircle(List<CompilingError> errors)
    {
        Stream.Next();
        Expect("(", errors);
        var dx = ParseExpression(errors);
        Expect(",", errors);
        var dy = ParseExpression(errors);
        Expect(",", errors);
        var radius = ParseExpression(errors);
        Expect(")", errors);
        return new DrawCircleStmt(Stream.LookAhead(),dx, dy, radius);
    }

    private DrawRectangleStmt ParseDrawRectangle(List<CompilingError> errors)
    {
        Stream.Next();
        Expect("(", errors);
        var dx = ParseExpression(errors);
        Expect(",", errors);
        var dy = ParseExpression(errors);
        Expect(",", errors);
        var dist = ParseExpression(errors);
        Expect(",", errors);
        var w = ParseExpression(errors);
        Expect(",", errors);
        var h = ParseExpression(errors);
        Expect(")", errors);
        return new DrawRectangleStmt(Stream.LookAhead(),dx, dy, dist, w, h);
    }

    private FillStmt ParseFill(List<CompilingError> errors)
    {
        Stream.Next();
        Expect("(", errors);
        Expect(")", errors);
        return new FillStmt(Stream.LookAhead());
    }

    private Label ParseLabel(List<CompilingError> errors)
    {
        Stream.Next(); // consume Label
        if (!Stream.Next(TokenType.IDENTIFIER))
            errors.Add(new CompilingError(Stream.LookAhead().Line, ErrorCode.Expected, "Expected label name"));
        return new Label(Stream.LookAhead().Lexeme, Stream.LookAhead());
    }

    private GoTo ParseGoto(List<CompilingError> errors)
    {
        Stream.Next(); // consume Goto

        if (!Stream.Next("[") || !Stream.Next(TokenType.IDENTIFIER))
            errors.Add(new CompilingError(Stream.LookAhead().Line, ErrorCode.Expected, "Expected label in brackets"));

        string label = Stream.LookAhead().Lexeme;

        if (!Stream.Next("]"))
            errors.Add(new CompilingError(Stream.LookAhead().Line, ErrorCode.Expected, "Expected ']'"));

        if (!Stream.Next("("))
            errors.Add(new CompilingError(Stream.LookAhead().Line, ErrorCode.Expected, "Expected '(' after label"));

        Expr cond = ParseExpression(errors);

        if (!Stream.Next(")"))
            errors.Add(new CompilingError(Stream.LookAhead().Line, ErrorCode.Expected, "Expected ')' after condition"));

        return new GoTo(label, cond, Stream.LookAhead());
    }

    private void Expect(string expected, List<CompilingError> errors)
    {
        if (!Stream.Next(expected))
            errors.Add(new CompilingError(Stream.LookAhead().Line, ErrorCode.Expected, $"Expected '{expected}'"));
    }



    public Expr ParseExpression(List<CompilingError> errors)
    {
        return ParseExpressionLv1(null, errors);
    }

    private Expr ParseExpressionLv1(Expr? left, List<CompilingError> errors)
    {
        var newLeft = ParseExpressionLv2(left, errors);
        return ParseExpressionLv1_(newLeft, errors);
    }

    private Expr ParseExpressionLv1_(Expr? left, List<CompilingError> errors)
    {
        Expr? exp = ParseOr(left, errors);
        if (exp != null) return ParseExpressionLv1_(exp, errors);

        return left!;
    }

    private Expr ParseExpressionLv2(Expr? left, List<CompilingError> errors)
    {
        var newLeft = ParseExpressionLv3(left, errors);
        return ParseExpressionLv2_(newLeft, errors);
    }

    private Expr ParseExpressionLv2_(Expr? left, List<CompilingError> errors)
    {
        Expr? exp = ParseAnd(left, errors);
        if (exp != null) return ParseExpressionLv2_(exp, errors);

        return left!;
    }

    private Expr ParseExpressionLv3(Expr? left, List<CompilingError> errors)
    {
        var newLeft = ParseExpressionLv4(left, errors);
        return ParseExpressionLv3_(newLeft, errors);
    }

    private Expr ParseExpressionLv3_(Expr? left, List<CompilingError> errors)
    {
        Expr? exp = ParseEqual(left, errors)
            ?? ParseGreater(left, errors)
            ?? ParseLess(left, errors)
            ?? ParseGreaterEqual(left, errors)
            ?? ParseLessEqual(left, errors);
        if (exp != null) return ParseExpressionLv3_(exp, errors);

        return left!;
    }

    private Expr ParseExpressionLv4(Expr? left, List<CompilingError> errors)
    {
        var newLeft = ParseExpressionLv5(left, errors);
        return ParseExpressionLv4_(newLeft, errors);
    }

    private Expr ParseExpressionLv4_(Expr? left, List<CompilingError> errors)
    {
        Expr? exp = ParseAdd(left, errors) ?? ParseSub(left, errors);
        if (exp != null) return ParseExpressionLv4_(exp, errors);

        return left!;
    }

    private Expr ParseExpressionLv5(Expr? left, List<CompilingError> errors)
    {
        var newLeft = ParseExpressionLv6(left, errors);
        return ParseExpressionLv5_(newLeft, errors);
    }

    private Expr ParseExpressionLv5_(Expr? left, List<CompilingError> errors)
    {
        Expr? exp = ParseMul(left, errors) ?? ParseDiv(left, errors) ?? ParseMod(left, errors);
        if (exp != null) return ParseExpressionLv5_(exp, errors);

        return left!;
    }

    private Expr ParseExpressionLv6(Expr? left, List<CompilingError> errors)
    {
        Expr? exp = ParseUnary(errors)
            ?? ParseGrouping(errors)
            ?? ParseLiteral(errors)
            ?? ParseVar(errors);
            //?? ParseFunctionCall(errors);

        return exp ?? new Literal(0,Stream.LookAhead()); // fallback literal
    }

    // Operadores
    private Expr? ParseAdd(Expr? left, List<CompilingError> errors)
    {
        if (left == null || !Stream.Next("+")) return null;
        var right = ParseExpressionLv5(null, errors);
        return new AddExpr(left, Stream.LookAhead(), right);
    }

    private Expr? ParseSub(Expr? left, List<CompilingError> errors)
    {
        if (left == null || !Stream.Next("-")) return null;
        var right = ParseExpressionLv5(null, errors);
        return new SubtractExpr(left, Stream.LookAhead(), right);
    }

    private Expr? ParseMul(Expr? left, List<CompilingError> errors)
    {
        if (left == null || !Stream.Next("*")) return null;
        var right = ParseExpressionLv6(null, errors);
        return new MultiplyExpr(left,Stream.LookAhead(), right);
    }

    private Expr? ParseDiv(Expr? left, List<CompilingError> errors)
    {
        if (left == null || !Stream.Next("/")) return null;
        var right = ParseExpressionLv6(null, errors);
        return new DivideExpr(left, Stream.LookAhead(),right);
    }

    private Expr? ParseMod(Expr? left, List<CompilingError> errors)
    {
        if (left == null || !Stream.Next("%")) return null;
        var right = ParseExpressionLv6(null, errors);
        return new ModuloExpr(left, Stream.LookAhead(),right);
    }

    private Expr? ParseEqual(Expr? left, List<CompilingError> errors)
    {
        if (left == null || !Stream.Next("==")) return null;
        var right = ParseExpressionLv4(null, errors);
        return new EqualExpr(left, Stream.LookAhead(),right);
    }

    private Expr? ParseGreater(Expr? left, List<CompilingError> errors)
    {
        if (left == null || !Stream.Next(">")) return null;
        var right = ParseExpressionLv4(null, errors);
        return new GreaterExpr(left, Stream.LookAhead(),right);
    }

    private Expr? ParseLess(Expr? left, List<CompilingError> errors)
    {
        if (left == null || !Stream.Next("<")) return null;
        var right = ParseExpressionLv4(null, errors);
        return new LessExpr(left, Stream.LookAhead(),right);
    }

    private Expr? ParseGreaterEqual(Expr? left, List<CompilingError> errors)
    {
        if (left == null || !Stream.Next(">=")) return null;
        var right = ParseExpressionLv4(null, errors);
        return new GreaterEqualExpr(left, Stream.LookAhead(),right);
    }

    private Expr? ParseLessEqual(Expr? left, List<CompilingError> errors)
    {
        if (left == null || !Stream.Next("<=")) return null;
        var right = ParseExpressionLv4(null, errors);
        return new LessEqualExpr(left, Stream.LookAhead(),right);
    }

    private Expr? ParseAnd(Expr? left, List<CompilingError> errors)
    {
        if (left == null || !Stream.Next("&&")) return null;
        var right = ParseExpressionLv3(null, errors);
        return new AndExpr(left, Stream.LookAhead(),right);
    }

    private Expr? ParseOr(Expr? left, List<CompilingError> errors)
    {
        if (left == null || !Stream.Next("||")) return null;
        var right = ParseExpressionLv3(null, errors);
        return new OrExpr(left,Stream.LookAhead(), right);
    }

    // Literales, variables, etc.
    private Expr? ParseLiteral(List<CompilingError> errors)
    {
        if (Stream.Next(TokenType.NUMBER))
            return new Literal(int.Parse(Stream.LookAhead().Lexeme), Stream.LookAhead());
        if (Stream.Next(TokenType.STRING))
            return new Literal(Stream.LookAhead().Lexeme, Stream.LookAhead());
        return null;
    }

    private Expr? ParseVar(List<CompilingError> errors)
    {
        if (!Stream.Next(TokenType.IDENTIFIER)) return null;
        return new Var(Stream.LookAhead().Lexeme, Stream.LookAhead());
    }

    private Expr? ParseGrouping(List<CompilingError> errors)
    {
        if (!Stream.Next("(")) return null;
        var expr = ParseExpression(errors);
        if (!Stream.Next(")"))
            errors.Add(new CompilingError(Stream.LookAhead().Line, ErrorCode.Expected, "')' expected"));
        return new Grouping(expr, Stream.LookAhead());
    }

    private Expr? ParseUnary(List<CompilingError> errors)
    {
        if (Stream.Next("!"))
        {
            var operand = ParseExpressionLv6(null, errors);
            return new NotExpr(operand, Stream.LookAhead());
        }
        if (Stream.Next("-"))
        {
            var operand = ParseExpressionLv6(null, errors);
            return new NegateExpr(operand, Stream.LookAhead());
        }
        return null;
    }

    /*private Expr? ParseFunctionCall(List<CompilingError> errors)
    {
        if (!Stream.Next(TokenType.IDENTIFIER)) return null;
        string name = Stream.LookAhead().Lexeme;

        if (!Stream.Next("(")) return null;

        List<Expr> args = new();
        if (!Stream.LookAhead().Lexeme.Equals(")"))
        {
            args.Add(ParseExpression(errors));
            while (Stream.Next(","))
            {
                args.Add(ParseExpression(errors));
            }
        }

        if (!Stream.Next(")"))
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "')' expected"));

        return new Fun(Stream.LookAhead(),args);
    }*/
}

