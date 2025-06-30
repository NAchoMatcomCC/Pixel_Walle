public class GoTo : Stmt
{
    public string LabelName { get; }
    public Expr Condition { get; }

    public GoTo(string labelName, Expr condition, Token starToken, List<CompilingError> CompilingErrors) : base(starToken, CompilingErrors)
    {
        LabelName = labelName;
        Condition = condition;
    }

    public override void CheckSemantics(SemanticContext context)
    {
        Condition.CheckSemantics(context);

        if (!Condition.IsBoolean(context))
            CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"La condici'on debe ser booleana"));

        /*if (!context.IsLabelDefined(LabelName))
            CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"La estiqueta {LabelName} no est'a definida"));*/
    }

    public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

    public override string ToString()
    {
        return $"Goto[{LabelName}]({Condition})";
    }
}