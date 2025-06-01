public class FunctionCallExpr : Expr
    {
        public string FunctionName { get; }
        public List<Expr> Arguments { get; }

        public FunctionCallExpr(Token identifier, List<Expr> arguments) 
            : base(identifier)
        {
            FunctionName = identifier.Lexeme;
            Arguments = arguments;
        }

         public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
    }