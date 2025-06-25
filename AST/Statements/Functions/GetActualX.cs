public class GetActualX : Expr
    {
        public Expr SizeValue { get; }

        public GetActualX(Token sizeToken, List<CompilingError> CompilingErrors) 
            : base(sizeToken, CompilingErrors)
        {
       
        }

        

        public override void CheckSemantics(SemanticContext context)
        {
            
        }

        public override bool IsNumeric(SemanticContext context) => true;
        public override bool IsBoolean(SemanticContext context) => false;

        

        public override string ToString()
        {
            return $"GetActualX()";
        }

    }