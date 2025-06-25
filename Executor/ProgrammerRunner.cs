using Segundo_Proyecto1._0;

public class ProgramRunner
{
    private readonly List<ASTNode> program;
    private readonly Dictionary<string, int> labelMap = new();
    private readonly Interpreter interpreter;
    private readonly CanvasData canvas;
    private List<CompilingError> CompilingErrors;

    public ProgramRunner(List<ASTNode> program, CanvasData canvas, List<CompilingError> compilingErrors)
    {
        this.canvas=canvas;
        this.program = program;
        CompilingErrors=compilingErrors;
        this.interpreter = new Interpreter(canvas, compilingErrors);
        IndexLabels();
    }

    private void IndexLabels()
    {
        for (int i = 0; i < program.Count; i++)
        {
            if (program[i] is Label label)
            {
                if (labelMap.ContainsKey(label.Name))
                    {CompilingErrors.Add(new CompilingError(label.StartToken.Line, ErrorCode.Invalid, ErrorStage.Semantic, $"Label '{label.Name}' ya fue definida"));
                    return;}

                labelMap[label.Name] = i;
            }
        }
    }

    public void Run()
    {
        
        for (int pc = 0; pc < program.Count; pc++)
        {
            var node = program[pc];

            if (node is GoTo jump)
            {
                object result = interpreter.Evaluate(jump.Condition);

                if (result is int n && n != 0)
                {
                    if (!labelMap.TryGetValue(jump.LabelName, out int targetIndex))
                    {CompilingErrors.Add(new CompilingError(jump.StartToken.Line, ErrorCode.Invalid, ErrorStage.Runtime, $"Label '{jump.LabelName}' no encontrada"));
                        break;}

                    pc = targetIndex - 1;
                }
            }
            try
            {
                node.Accept(interpreter);
            }
            catch (System.Exception ex)
            {
                CompilingErrors.Add(new CompilingError(node.StartToken.Line, ErrorCode.Invalid, ErrorStage.Runtime, $"Error en ejecución: {ex.Message}"));
                break;
            }

            
        }
        
        
        
    }
}