using Segundo_Proyecto1._0;
using System.Drawing;

public class Interpreter : INodeVisitor
{
    private readonly CanvasData canvas;
    private int posX = 0;
    private int posY = 0;
    private string currentColor = "black";
    private int currentSize = 1;
    private readonly Dictionary<string, int> variables = new();

    public Interpreter(CanvasData canvas)
    {
        this.canvas = canvas;
    }

    private static readonly Dictionary<string, Color> ColorMap = new()
    {
        ["Red"] = Color.Red,
        ["Blue"] = Color.Blue,
        ["Green"] = Color.Green,
        ["Yellow"] = Color.Yellow,
        ["Orange"] = Color.Orange,
        ["Purple"] = Color.Purple,
        ["Black"] = Color.Black,
        ["White"] = Color.White,
        ["Transparent"] = Color.Transparent
    };

    public void Visit(SpawnStmt node)
    {
        posX = Evaluate(node.X);
        posY = Evaluate(node.Y);

        canvas.WallE_X = posX;
        canvas.WallE_Y = posY;
    }

    public void Visit(ColorCommand node)
    {
        // Evaluar la expresión de color
        object colorValue = node.ColorExpression switch
        {
            Literal lit => lit.Value,
            Var v when variables.ContainsKey(v.Name) => variables[v.Name],
            _ => ""
        };
        
        string colorName = colorValue.ToString();
        
        if (!ColorMap.TryGetValue(colorName, out Color color))
        {
            throw new Exception($"Color desconocido: {colorName}");
        }
        
        currentColor = colorName;
    }

    public void Visit(SizeStmt node)
    {
        int size = Evaluate(node.SizeValue);
    
        // Asegurar que el tamaño sea positivo e impar
        currentSize = Math.Max(1, Math.Abs(size));
        if (currentSize % 2 == 0)
        {
            currentSize--; // Hacer impar
        }
    }

    private void DrawWithBrush(int x, int y, Color color)
    {
        int halfSize = currentSize ;
        
    
        for (int dx = -halfSize; dx < halfSize; dx++)
        {
            for (int dy = -halfSize; dy < halfSize; dy++)
            {
                int targetX = x + dx;
                int targetY = y + dy;
            
                if (targetX >= 0 && targetX < canvas.Size && 
                    targetY >= 0 && targetY < canvas.Size)
                    {
                        canvas.SetPixel(targetX, targetY, color);
                    }
            }
        }
        
        
        
    }

    public void Visit(DrawLineStmt node)
    {
        int dx = Evaluate(node.DirX);
        int dy = Evaluate(node.DirY);
        int dist = Evaluate(node.Distance);
        Color color = GetCurrentColor();
    
        // Validar dirección
        if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
        {
            throw new Exception($"Dirección inválida: ({dx}, {dy}). " +
                               "Los valores deben ser -1, 0 o 1");
        }
    
        for (int i = 0; i < dist; i++)
        {
            int currentX = posX + dx * i;
            int currentY = posY + dy * i;
            

            if(currentSize==1) canvas.SetPixel(currentX, currentY, color);
            else DrawWithBrush(currentX, currentY, color);
           
        }

         posX += dx * dist;
        posY += dy * dist;
    
        // Dibujar la posición final
        //DrawWithBrush(posX, posY, color);

        canvas.WallE_X = posX;
        canvas.WallE_Y = posY;
    }

    public void Visit(DrawCircleStmt node)
    {
        int centerDx = Evaluate(node.DirX);
        int centerDy = Evaluate(node.DirY);
        int radius = Evaluate(node.Radius);
        int centerX = posX + centerDx;
        int centerY = posY + centerDy;
        Color color = GetCurrentColor();
    
        // Algoritmo para dibujar círculo (Bresenham)
        int x = 0;
        int y = radius;
        int d = 3 - 2 * radius;
    
        while (y >= x)
        {
            DrawWithBrush(centerX + x, centerY + y, color);
            DrawWithBrush(centerX + x, centerY - y, color);
            DrawWithBrush(centerX - x, centerY + y, color);
            DrawWithBrush(centerX - x, centerY - y, color);
            DrawWithBrush(centerX + y, centerY + x, color);
            DrawWithBrush(centerX + y, centerY - x, color);
            DrawWithBrush(centerX - y, centerY + x, color);
            DrawWithBrush(centerX - y, centerY - x, color);
        
            x++;
            if (d > 0)
            {
                y--;
                d = d + 4 * (x - y) + 10;
            }
            else
            {
                d = d + 4 * x + 6;
            }
        }
    
        // Actualizar posición al centro del círculo
        posX = centerX;
        posY = centerY;

        canvas.WallE_X = posX;
        canvas.WallE_Y = posY;
    }

