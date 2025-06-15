public class DrawCircleStmt : Stmt
    {
        public Expr DirX { get; }
        public Expr DirY { get; }
        public Expr Radius { get; }

        public DrawCircleStmt(Token drawToken, Expr dirX, Expr dirY, Expr radius) 
            : base(drawToken)
        {
            DirX = dirX;
            DirY = dirY;
            Radius = radius;
        }

        

        public override void CheckSemantics(SemanticContext context)
    {
        DirX.CheckSemantics(context);
        DirY.CheckSemantics(context);
        Radius.CheckSemantics(context);

        if (!DirX.IsNumeric(context) || !DirY.IsNumeric(context) || !Radius.IsNumeric(context))
            throw new Exception("DrawLine expects numeric arguments.");
    }

    public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

    public override string ToString()
    {
        return $"DrawCircle({DirX}, {DirY}, {Radius})";
    }


    }