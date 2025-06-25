public class ModuloExpr : BinaryExpr
{
    public ModuloExpr(Expr left, Token opToken, Expr right, List<CompilingError> CompilingErrors)
        : base(left, opToken, right, CompilingErrors) { }

    
    public override bool IsNumeric(SemanticContext context) => true;
    public override bool IsBoolean(SemanticContext context) => false;

    public override void CheckSemantics(SemanticContext context)
    {
        Left.CheckSemantics(context);
        Right.CheckSemantics(context);

        if (!Left.IsNumeric(context) || !Right.IsNumeric(context))
            CompilingErrors.Add(new CompilingError(Operator.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"Debe ser un n'umero"));
        
        // Verificar módulo por cero en tiempo de compilación si es posible
        if (Right is Literal lit && Convert.ToInt32(lit.Value) == 0)
            CompilingErrors.Add(new CompilingError(Operator.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"M'odulo de cero no est'a permitido"));
    }

    public override string ToString() => $"({Left} % {Right})";
}