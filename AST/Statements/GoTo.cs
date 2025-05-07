public class GoTo : Stmt
{
    public Token Label { get; } // Etiqueta destino (ej: "loop1")
    public Expr Condition { get; } // Condición booleana (ej: n > 0)
    public GoTo(Token label, Expr condition)
    {
        Label = label;
        Condition = condition;
    }
}