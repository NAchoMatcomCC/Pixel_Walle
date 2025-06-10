public class Var : Expr
{
    public string Name { get; }

    public Var(string name, Token startoken) : base(startoken)
    {
        Name = name;
    }

    public override void CheckSemantics(SemanticContext context)
    {
        if (!context.IsVariableDefined(Name))
            throw new Exception($"Variable '{Name}' is not defined.");
    }

    public override bool IsNumeric(SemanticContext context) => context.IsVariableNumeric(Name);
    public override bool IsBoolean(SemanticContext context) => !context.IsVariableNumeric(Name);

    

}
