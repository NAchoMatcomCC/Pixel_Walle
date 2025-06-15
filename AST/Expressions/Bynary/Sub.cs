public class SubtractExpr : BinaryExpr {
    public SubtractExpr(Expr left, Token op, Expr right) :  base(left, op, right) { }

    public override void CheckSemantics(SemanticContext context) {
        Left.CheckSemantics(context);
        Right.CheckSemantics(context);
        if (!Left.IsNumeric(context) || !Right.IsNumeric(context))
            throw new Exception("Subtraction operands must be numeric.");
    }

    public override bool IsNumeric(SemanticContext context) => true;
    public override bool IsBoolean(SemanticContext context) => false;

    public override string ToString() => $"({Left} - {Right})";

    
}

