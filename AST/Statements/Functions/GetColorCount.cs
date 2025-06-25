public class GetColorCount : Expr
    {
        public Expr ColorExpression {get;}
        public Expr DirX1 { get; }
        public Expr DirY1 {get;}
        public Expr DirX2 {get;}
        public Expr DirY2 {get;}

         private static readonly HashSet<string> AllowedColors = new()
    {
        "Red", "Blue", "Green", "Yellow", "Orange",
        "Purple", "Black", "White", "Transparent"
    };
        public GetColorCount(Token colorToken, Expr color, Expr dirx1, Expr diry1, Expr dirx2, Expr diry2, List<CompilingError> CompilingErrors) 
        : base(colorToken, CompilingErrors)
    {
        ColorExpression=color;
        DirX1=dirx1;
        DirY1=diry1;
        DirX2=dirx2;
        DirY2=diry2;
    }

    public override void CheckSemantics(SemanticContext context)
    {
        DirX1.CheckSemantics(context);
        DirY1.CheckSemantics(context);
        DirX2.CheckSemantics(context);
        DirY2.CheckSemantics(context);
        ColorExpression.CheckSemantics(context);

        if (!DirX1.IsNumeric(context) || !DirY1.IsNumeric(context) || !DirX2.IsNumeric(context) || !DirY2.IsNumeric(context))
            CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"GetColorCount espera argumentos num'ericos"));
        
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