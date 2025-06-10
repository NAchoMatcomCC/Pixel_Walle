public class SpawnStmt : Stmt
    {
        public Expr X { get; }
        public Expr Y { get; }

        public SpawnStmt(Token spawnToken, Expr x, Expr y) 
            : base(spawnToken)
        {
            X = x;
            Y = y;
        }

        

        public override void CheckSemantics(SemanticContext context)
        {
            if (context.SpawnCalled)
                throw new Exception("Spawn can only be called once.");
    
            if (!X.IsNumeric(context) || !Y.IsNumeric(context))
                throw new Exception("Spawn arguments must be numeric.");
    
            context.SpawnCalled = true;
    
            X.CheckSemantics(context);
            Y.CheckSemantics(context);
        }


        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

    }