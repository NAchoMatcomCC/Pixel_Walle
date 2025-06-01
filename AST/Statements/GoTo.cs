public class GoTo : Stmt
    {
        public string Label { get; }
        public Expr Condition { get; }

        public GoTo(Token gotoToken, Token labelToken, Expr condition) 
            : base(gotoToken)
        {
            Label = labelToken.Lexeme;
            Condition = condition;
        }

         public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
    }