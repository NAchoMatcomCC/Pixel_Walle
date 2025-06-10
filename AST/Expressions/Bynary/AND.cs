public class OrExpr : BinaryExpr
{
    public OrExpr(Expr left, Token opToken, Expr right)
        : base(left, opToken, right) { }

    

    public override bool IsNumeric(SemanticContext context) => false;
    public override bool IsBoolean(SemanticContext context) => true;

    public override void CheckSemantics(SemanticContext context)
    {
        Left.CheckSemantics(context);
        Right.CheckSemantics(context);

        if (!Left.IsBoolean(context) || !Right.IsBoolean(context))
            throw new Exception("Logical OR operands must be boolean.");
    }
}