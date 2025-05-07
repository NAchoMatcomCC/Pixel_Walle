public class Binary : Expr
{
    public Expr Left { get; }
    public Token Operator { get; } // Token como "+", ">", etc.
    public Expr Right { get; }
    public Binary(Expr left, Token op, Expr right)
    {
        Left = left;
        Operator = op;
        Right = right;
    }
}