public class ModuloExpr : BinaryExpr
{
    public ModuloExpr(Expr left, Token opToken, Expr right)
        : base(left, opToken, right) { }

    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);

    public override bool IsNumeric(SemanticContext context) => true;
    public override bool IsBoolean(SemanticContext context) => false;

    public override void CheckSemantics(SemanticContext context)
    {
        Left.CheckSemantics(context);
        Right.CheckSemantics(context);

        if (!Left.IsNumeric(context) || !Right.IsNumeric(context))
            throw new Exception("Modulo operands must be numeric.");
        
        // Verificar módulo por cero en tiempo de compilación si es posible
        if (Right is LiteralExpr lit && Convert.ToInt32(lit.Value) == 0)
            throw new Exception("Modulo by zero is not allowed.");
    }
}