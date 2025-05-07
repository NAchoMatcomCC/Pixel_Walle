public class Unary : Expr
{
    public Token Operator { get; }
    public Expr Right { get; }
    public Unary(Token op, Expr right) => (Operator, Right) = (op, right);
}