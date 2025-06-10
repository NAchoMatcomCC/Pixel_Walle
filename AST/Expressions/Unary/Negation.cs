public class NegateExpr : UnaryExpr
{
    public NegateExpr(Expr operand, Token startoken) : base(operand, startoken) { }

    public override void CheckSemantics(SemanticContext context)
    {
        Operand.CheckSemantics(context);
        if (!Operand.IsNumeric(context))
            throw new Exception("Unary '-' requires a numeric operand.");
    }

    public override bool IsNumeric(SemanticContext context) => true;
    public override bool IsBoolean(SemanticContext context) => false;

    
}