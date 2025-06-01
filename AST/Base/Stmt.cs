public abstract class Stmt : ASTNode
    {
        protected Stmt(Token startToken) : base(startToken) { }

        public abstract T Accept<T>(IAstVisitor<T> visitor);
    }