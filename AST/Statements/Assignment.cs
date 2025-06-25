public class AssignmentStmt : Stmt
    {
        public string VariableName { get; }
        public Expr Expression { get; }

        public AssignmentStmt(Token identifier, Expr expression, List<CompilingError> CompilingErrors) 
            : base(identifier, CompilingErrors)
        {
            VariableName = identifier.Lexeme;
            Expression = expression;
        }



    



        public override void CheckSemantics(SemanticContext context)
        {
            Expression.CheckSemantics(context);
    
            if (!IsValidVariableName(VariableName))
                CompilingErrors.Add(new CompilingError(StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, 
        $"Nombre de variable {VariableName} inv'alido"));
    
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