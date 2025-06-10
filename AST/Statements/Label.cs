public class Label : ASTNode
{
    public string Name { get; }

    public Label(string name, Token starToken) : base(starToken)
    {
        Name = name;
    }

    public override void CheckSemantics(SemanticContext context)
    {
        if (!IsValidLabelName(Name))
            throw new Exception($"Invalid label name: '{Name}'");

        if (context.IsLabelDefined(Name))
            throw new Exception($"Label '{Name}' is already defined.");

        context.DefineLabel(Name);
    }

    private bool IsValidLabelName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        char first = name[0];
        return char.IsLetter(first) || first == '_';
    }

    public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

}