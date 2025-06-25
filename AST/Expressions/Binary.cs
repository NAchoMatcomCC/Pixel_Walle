 public abstract class BinaryExpr : Expr
    {
        public Expr Left { get; }
        public Token Operator { get; }
        public Expr Right { get; }

        public BinaryExpr(Expr left, Token op, Expr right, List<CompilingError> CompilingErrors) 
            : base(left.StartToken, CompilingErrors)
        {
            Left = left;
            Operator = op;
            Right = right;
        }

        //public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
    }