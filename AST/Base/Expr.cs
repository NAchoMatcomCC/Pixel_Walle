public abstract class Expr : ASTNode
    {
        public Expr(Token startToken, List<CompilingError> CompilingErrors) : base(startToken, CompilingErrors) { }

        public abstract bool IsNumeric(SemanticContext context);

        public abstract bool IsBoolean(SemanticContext context);

        public override void Accept(INodeVisitor visitor)
        {
            throw new InvalidOperationException("Expressions do not support Accept for execution.");
        }

        
    }