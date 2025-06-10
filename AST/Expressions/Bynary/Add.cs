public class AddExpr : BinaryExpr
{
    public AddExpr(Expr left, Token opToken, Expr right)
        : base(left, opToken, right) { }

    

    public override bool IsNumeric(SemanticContext context) => true;
    public override bool IsBoolean(SemanticContext context) => false;


    public override void CheckSemantics(SemanticContext context) {
        Left.CheckSemantics(context);
        Right.CheckSemantics(context);

        if (!Left.IsNumeric(context) || !Right.IsNumeric(context))
            throw new Exception("Addition operands must be numeric.");
    }
}