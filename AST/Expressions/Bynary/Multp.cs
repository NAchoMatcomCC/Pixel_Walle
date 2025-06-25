public class MultiplyExpr : BinaryExpr {
    public MultiplyExpr(Expr left, Token op, Expr right, List<CompilingError> CompilingErrors) :  base(left, op, right, CompilingErrors) { }

    public override void CheckSemantics(SemanticContext context) {
        Left.CheckSemantics(context);
        Right.CheckSemantics(context);
        if (!Left.IsNumeric(context) || !Right.IsNumeric(context))
            CompilingErrors.Add(new CompilingError(Operator.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"Se deben multiplicar n'umeros"));
    }

    public override bool IsNumeric(SemanticContext context) => true;
    public override bool IsBoolean(SemanticContext context) => false;

    public override string ToString() => $"({Left} * {Right})";

    
}
