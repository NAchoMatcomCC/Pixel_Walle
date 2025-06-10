public abstract class Expr : ASTNode
    {
        protected Expr(Token startToken) : base(startToken) { }

        public abstract bool IsNumeric(SemanticContext context);

        public abstract bool IsBoolean(SemanticContext context);

        public override void Accept(INodeVisitor visitor)
        {
            throw new InvalidOperationException("Expressions do not support Accept for execution.");
        }

        
    }