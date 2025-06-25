public abstract class ASTNode
    {
        public Token StartToken { get; }

        public List<CompilingError> CompilingErrors; 


        protected ASTNode(Token startToken, List<CompilingError> compilingErrors)
        {
            StartToken = startToken;
            CompilingErrors=compilingErrors;
        }


        public abstract void CheckSemantics(SemanticContext context);

        public abstract void Accept(INodeVisitor visitor);
    }