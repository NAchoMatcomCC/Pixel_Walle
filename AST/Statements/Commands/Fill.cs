public class FillStmt : Stmt
    {
        public FillStmt(Token fillToken) : base(fillToken) { }


        public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);

        public override void CheckSemantics(SemanticContext context)
        {
            // Nada que chequear semánticamente en Fill
        }
    }