public class FillStmt : Stmt
    {
        public FillStmt(Token fillToken) : base(fillToken) { }


       

        public override void CheckSemantics(SemanticContext context)
        {
            // Nada que chequear semánticamente en Fill
        }


        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

    }