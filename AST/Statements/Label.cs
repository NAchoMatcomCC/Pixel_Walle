public class Label : Stmt
    {
        public string Name { get; }

        public Label(Token labelToken) 
            : base(labelToken)
        {
            Name = labelToken.Lexeme;
        }

        public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
    }
