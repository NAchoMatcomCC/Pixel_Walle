using System.CodeDom;

public class Literal : Expr
{
    public object Value { get; }

    public Literal(object value, Token startoken, List<CompilingError> CompilingErrors) : base(startoken, CompilingErrors)
    {
        Value = value;
    }

    public override void CheckSemantics(SemanticContext context)
    {
        // Nada que verificar en literales
    }

    public override bool IsNumeric(SemanticContext context) => Value is int;
    public override bool IsBoolean(SemanticContext context)
        => Value is bool || (Value is int i && (i == 0 || i == 1));

    public override string ToString() => Value.ToString();

    

}
