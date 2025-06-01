public abstract class Expr : ASTNode
    {
        protected Expr(Token startToken) : base(startToken) { }

        public abstract bool IsNumeric(SemanticContext context);

        public abstract bool IsBoolean(SemanticContext context);

        public abstract T Accept<T>(IAstVisitor<T> visitor);
    }