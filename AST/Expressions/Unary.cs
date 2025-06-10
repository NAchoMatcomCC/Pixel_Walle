public abstract class UnaryExpr : Expr
{
    public Expr Operand { get; }

    protected UnaryExpr(Expr operand, Token startoken) : base(startoken)
    {
        Operand = operand;
    }
}
