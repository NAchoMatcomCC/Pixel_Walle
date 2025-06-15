public class FunctionCall : Expr
{
    public string FunctionName { get; }
    public List<Expr> Arguments { get; }

    public FunctionCall(string functionName, List<Expr> arguments, Token token) 
        : base(token)
    {
        FunctionName = functionName;
        Arguments = arguments;
    }

    public override void CheckSemantics(SemanticContext context)
    {
        foreach (var arg in Arguments)
        {
            arg.CheckSemantics(context);
        }
    }

    public override bool IsNumeric(SemanticContext context) => true;
    public override bool IsBoolean(SemanticContext context) => false;
}