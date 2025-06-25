public class Var : Expr
{
    public string Name { get; }

    public Var(string name, Token startoken, List<CompilingError> CompilingErrors) : base(startoken, CompilingErrors)
    {
        Name = name;
    }

    public override void CheckSemantics(SemanticContext context)
    {
        if (!context.IsVariableDefined(Name))
            CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"Variable no definida"));;
    }

    public override bool IsNumeric(SemanticContext context) => context.IsVariableNumeric(Name);
    public override bool IsBoolean(SemanticContext context) => !context.IsVariableNumeric(Name);

    

}
