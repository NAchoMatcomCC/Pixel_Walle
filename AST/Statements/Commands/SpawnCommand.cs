public class SpawnStmt : Stmt
    {
        public Expr X { get; }
        public Expr Y { get; }

        public SpawnStmt(Token spawnToken, Expr x, Expr y, List<CompilingError> CompilingErrors) 
            : base(spawnToken, CompilingErrors)
        {
            X = x;
            Y = y;
        }

        

        public override void CheckSemantics(SemanticContext context)
        {
            X.CheckSemantics(context);
            Y.CheckSemantics(context);

            if (context.SpawnCalled)
                CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"Spawn solo puede ser utilizado una vez"));
    
            if (!X.IsNumeric(context) || !Y.IsNumeric(context))
                CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"Spawn espera argumentos num'ericos"));
    
            context.SpawnCalled = true;
    
    
        }


        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return $"Spawn({X}, {Y})";
        }

    }