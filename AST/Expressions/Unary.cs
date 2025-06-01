public class Unary : Expr
{
    public Token Operator { get; }
    public Expr Right { get; }
    public Unary(Token op, Expr right) : base(op) => (Operator, Right) = (op, right);

     public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}