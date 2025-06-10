public class GoTo : ASTNode
{
    public string LabelName { get; }
    public Expr Condition { get; }

    public GoTo(string labelName, Expr condition, Token starToken) : base(starToken)
    {
        LabelName = labelName;
        Condition = condition;
    }

    public override void CheckSemantics(SemanticContext context)
    {
        Condition.CheckSemantics(context);

        if (!Condition.IsBoolean(context))
            throw new Exception("The condition in GoTo must be a boolean expression.");

        if (!context.IsLabelDefined(LabelName))
            throw new Exception($"Label '{LabelName}' used in GoTo is not defined.");
    }

    public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
}