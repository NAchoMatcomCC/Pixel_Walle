public class Variable : Expr
{
    public Token Name { get; } // Token del identificador (ej: "n")
    public Variable(Token name) => Name = name;
}
