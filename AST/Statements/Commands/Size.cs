public class SizeStmt : Stmt
    {
        public Expr SizeValue { get; }

        public SizeStmt(Token sizeToken, Expr sizeValue) 
            : base(sizeToken)
        {
            SizeValue = sizeValue;
        }

        public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);

        public override void CheckSemantics(SemanticContext context)
        {
            SizeValue.CheckSemantics(context);

            if (!SizeValue.IsNumeric(context))
                throw new Exception("Size expects a numeric argument.");
        }
    }