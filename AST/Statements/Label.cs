public class Label : Stmt
{
    public Token Name { get; }
    public Label(Token name) => Name = name;
}