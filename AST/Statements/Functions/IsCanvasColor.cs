public class IsCanvasColor : Expr
    {
        public Expr ColorExpression { get; }
        public Expr DirX {get;}
        public Expr DirY {get;}

         private static readonly HashSet<string> AllowedColors = new()
    {
        "Red", "Blue", "Green", "Yellow", "Orange",
        "Purple", "Black", "White", "Transparent"
    };
        public IsCanvasColor(Token colorToken, Expr colorExpression, Expr dirx, Expr diry, List<CompilingError> CompilingErrors) 
        : base(colorToken, CompilingErrors)
    {
        DirX=dirx;
        DirY=diry;
        ColorExpression = colorExpression;
    }

    public override void CheckSemantics(SemanticContext context)
    {
        DirX.CheckSemantics(context);
        DirY.CheckSemantics(context);
        ColorExpression.CheckSemantics(context);

        if (!DirX.IsNumeric(context) || !DirY.IsNumeric(context))
           CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"IsCanvasColor espera 2 argumentos num'ericos"));
        
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