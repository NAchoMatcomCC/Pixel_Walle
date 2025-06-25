public class NotExpr : UnaryExpr
{
    public NotExpr(Expr operand, Token startoken, List<CompilingError> CompilingErrors) : base(operand, startoken, CompilingErrors) { }

    public override void CheckSemantics(SemanticContext context)
    {
        Operand.CheckSemantics(context);
        if (!Operand.IsBoolean(context))
            CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"'!' requiere una expresi'on booleana"));
    }

    public override bool IsNumeric(SemanticContext context) => false;
    public override bool IsBoolean(SemanticContext context) => true;
}
