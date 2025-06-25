public class DivideExpr : BinaryExpr
{
    public DivideExpr(Expr left, Token opToken, Expr right, List<CompilingError> CompilingErrors)
        : base(left, opToken, right, CompilingErrors) { }

    

    public override bool IsNumeric(SemanticContext context) => true;
    public override bool IsBoolean(SemanticContext context) => false;

    public override void CheckSemantics(SemanticContext context)
    {
        Left.CheckSemantics(context);
        Right.CheckSemantics(context);

        if (!Left.IsNumeric(context) || !Right.IsNumeric(context))
            CompilingErrors.Add(new CompilingError(Operator.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
            $"Se deben dividir números"));
        
        // Verificar división por cero en tiempo de compilación si es posible
        if (Right is Literal lit && Convert.ToDouble(lit.Value) == 0)
        CompilingErrors.Add(new CompilingError(Operator.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"No se admite dividir entre cero"));
    }

    public override string ToString() => $"({Left} / {Right})";

    
}
