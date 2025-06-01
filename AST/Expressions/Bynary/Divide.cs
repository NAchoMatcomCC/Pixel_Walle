public class DivideExpr : BinaryExpr
{
    public DivideExpr(Expr left, Token opToken, Expr right)
        : base(left, opToken, right) { }

    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);

    public override bool IsNumeric(SemanticContext context) => true;
    public override bool IsBoolean(SemanticContext context) => false;

    public override void CheckSemantics(SemanticContext context)
    {
        Left.CheckSemantics(context);
        Right.CheckSemantics(context);

        if (!Left.IsNumeric(context) || !Right.IsNumeric(context))
            throw new Exception("Division operands must be numeric.");
        
        // Verificar división por cero en tiempo de compilación si es posible
        if (Right is LiteralExpr lit && Convert.ToDouble(lit.Value) == 0)
            throw new Exception("Division by zero is not allowed.");
    }
}
