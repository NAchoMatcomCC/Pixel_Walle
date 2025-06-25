public class ColorCommand : Stmt
{
    public Expr ColorExpression { get; }

    private static readonly HashSet<string> AllowedColors = new()
    {
        "Red", "Blue", "Green", "Yellow", "Orange",
        "Purple", "Black", "White", "Transparent"
    };

    public ColorCommand(Token colorToken, Expr colorExpression, List<CompilingError> CompilingErrors) 
        : base(colorToken, CompilingErrors)
    {
        ColorExpression = colorExpression;
    }

    public override void CheckSemantics(SemanticContext context)
    {
        ColorExpression.CheckSemantics(context);
        
        // Check if it's a string literal
        if (ColorExpression is Literal literal && literal.Value is string colorValue)
        {
            if (!AllowedColors.Contains(colorValue))
                CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"Color no v'alido"));
        }
        // Else: runtime check will be needed
    }

    public override string ToString()
    {
        return $"Color({ColorExpression})";
    }

    public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
}