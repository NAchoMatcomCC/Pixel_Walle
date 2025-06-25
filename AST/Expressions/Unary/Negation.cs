public class NegateExpr : UnaryExpr
{
    public NegateExpr(Expr operand, Token startoken, List<CompilingError> CompilingErrors) : base(operand, startoken, CompilingErrors) { }

    public override void CheckSemantics(SemanticContext context)
    {
        Operand.CheckSemantics(context);
        if (!Operand.IsNumeric(context))
            CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"'-' requiere un n'umero"));
    }

    public override bool IsNumeric(SemanticContext context) => true;
    public override bool IsBoolean(SemanticContext context) => false;

    
}