public class LessEqualExpr : BinaryExpr
{
    public LessEqualExpr(Expr left, Token opToken, Expr right)
        : base(left, opToken, right) { }

    

    public override bool IsNumeric(SemanticContext context) => false;
    public override bool IsBoolean(SemanticContext context) => true;

    public override void CheckSemantics(SemanticContext context)
    {
        Left.CheckSemantics(context);
        Right.CheckSemantics(context);

        if (!Left.IsNumeric(context) || !Right.IsNumeric(context))
            throw new Exception("Comparison operands must be numeric.");
    }

    public override string ToString() => $"({Left} <= {Right})";
}