    public void Visit(DrawRectangleStmt node)
    {
        int dx = Evaluate(node.DirX);
        int dy = Evaluate(node.DirY);
        int distance = Evaluate(node.Distance);
        int width = Evaluate(node.Width);
        int height = Evaluate(node.Height);
        
        // Calcular posición del centro
        int centerX = posX + dx * distance;
        int centerY = posY + dy * distance;
    
        // Calcular esquinas
        int startX = centerX - width / 2;
        int startY = centerY - height / 2;
        int endX = startX + width;
        int endY = startY + height;
    
        Color color = GetCurrentColor();
    
        // Dibujar rectángulo
        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                if (x >= 0 && x < canvas.Size && y >= 0 && y < canvas.Size)
                {
                    DrawWithBrush(x, y, color);
                }
            }
        }
        
        // Actualizar posición al centro del rectángulo
        posX = centerX;
        posY = centerY;

        canvas.WallE_X = posX;
        canvas.WallE_Y = posY;
    }

    private Color GetCurrentColor()
    {
        if (currentColor == "Transparent") return Color.Transparent;
        return ColorMap[currentColor];
    }

    

    private void FloodFill(int x, int y, Color target, Color replacement)
    {
        if (target == replacement) return;
        if (canvas.GetPixel(x, y) != target) return;

        if (x < 0 || x >= canvas.Size || y < 0 || y >= canvas.Size)
        {
            throw new Exception($"Pixel coordinates ({x}, {y}) are out of canvas bounds.");
        }

        Queue<(int x, int y)> queue = new();
        queue.Enqueue((x, y));

        while (queue.Count > 0)
        {
            var (cx, cy) = queue.Dequeue();
            if (cx < 0 || cx >= canvas.Size || cy < 0 || cy >= canvas.Size) continue;
            if (canvas.GetPixel(cx, cy) != target) continue;

            canvas.SetPixel(cx, cy, replacement);

            queue.Enqueue((cx + 1, cy));
            queue.Enqueue((cx - 1, cy));
            queue.Enqueue((cx, cy + 1));
            queue.Enqueue((cx, cy - 1));
        }
    }

    public void Visit(FillStmt node)
    {
        Color target = canvas.GetPixel(posX, posY);
        Color replacement = GetCurrentColor();

        FloodFill(posX, posY, target, replacement);
    }

    public void Visit(AssignmentStmt node)
    {
        int value = Evaluate(node.Expression);
        variables[node.VariableName] = value;
    }

    public void Visit(Label node)
    {
        // No hace nada en tiempo de ejecución
    }

    public void Visit(GoTo node)
    {
        // Control de flujo lo maneja ProgramRunner
    }

    



    public int Evaluate(Expr expr)
    {
        return expr switch
        {
            Literal lit => Convert.ToInt32(lit.Value),
            Var v => variables.ContainsKey(v.Name) ? variables[v.Name] : 0,
            AddExpr add => Evaluate(add.Left) + Evaluate(add.Right),
            SubtractExpr sub => Evaluate(sub.Left) - Evaluate(sub.Right),
            MultiplyExpr mul => Evaluate(mul.Left) * Evaluate(mul.Right),
            DivideExpr div => Evaluate(div.Left) / Evaluate(div.Right),
            ModuloExpr mod => Evaluate(mod.Left) % Evaluate(mod.Right),
            PowerExpr pow => (int)Math.Pow(Evaluate(pow.Left), Evaluate(pow.Right)),
            
            // Comparaciones
            EqualExpr eq => Evaluate(eq.Left) == Evaluate(eq.Right) ? 1 : 0,
            NotEqualExpr ne => Evaluate(ne.Left) != Evaluate(ne.Right) ? 1 : 0,
            GreaterExpr gt => Evaluate(gt.Left) > Evaluate(gt.Right) ? 1 : 0,
            LessExpr lt => Evaluate(lt.Left) < Evaluate(lt.Right) ? 1 : 0,
            GreaterEqualExpr ge => Evaluate(ge.Left) >= Evaluate(ge.Right) ? 1 : 0,
            LessEqualExpr le => Evaluate(le.Left) <= Evaluate(le.Right) ? 1 : 0,
            
            // Lógicos
            AndExpr and => (Evaluate(and.Left) != 0 && Evaluate(and.Right) != 0) ? 1 : 0,
            OrExpr or => (Evaluate(or.Left) != 0 || Evaluate(or.Right) != 0) ? 1 : 0,
            NotExpr not => Evaluate(not.Operand) == 0 ? 1 : 0,
            
        // Unarios
            NegateExpr neg => -Evaluate(neg.Operand),
            
            // Agrupación
            Grouping group => Evaluate(group.Expression),
            
            // Llamadas a función
            FunctionCall call => EvaluateFunction(call),
            
            _ => throw new Exception($"Expresión no soportada: {expr.GetType().Name}")
        };
    }

    private int EvaluateFunction(FunctionCall call)
    {
        switch (call.FunctionName)
        {
            case "GetActualX":
                return posX;
                
            case "GetActualY":
                return posY;
                
            case "GetCanvasSize":
                return canvas.Size;
                
            // ... otras funciones ...
            
            default:
                throw new Exception($"Función desconocida: {call.FunctionName}");
        }
    }
}
