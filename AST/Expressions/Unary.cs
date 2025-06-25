public abstract class UnaryExpr : Expr
{
    public Expr Operand { get; }

    protected UnaryExpr(Expr operand, Token startoken, List<CompilingError> CompilingErrors) : base(startoken, CompilingErrors)
    {
        Operand = operand;
    }
}
