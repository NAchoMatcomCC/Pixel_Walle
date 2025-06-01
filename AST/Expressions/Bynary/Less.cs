public class LessExpr : BinaryExpr
{
    public LessExpr(Expr left, Token opToken, Expr right)
        : base(left, opToken, right) { }

    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);

    public override bool IsNumeric(SemanticContext context) => false;
    public override bool IsBoolean(SemanticContext context) => true;

    public override void CheckSemantics(SemanticContext context)
    {
        Left.CheckSemantics(context);
        Right.CheckSemantics(context);

        if (!Left.IsNumeric(context) || !Right.IsNumeric(context))
            throw new Exception("Comparison operands must be numeric.");
    }
}