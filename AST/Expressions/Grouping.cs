public class Grouping : Expr
{
    public Expr Expression { get; }
    public Grouping(Expr expression, Token token) : base(token) => Expression = expression;

     public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}