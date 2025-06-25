public class PowerExpr : BinaryExpr
{
    public PowerExpr(Expr left, Token opToken, Expr right, List<CompilingError> CompilingErrors)
        : base(left, opToken, right, CompilingErrors) { }

    

    public override bool IsNumeric(SemanticContext context) => true;
    public override bool IsBoolean(SemanticContext context) => false;

    public override void CheckSemantics(SemanticContext context)
    {
        Left.CheckSemantics(context);
        Right.CheckSemantics(context);

        if (!Left.IsNumeric(context) || !Right.IsNumeric(context))
            CompilingErrors.Add(new CompilingError(Operator.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"Deben ser n'umeros"));
    }

    public override string ToString() => $"({Left} ** {Right})";


}