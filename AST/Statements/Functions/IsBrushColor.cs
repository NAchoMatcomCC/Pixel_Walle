public class IsBrushColor : Expr
    {
        public Expr ColorExpression { get; }


         private static readonly HashSet<string> AllowedColors = new()
    {
        "Red", "Blue", "Green", "Yellow", "Orange",
        "Purple", "Black", "White", "Transparent"
    };
        public IsBrushColor(Token colorToken, Expr colorExpression, List<CompilingError> CompilingErrors) 
        : base(colorToken, CompilingErrors)
    {
        ColorExpression = colorExpression;
    }

    public override void CheckSemantics(SemanticContext context)
    {
        ColorExpression.CheckSemantics(context);
        
        // Check if it's a string literal
        if (ColorExpression is Literal literal && literal.Value is string colorValue)
        {
            if (!AllowedColors.Contains(colorValue))
                CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"Color {colorValue} no v'alido"));
        }
        // Else: runtime check will be needed
    }


        public override bool IsNumeric(SemanticContext context) => false;
        public override bool IsBoolean(SemanticContext context) => true;

        public override string ToString()
        {
            return $"IsBrushColor({ColorExpression})";
        }

    }