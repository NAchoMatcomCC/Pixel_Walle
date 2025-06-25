public class DrawLineStmt : Stmt
    {
        public Expr DirX { get; }
        public Expr DirY { get; }
        public Expr Distance { get; }

        public DrawLineStmt(Token drawToken, Expr dirX, Expr dirY, Expr distance, List<CompilingError> CompilingErrors) 
            : base(drawToken, CompilingErrors)
        {
            DirX = dirX;
            DirY = dirY;
            Distance = distance;
        }


        


        public override void CheckSemantics(SemanticContext context)
    {
        DirX.CheckSemantics(context);
        DirY.CheckSemantics(context);
        Distance.CheckSemantics(context);

        if (!DirX.IsNumeric(context) || !DirY.IsNumeric(context) || !Distance.IsNumeric(context))
            CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"DrawLine espera argumentos num'ericos"));
    }

    public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

    public override string ToString()
    {
        return $"DrawLine({DirX}, {DirY}, {Distance})";
    }

    
    }