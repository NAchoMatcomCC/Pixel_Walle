using Segundo_Proyecto1._0;

public class ProgramRunner
{
    private readonly List<ASTNode> program;
    private readonly Dictionary<string, int> labelMap = new();
    private readonly Interpreter interpreter;
    private readonly CanvasData canvas;

    public ProgramRunner(List<ASTNode> program, CanvasData canvas)
    {
        this.canvas=canvas;
        this.program = program;
        this.interpreter = new Interpreter(canvas);
        IndexLabels();
    }

    private void IndexLabels()
    {
        for (int i = 0; i < program.Count; i++)
        {
            if (program[i] is Label label)
            {
                if (labelMap.ContainsKey(label.Name))
                    throw new Exception($"Label '{label.Name}' already defined.");

                labelMap[label.Name] = i;
            }
        }
    }

    public void Run()
    {
        try
        {
            for (int pc = 0; pc < program.Count; pc++)
        {
            var node = program[pc];

            if (node is GoTo jump)
            {
                int result = interpreter.Evaluate(jump.Condition);
                if (result != 0)
                {
                    if (!labelMap.TryGetValue(jump.LabelName, out int targetIndex))
                        throw new Exception($"Label '{jump.LabelName}' not found.");

                    pc = targetIndex - 1; // porque el for hace pc++
                }
                continue;
            }

            node.Accept(interpreter);
        }
        }
        catch (System.Exception ex)
        {
            int line = 0;
            if (program.Count > 0 && program[0] is ASTNode firstNode)
            {
                line = firstNode.StartToken.Line;
            }
        
            throw new Exception($"[Línea {line}] Error en ejecución: {ex.Message}");
        }
        
    }
}