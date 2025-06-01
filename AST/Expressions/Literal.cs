public class LiteralExpr : Expr
    {
        public object Value { get; }

        public LiteralExpr(Token token) 
            : base(token)
        {
            Value = token.Literal;
        }

        public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
    }