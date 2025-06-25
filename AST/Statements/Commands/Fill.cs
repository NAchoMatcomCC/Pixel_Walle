public class FillStmt : Stmt
    {
        public FillStmt(Token fillToken, List<CompilingError> CompilingErrors) : base(fillToken, CompilingErrors) { }


       

        public override void CheckSemantics(SemanticContext context)
        {
            // Nada que chequear semánticamente en Fill
        }


        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return $"Fill()";
        }

    }