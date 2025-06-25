public class EqualExpr : BinaryExpr
{
    public EqualExpr(Expr left, Token opToken, Expr right, List<CompilingError> CompilingErrors)
        : base(left, opToken, right, CompilingErrors) { }

    

    public override bool IsNumeric(SemanticContext context) => false;
    public override bool IsBoolean(SemanticContext context) => true;

    public override void CheckSemantics(SemanticContext context)
    {
        Left.CheckSemantics(context);
        Right.CheckSemantics(context);

        // Permitir comparación entre tipos compatibles
        if (Left.IsNumeric(context) != Right.IsNumeric(context) && 
            Left.IsBoolean(context) != Right.IsBoolean(context))
        {
            CompilingErrors.Add(new CompilingError(Operator.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"No se pueden comparar tipos diferentes"));
        }
    }

    public override string ToString() => $"({Left} == {Right})";
}