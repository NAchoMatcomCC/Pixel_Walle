public abstract class ASTNode
    {
        public Token StartToken { get; }

        protected ASTNode(Token startToken)
        {
            StartToken = startToken;
        }


        public abstract void CheckSemantics(SemanticContext context);

        public abstract void Accept(INodeVisitor visitor);
    }