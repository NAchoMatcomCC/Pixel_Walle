public class EqualExpr : BinaryExpr
{
    public EqualExpr(Expr left, Token opToken, Expr right)
        : base(left, opToken, right) { }

    

    public override bool IsNumeric(SemanticContext context) => false;
    public override bool IsBoolean(SemanticContext context) => true;

    public override void CheckSemantics(SemanticContext context)
    {
        Left.CheckSemantics(context);
        Right.CheckSemantics(context);

        // Permitir comparación entre tipos compatibles
        if (Left.IsNumeric(context) != Right.IsNumeric(context) && 
            Left.IsBoolean(context) != Right.IsBoolean(context))
        {
            throw new Exception("Cannot compare different types.");
        }
    }

    public override string ToString() => $"({Left} == {Right})";
}