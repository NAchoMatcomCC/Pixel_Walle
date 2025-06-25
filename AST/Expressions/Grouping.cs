public class Grouping : Expr
{
    public Expr Expression { get; }

    public Grouping(Expr expr, Token startoken, List<CompilingError> CompilingErrors) : base(startoken, CompilingErrors)
    {
        Expression = expr;
    }

    public override void CheckSemantics(SemanticContext context)
    {
        Expression.CheckSemantics(context);
    }

    public override bool IsNumeric(SemanticContext context) => Expression.IsNumeric(context);
    public override bool IsBoolean(SemanticContext context) => Expression.IsBoolean(context);

    public override string ToString()
    {
        return $"({Expression})";
    }

    
}
