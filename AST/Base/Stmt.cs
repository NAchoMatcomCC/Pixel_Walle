public abstract class Stmt : ASTNode
    {
        protected Stmt(Token startToken, List<CompilingError> CompilingErrors) : base(startToken, CompilingErrors) { }

        
    }