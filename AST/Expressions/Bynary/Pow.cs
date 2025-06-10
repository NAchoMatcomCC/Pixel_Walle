public class PowerExpr : BinaryExpr
{
    public PowerExpr(Expr left, Token opToken, Expr right)
        : base(left, opToken, right) { }

    

    public override bool IsNumeric(SemanticContext context) => true;
    public override bool IsBoolean(SemanticContext context) => false;

    public override void CheckSemantics(SemanticContext context)
    {
        Left.CheckSemantics(context);
        Right.CheckSemantics(context);

        if (!Left.IsNumeric(context) || !Right.IsNumeric(context))
            throw new Exception("Exponentiation operands must be numeric.");
    }
}