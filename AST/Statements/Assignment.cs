public class Assignment : Stmt
{
    public Token VariableName { get; } // Nombre de la variable (ej: "n")
    public Expr Value { get; } // Valor asignado (ej: 5 + 3)
    public Assignment(Token name, Expr value)
    {
        VariableName = name;
        Value = value;
    }
}