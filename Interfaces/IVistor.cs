public interface IAstVisitor<T>
    {
        T Visit(Expr expr);
        T Visit(Stmt stmt);
        T Visit(BinaryExpr expr);
        T Visit(LiteralExpr expr);
        T Visit(FunctionCallExpr expr);
        T Visit(SpawnStmt stmt);
        T Visit(Color stmt);
        T Visit(SizeStmt stmt);
        T Visit(DrawLineStmt stmt);
        T Visit(DrawCircleStmt stmt);
        T Visit(DrawRectangleStmt stmt);
        T Visit(FillStmt stmt);
        T Visit(AssignmentStmt stmt);
        T Visit(Label stmt);
        T Visit(GoTo stmt);
    }

    // Visitor base con implementaciones vacías
    public abstract class AstBaseVisitor<T> : IAstVisitor<T>
    {
        public virtual T Visit(Expr expr) => expr.Accept(this);
        public virtual T Visit(Stmt stmt) => stmt.Accept(this);
        
        public virtual T Visit(BinaryExpr expr) => default;
        public virtual T Visit(LiteralExpr expr) => default;
       
        public virtual T Visit(FunctionCallExpr expr) => default;
        public virtual T Visit(SpawnStmt stmt) => default;
        public virtual T Visit(Color stmt) => default;
        public virtual T Visit(SizeStmt stmt) => default;
        public virtual T Visit(DrawLineStmt stmt) => default;
        public virtual T Visit(DrawCircleStmt stmt) => default;
        public virtual T Visit(DrawRectangleStmt stmt) => default;
        public virtual T Visit(FillStmt stmt) => default;
        public virtual T Visit(AssignmentStmt stmt) => default;
        public virtual T Visit(Label stmt) => default;
        public virtual T Visit(GoTo stmt) => default;
    }