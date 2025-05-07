    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _current = 0;

        public Parser(List<Token> tokens) => _tokens = tokens;

        public List<Stmt> Parse()
        {
            List<Stmt> statements = new List<Stmt>();
            while (!IsAtEnd())
            {
                try
                {
                    statements.Add(ParseStatement());
                }
                catch (ParseError)
                {
                    Synchronize();
                }
            }
            return statements;
        }

        #region Análisis de Statements
        private Stmt ParseStatement()
        {
            if (Match(TokenType.SPAWN)) return ParseSpawn();
            if (Match(TokenType.COLOR)) return ParseColor();
            if (Match(TokenType.DRAW_LINE)) return ParseDrawLine();
            if (Match(TokenType.IDENTIFIER)) return ParseAssignmentOrLabel();
            if (Match(TokenType.GOTO)) return ParseGoTo();

            throw Error(Peek(), "Instrucción no reconocida");
        }

        private Stmt ParseSpawn()
        {
            Consume(TokenType.LEFT_PAREN, "Se esperaba '(' después de Spawn");
            Expr x = ParseExpression();
            Consume(TokenType.COMMA, "Se esperaba ',' después de X");
            Expr y = ParseExpression();
            Consume(TokenType.RIGHT_PAREN, "Se esperaba ')' después de Y");
            return new SpawnCommand(x, y);
        }

        private Stmt ParseColor()
        {
            Consume(TokenType.LEFT_PAREN, "Se esperaba '(' después de Color");
            Expr color = ParseExpression();
            Consume(TokenType.RIGHT_PAREN, "Se esperaba ')' después del color");
            return new ColorCommand(color);
        }

        private Stmt ParseDrawLine()
        {
            Consume(TokenType.LEFT_PAREN, "Se esperaba '(' después de DrawLine");
            Expr dirX = ParseExpression();
            Consume(TokenType.COMMA, "Se esperaba ',' después de dirX");
            Expr dirY = ParseExpression();
            Consume(TokenType.COMMA, "Se esperaba ',' después de dirY");
            Expr distance = ParseExpression();
            Consume(TokenType.RIGHT_PAREN, "Se esperaba ')' al final de DrawLine");
            return new DrawLineCommand(dirX, dirY, distance);
        }

        private Stmt ParseAssignmentOrLabel()
        {
            Token name = Previous();
            if (Match(TokenType.ARROW))
            {
                Expr value = ParseExpression();
                return new Assignment(name, value);
            }
            else if (Match(TokenType.COLON))
            {
                return new Label(name);
            }
            throw Error(Peek(), "Se esperaba '←' o ':'");
        }

        private Stmt ParseGoTo()
        {
            Consume(TokenType.LEFT_BRACKET, "Se esperaba '[' después de GoTo");
            Token label = Consume(TokenType.IDENTIFIER, "Se esperaba nombre de etiqueta");
            Consume(TokenType.RIGHT_BRACKET, "Se esperaba ']' después de la etiqueta");
            Consume(TokenType.LEFT_PAREN, "Se esperaba '(' para la condición");
            Expr condition = ParseExpression();
            Consume(TokenType.RIGHT_PAREN, "Se esperaba ')' al final de la condición");
            return new GoTo(label, condition);
        }
        #endregion

        #region Análisis de Expresiones
        private Expr ParseExpression() => ParseLogicalOr();

        private Expr ParseLogicalOr()
        {
            Expr expr = ParseLogicalAnd();
            while (Match(TokenType.OR))
            {
                Token op = Previous();
                Expr right = ParseLogicalAnd();
                expr = new Binary(expr, op, right);
            }
            return expr;
        }

        private Expr ParseLogicalAnd()
        {
            Expr expr = ParseEquality();
            while (Match(TokenType.AND))
            {
                Token op = Previous();
                Expr right = ParseEquality();
                expr = new Binary(expr, op, right);
            }
            return expr;
        }

        private Expr ParseEquality()
        {
            Expr expr = ParseComparison();
            while (Match(TokenType.EQUAL_EQUAL, TokenType.BANG_EQUAL))
            {
                Token op = Previous();
                Expr right = ParseComparison();
                expr = new Binary(expr, op, right);
            }
            return expr;
        }

        private Expr ParseComparison()
        {
            Expr expr = ParseTerm();
            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, 
                       TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token op = Previous();
                Expr right = ParseTerm();
                expr = new Binary(expr, op, right);
            }
            return expr;
        }

        private Expr ParseTerm()
        {
            Expr expr = ParseFactor();
            while (Match(TokenType.PLUS, TokenType.MINUS))
            {
                Token op = Previous();
                Expr right = ParseFactor();
                expr = new Binary(expr, op, right);
            }
            return expr;
        }

        private Expr ParseFactor()
        {
            Expr expr = ParseUnary();
            while (Match(TokenType.STAR, TokenType.SLASH, TokenType.PERCENT))
            {
                Token op = Previous();
                Expr right = ParseUnary();
                expr = new Binary(expr, op, right);
            }
            return expr;
        }

        private Expr ParseUnary()
        {
            if (Match(TokenType.MINUS, TokenType.BANG))
            {
                Token op = Previous();
                Expr right = ParseUnary();
                return new Unary(op, right);
            }
            return ParsePrimary();
        }

        private Expr ParsePrimary()
        {
            if (Match(TokenType.NUMBER)) return new Literal(double.Parse(Previous().Lexeme));
            if (Match(TokenType.STRING)) return new Literal(Previous().Lexeme.Trim('"'));
            if (Match(TokenType.IDENTIFIER)) return new Variable(Previous());
            if (Match(TokenType.LEFT_PAREN))
            {
                Expr expr = ParseExpression();
                Consume(TokenType.RIGHT_PAREN, "Se esperaba ')' después de expresión");
                return new Grouping(expr);
            }
            throw Error(Peek(), "Expresión inválida");
        }
        #endregion

        #region Métodos Auxiliares
        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            throw Error(Peek(), message);
        }

        private bool Check(TokenType type) => !IsAtEnd() && Peek().Type == type;

        private Token Advance()
        {
            if (!IsAtEnd()) _current++;
            return Previous();
        }

        private bool IsAtEnd() => Peek().Type == TokenType.EOF;

        private Token Peek() => _tokens[_current];

        private Token Previous() => _tokens[_current - 1];

        private ParseError Error(Token token, string message) => new ParseError(token, message);

        private void Synchronize()
        {
            Advance();
            while (!IsAtEnd())
            {
                if (Previous().Type == TokenType.NEWLINE) return;

                switch (Peek().Type)
                {
                    case TokenType.SPAWN:
                    case TokenType.COLOR:
                    case TokenType.DRAW_LINE:
                    case TokenType.GOTO:
                        return;
                }

                Advance();
            }
        }
        #endregion
    }
