public class GreaterEqualExpr : BinaryExpr
{
    public GreaterEqualExpr(Expr left, Token opToken, Expr right, List<CompilingError> CompilingErrors)
        : base(left, opToken, right, CompilingErrors) { }

    

    public override bool IsNumeric(SemanticContext context) => false;
    public override bool IsBoolean(SemanticContext context) => true;

    public override void CheckSemantics(SemanticContext context)
    {
        Left.CheckSemantics(context);
        Right.CheckSemantics(context);

        if (!Left.IsNumeric(context) || !Right.IsNumeric(context))
            CompilingErrors.Add(new CompilingError(Operator.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"La comparación debe ser entre números"));
    }

    public override string ToString() => $"({Left} >= {Right})";
}