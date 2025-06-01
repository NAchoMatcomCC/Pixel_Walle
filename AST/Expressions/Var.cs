public class Var : Expr
    {
        public string Name { get; }

        public Var(Token token) 
            : base(token)
        {
            Name = token.Lexeme;
        }

         public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
    }

    
