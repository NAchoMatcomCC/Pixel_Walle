public class OrExpr : BinaryExpr
{
    public OrExpr(Expr left, Token opToken, Expr right, List<CompilingError> CompilingErrors)
        : base(left, opToken, right, CompilingErrors) { }

    

    public override bool IsNumeric(SemanticContext context) => false;
    public override bool IsBoolean(SemanticContext context) => true;

    public override void CheckSemantics(SemanticContext context) {
        Left.CheckSemantics(context);
        Right.CheckSemantics(context);

            if (!Left.IsBoolean(context) || !Right.IsBoolean(context))
            CompilingErrors.Add(new CompilingError(Operator.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"AND recibe expresiones booleanas (1 y 0)"));
    }

    public override string ToString() => $"({Left} && {Right})";

    
}