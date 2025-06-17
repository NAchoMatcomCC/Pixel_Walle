public class GetActualY : Expr
    {
        public Expr SizeValue { get; }

        public GetActualY(Token sizeToken) 
            : base(sizeToken)
        {
            
        }

        

        public override void CheckSemantics(SemanticContext context)
        {
            
        }

        public override bool IsNumeric(SemanticContext context) => true;
        public override bool IsBoolean(SemanticContext context) => false;


        
        public override string ToString()
        {
            return $"GetActualY()";
        }

    }