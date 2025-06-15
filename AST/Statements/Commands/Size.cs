public class SizeStmt : Stmt
    {
        public Expr SizeValue { get; }

        public SizeStmt(Token sizeToken, Expr sizeValue) 
            : base(sizeToken)
        {
            SizeValue = sizeValue;
        }

        

        public override void CheckSemantics(SemanticContext context)
        {
            SizeValue.CheckSemantics(context);

            if (!SizeValue.IsNumeric(context))
                throw new Exception("Size expects a numeric argument.");
        }


        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return $"Size({SizeValue})";
        }

    }