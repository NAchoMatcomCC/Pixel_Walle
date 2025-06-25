public class IsBrushSize : Expr
    {
        public Expr SizeValue { get; }

        public IsBrushSize(Token sizeToken, Expr sizeValue, List<CompilingError> CompilingErrors) 
            : base(sizeToken, CompilingErrors)
        {
            SizeValue = sizeValue;
        }

        

        public override void CheckSemantics(SemanticContext context)
        {
            SizeValue.CheckSemantics(context);

            if (!SizeValue.IsNumeric(context))
                CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"IsBrushSize espera un argumento num'erico"));
        }

        public override bool IsNumeric(SemanticContext context) => false;
        public override bool IsBoolean(SemanticContext context) => true;
        

        public override string ToString()
        {
            return $"IsBrushSizeSize({SizeValue})";
        }

        

    }