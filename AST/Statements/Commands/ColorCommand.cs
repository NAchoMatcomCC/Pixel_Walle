public class ColorCommand : Stmt
    {
        public string ColorValue { get; }

        private static readonly HashSet<string> AllowedColors = new()
        {
        "Red", "Blue", "Green", "Yellow", "Orange",
        "Purple", "Black", "White", "Transparent"
        };

        public ColorCommand(Token colorToken, string colorValue) 
            : base(colorToken)
        {
            ColorValue = colorValue;
        }

        


        public override void CheckSemantics(SemanticContext context)
        {
            if (!AllowedColors.Contains(ColorValue))
                throw new Exception($"Color '{ColorValue}' is not valid.");
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

    }