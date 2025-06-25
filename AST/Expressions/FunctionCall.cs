public class FunctionCall : Expr
{
    public string FunctionName { get; }
    public List<Expr> Arguments { get; }

    public FunctionCall(string functionName, List<Expr> arguments, Token token, List<CompilingError> CompilingErrors) 
        : base(token, CompilingErrors)
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

    /*switch (FunctionName)
    {
        case "GetColorCount":
            if (Arguments.Count != 5)
               
            break;
            
        case "IsCanvasColor":
            if (Arguments.Count != 3)
                throw new CompilingError(Token.Line, ErrorCode.Invalid, "IsCanvasColor requiere 3 argumentos");
            break;
            
        // Validaciones similares para otras funciones...
    }*/
}

    public override bool IsNumeric(SemanticContext context) => true;
    public override bool IsBoolean(SemanticContext context) => false;
}