public class SizeStmt : Stmt
    {
        public Expr SizeValue { get; }

        public SizeStmt(Token sizeToken, Expr sizeValue, List<CompilingError> CompilingErrors) 
            : base(sizeToken, CompilingErrors)
        {
            SizeValue = sizeValue;
        }

        

        public override void CheckSemantics(SemanticContext context)
        {
            SizeValue.CheckSemantics(context);

            if (!SizeValue.IsNumeric(context))
                CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"Size espera un argumento num'erico"));
        }


        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return $"Size({SizeValue})";
        }

    }