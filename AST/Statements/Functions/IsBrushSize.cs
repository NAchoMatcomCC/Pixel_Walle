public class IsBrushSize : Expr
    {
        public Expr SizeValue { get; }

        public IsBrushSize(Token sizeToken, Expr sizeValue) 
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

        public override bool IsNumeric(SemanticContext context) => false;
        public override bool IsBoolean(SemanticContext context) => true;
        

        public override string ToString()
        {
            return $"IsBrushSizeSize({SizeValue})";
        }

        

    }