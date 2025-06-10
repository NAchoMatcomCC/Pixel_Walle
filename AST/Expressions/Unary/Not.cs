public class NotExpr : UnaryExpr
{
    public NotExpr(Expr operand, Token startoken) : base(operand, startoken) { }

    public override void CheckSemantics(SemanticContext context)
    {
        Operand.CheckSemantics(context);
        if (!Operand.IsBoolean(context))
            throw new Exception("Unary '!' requires a boolean operand.");
    }

    public override bool IsNumeric(SemanticContext context) => false;
    public override bool IsBoolean(SemanticContext context) => true;
}
