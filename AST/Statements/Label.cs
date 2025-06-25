public class Label : Stmt
{
    public string Name { get; }

    public Label(string name, Token starToken, List<CompilingError> CompilingErrors) : base(starToken, CompilingErrors)
    {
        Name = name;
    }

    public override void CheckSemantics(SemanticContext context)
    {
        if (!IsValidLabelName(Name))
            CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"{Name} nombre inv'alido para etiqueta"));
        if (context.IsLabelDefined(Name))
            CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"Nombre de etiqueta ya defnido"));

        context.DefineLabel(Name);
    }

    private bool IsValidLabelName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        char first = name[0];
        return char.IsLetter(first) || first == '_';
    }

    public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

    public override string ToString()
    {
        return $"Label {Name}";
    }

}