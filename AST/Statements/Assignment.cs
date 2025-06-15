public class AssignmentStmt : Stmt
    {
        public string VariableName { get; }
        public Expr Expression { get; }

        public AssignmentStmt(Token identifier, Expr expression) 
            : base(identifier)
        {
            VariableName = identifier.Lexeme;
            Expression = expression;
        }



    



        public override void CheckSemantics(SemanticContext context)
        {
            Expression.CheckSemantics(context);
    
            if (!IsValidVariableName(VariableName))
                throw new Exception($"Invalid variable name: {VariableName}");
    
            context.DefineVariable(VariableName, Expression.IsNumeric(context));
        }
    
        private bool IsValidVariableName(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return char.IsLetter(name[0]) || name[0] == '_'; // Puedes extender esto según las reglas del lenguaje
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return $"{VariableName} <- {Expression}";
        }
    }