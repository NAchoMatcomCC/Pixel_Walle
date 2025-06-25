public class DrawRectangleStmt : Stmt
    {
        public Expr DirX { get; }
        public Expr DirY { get; }
        public Expr Distance { get; }
        public Expr Width { get; }
        public Expr Height { get; }

        public DrawRectangleStmt(Token drawToken, Expr dirX, Expr dirY, Expr distance, Expr width, Expr height, List<CompilingError> CompilingErrors) 
            : base(drawToken, CompilingErrors)
        {
            DirX = dirX;
            DirY = dirY;
            Distance = distance;
            Width = width;
            Height = height;
        }

       


        public override void CheckSemantics(SemanticContext context)
    {
        DirX.CheckSemantics(context);
        DirY.CheckSemantics(context);
        Distance.CheckSemantics(context);
        Width.CheckSemantics(context);
        Height.CheckSemantics(context);

        if (!DirX.IsNumeric(context) || !DirY.IsNumeric(context) ||
            !Distance.IsNumeric(context) || !Width.IsNumeric(context) || !Height.IsNumeric(context))
           CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"DrawRectangle espera argumentos num'ericos"));
    }


    public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

    public override string ToString()
    {
        return $"DrawRectangle({DirX}, {DirY}, {Distance}, {Width}, {Height})";
    }



    }