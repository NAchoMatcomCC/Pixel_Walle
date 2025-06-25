using Segundo_Proyecto1._0;
using System.CodeDom;
using System.Drawing;

public class Interpreter : INodeVisitor
{
    private readonly CanvasData canvas;
    private int posX = 0;
    private int posY = 0;
    private string currentColor = "black";
    private int currentSize = 1;
    private readonly Dictionary<string, int> variables = new();
    private List<CompilingError> CompilingErrors;

    public Interpreter(CanvasData canvas, List<CompilingError> compilingErrors)
    {
        this.canvas = canvas;
        CompilingErrors=compilingErrors;
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
        posX = (int)Evaluate(node.X);
        posY = (int)Evaluate(node.Y);

        canvas.WallE_X = posX;
        canvas.WallE_Y = posY;
    }

    public void Visit(ColorCommand node)
{
    object colorValue = Evaluate(node.ColorExpression);
    string colorName = colorValue.ToString();
    
    if (!ColorMap.TryGetValue(colorName, out Color color))
    {
        CompilingErrors.Add(new CompilingError(node.StartToken.Line, ErrorCode.Invalid, ErrorStage.Runtime, $"Color {colorName} inv'alido"));
        return; 
        
    }
    
    currentColor = colorName;
}

    public void Visit(SizeStmt node)
    {
        int size =(int) Evaluate(node.SizeValue);
    
        // Asegurar que el tamaño sea positivo e impar
        currentSize = Math.Max(1, Math.Abs(size));
        if (currentSize % 2 == 0)
        {
            currentSize--; // Hacer impar
        }
    }

    private void DrawBrushAt(int centerX, int centerY, Color color)
{
    if (color == Color.Transparent) return;
    
    int halfSize = currentSize / 2;
    
    for (int dx = -halfSize; dx <= halfSize; dx++)
    {
        for (int dy = -halfSize; dy <= halfSize; dy++)
        {
            int targetX = centerX + dx;
            int targetY = centerY + dy;
            
            if (targetX >= 0 && targetX < canvas.Size && 
                targetY >= 0 && targetY < canvas.Size /*&& Around(centerX, centerY, targetX, targetY, halfSize)*/)
            {
                canvas.SetPixel(targetX, targetY, color);
            }
        }
    }
}

private void FixLine(int centerX, int centerY, Color color)
{
    int halfSize = currentSize / 2;
    
    for (int dx = 0; dx <= halfSize; dx++)
    {
        for (int dy = 0; dy <= halfSize; dy++)
        {
            int targetX = centerX + dx;
            int targetY = centerY + dy;
            
            if (targetX >= 0 && targetX < canvas.Size && 
                targetY >= 0 && targetY < canvas.Size && Around(centerX, centerY, targetX, targetY, halfSize))
            {
                canvas.SetPixel(targetX, targetY, color);
            }
        }
    }
}

    public void Visit(DrawLineStmt node)
{
    int dx = (int)Evaluate(node.DirX);
    int dy = (int)Evaluate(node.DirY);
    int dist = (int)Evaluate(node.Distance);
    Color color = GetCurrentColor();
    
    // Validar dirección
    if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
    {
        CompilingErrors.Add(new CompilingError(node.StartToken.Line, ErrorCode.Invalid, ErrorStage.Runtime, $"Dirección inválida: ({dx}, {dy}). Valores deben ser -1, 0 o 1"));
    }

    for (int i = 0; i < dist; i++)
            {
                DrawBrushAt(posX, posY, color);; // Draw the current pixel with brush size
                posX += dx;
                posY += dy;
            }
            //DrawBrushAt(posX, posY, color); // Draw the last pixel
    
    /*// Calcular puntos de la línea usando Bresenham
    int x0 = posX;
    int y0 = posY;
    int x1 = posX + dx * dist;
    int y1 = posY + dy * dist;
    
    bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
    if (steep)
    {
        Swap(ref x0, ref y0);
        Swap(ref x1, ref y1);
    }
    if (x0 > x1)
    {
        Swap(ref x0, ref x1);
        Swap(ref y0, ref y1);
    }
    
    int deltax = x1 - x0;
    int deltay = Math.Abs(y1 - y0);
    int error = deltax / 2;
    int ystep = (y0 < y1) ? 1 : -1;
    int y = y0;
    
    for (int x = x0; x <= x1; x++)
    {
        if (steep)
            DrawBrushAt(y, x, color);
        else
            DrawBrushAt(x, y, color);
        
        error -= deltay;
        if (error < 0)
        {
            y += ystep;
            error += deltax;
        }
    }*/
    
    // Actualizar posición final
    //posX = x1;
    //posY = y1;
    canvas.WallE_X = posX;
    canvas.WallE_Y = posY;
}

private void Swap(ref int a, ref int b)
{
    int temp = a;
    a = b;
    b = temp;
}

    public void Visit(DrawCircleStmt node)
{
    int centerDx = (int)Evaluate(node.DirX);
    int centerDy = (int)Evaluate(node.DirY);
    int radius = (int)Evaluate(node.Radius);
    int centerX = posX + centerDx;
    int centerY = posY + centerDy;
    Color color = GetCurrentColor();
    
    // Algoritmo optimizado para círculos con grosor
    /*int x = 0;
    int y = radius;
    int d = 3 - 2 * radius;
    
    while (y >= x)
    {
        // Solo dibujar puntos necesarios para evitar solapamientos
        DrawBrushAt(centerX + x, centerY + y, color);
        DrawBrushAt(centerX - x, centerY + y, color);
        DrawBrushAt(centerX + x, centerY - y, color);
        DrawBrushAt(centerX - x, centerY - y, color);
        DrawBrushAt(centerX + y, centerY + x, color);
        DrawBrushAt(centerX - y, centerY + x, color);
        DrawBrushAt(centerX + y, centerY - x, color);
        DrawBrushAt(centerX - y, centerY - x, color);
        
        x++;
        if (d > 0)
        {
            y--;
            d += 4 * (x - y) + 10;
        }
        else
        {
            d += 4 * x + 6;
        }
    }*/

    for (int i = -radius; i <= radius; i++)
            {

                int pixelX = centerX + i;
                int Image = CircleImage(i, radius);

                for (int j = 0; j < 2; j++)
                {
                    int pixelY = (int)Math.Pow(-1, j) * Image + centerY;

                    if (pixelX >= 0 && pixelX < canvas.Size && pixelY >= 0 && pixelY < canvas.Size)
                    {
                        DrawBrushAt(pixelX, pixelY, color);
                    }
                }
            }

    for (int i = -radius; i <= radius; i++)
            {

                int pixelY =centerY  + i;
                int Image = CircleImage(i, radius);

                for (int j = 0; j < 2; j++)
                {
                    int pixelX = (int)Math.Pow(-1, j) * Image + centerX;

                    if (pixelX >= 0 && pixelX < canvas.Size && pixelY >= 0 && pixelY < canvas.Size)
                    {
                        DrawBrushAt(pixelX, pixelY, color);
                    }
                }
            }
    
    // Actualizar posición al centro del círculo

    posX = centerX;
    posY = centerY;
    canvas.WallE_X = posX;
    canvas.WallE_Y = posY;
}

public bool Around(int x, int y, int a, int b, int brushSize) => Math.Pow((x - a), 2) + Math.Pow((y - b), 2) <= Math.Pow(brushSize, 2);

public int CircleImage(int i, int radius)
        {
            int Y = (int)(Math.Pow(radius, 2) - Math.Pow(i, 2));
            Y = Math.Abs(Y);

            return Math.Round(Math.Sqrt(Y),3) >= (int)Math.Sqrt(Y) + 0.5? (int)Math.Sqrt(Y)+1: (int)Math.Sqrt(Y);
        }



    public void Visit(DrawRectangleStmt node)
{
    int dx = (int)Evaluate(node.DirX);
    int dy = (int)Evaluate(node.DirY);
    int distance = (int)Evaluate(node.Distance);
    int width = (int)Evaluate(node.Width);
    int height = (int)Evaluate(node.Height);
    Color color = GetCurrentColor();
    
    // Calcular la posición de inicio real (esquina superior izquierda)
    int startX = posX + dx * distance;
    int startY = posY + dy * distance;
    
    // Calcular el área completa que debe cubrir el rectángulo
    //int endX = startX + width;
    //int endY = startY + height;
    
    // Dibujar todo el rectángulo (no solo el contorno)
    int MidWidth = width / 2;
    int MidHeight = height / 2;

            for(int i = -MidWidth-1; i <= MidWidth+1; i++)
            {
                for(int j = -MidHeight-1; j <= MidHeight+1; j++)
                {
                    if(i == -MidWidth - 1 || j == -MidHeight - 1 || i == MidWidth + 1 || j == MidHeight + 1)
                    {
                        int pixelX = startX + i;
                        int pixelY = startY + j;

                        if (pixelX >= 0 && pixelX < canvas.Size && pixelY >= 0 && pixelY < canvas.Size)
                        {
                            DrawBrushAt(pixelX, pixelY, color);
                        }
                    }
                }
            }
    
    // Actualizar posición al punto final (esquina inferior derecha)
    posX = startX;
    posY = startY;
    canvas.WallE_X = posX;
    canvas.WallE_Y = posY;
}

    private Color GetCurrentColor()
    {
        if (currentColor == "Transparent") return Color.Transparent;
        return ColorMap[currentColor];
    }

    

    private void FloodFill(int x, int y, Color target, Color replacement, FillStmt node)
    {
        if (target == replacement) return;
        if (canvas.GetPixel(x, y) != target) return;

        if (x < 0 || x >= canvas.Size || y < 0 || y >= canvas.Size)
        {
            CompilingErrors.Add(new CompilingError(node.StartToken.Line, ErrorCode.Invalid, ErrorStage.Runtime, $"Pixel coordinates ({x}, {y}) are out of canvas bounds.")); 
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

        FloodFill(posX, posY, target, replacement, node);
    }

    public void Visit(AssignmentStmt node)
{
    object value = Evaluate(node.Expression);
    
    if (value is int intValue)
    {
        variables[node.VariableName] = intValue;
    }
    else if (value is bool boolValue)
    {
        variables[node.VariableName] = boolValue ? 1 : 0;
    }
    else
    {
        CompilingErrors.Add(new CompilingError(node.StartToken.Line, ErrorCode.Invalid, ErrorStage.Runtime, $"Tipo no soportado para asignación: {value.GetType().Name}")); 
    }
}

    public void Visit(Label node)
    {
        // No hace nada en tiempo de ejecución
    }

    public void Visit(GoTo node)
    {
        // Control de flujo lo maneja ProgramRunner
    }

    



    public object  Evaluate(Expr expr)
{
    return expr switch
    {
        GetActualX =>canvas.WallE_X,
        GetActualY =>canvas.WallE_X,
        Literal lit => lit.Value,
        GetCanvasSize =>canvas.Size,
        IsBrushColor w=> currentColor==w.ColorExpression.ToString()? 1:0,
        IsBrushSize r=>currentSize==Int32.Parse(r.SizeValue.ToString()),
        IsCanvasColor m=>EvaluateIsCanvasColor(m),
        GetColorCount n=>EvaluateGetColorCount(n),
        Var v => variables.ContainsKey(v.Name) ? variables[v.Name] : 0,
        AddExpr add => (int)Evaluate(add.Left) + (int)Evaluate(add.Right),
        SubtractExpr sub => (int)Evaluate(sub.Left) - (int)Evaluate(sub.Right),
        MultiplyExpr mul => (int)Evaluate(mul.Left) * (int)Evaluate(mul.Right),
        DivideExpr div => (int)Evaluate(div.Left) / (int)Evaluate(div.Right),
        ModuloExpr mod => (int)Evaluate(mod.Left) % (int)Evaluate(mod.Right),
        PowerExpr pow => (int)Math.Pow((int)Evaluate(pow.Left), (int)Evaluate(pow.Right)),
        
        // Comparaciones
        EqualExpr eq => (int)Evaluate(eq.Left) == (int)Evaluate(eq.Right) ? 1 : 0,
        NotEqualExpr ne => (int)Evaluate(ne.Left) != (int)Evaluate(ne.Right) ? 1 : 0,
        GreaterExpr gt => (int)Evaluate(gt.Left) > (int)Evaluate(gt.Right) ? 1 : 0,
        LessExpr lt => (int)Evaluate(lt.Left) < (int)Evaluate(lt.Right) ? 1 : 0,
        GreaterEqualExpr ge => (int)Evaluate(ge.Left) >= (int)Evaluate(ge.Right) ? 1 : 0,
        LessEqualExpr le => (int)Evaluate(le.Left) <= (int)Evaluate(le.Right) ? 1 : 0,
        
        // Lógicos
        AndExpr and => (int)Evaluate(and.Left) != 0 && (int)Evaluate(and.Right) != 0 ? 1 : 0,
        OrExpr or => (int)Evaluate(or.Left) != 0 || (int)Evaluate(or.Right) != 0 ? 1 : 0,
        NotExpr not => (int)Evaluate(not.Operand) == 0 ? 1 : 0,
        
        // Unarios
        NegateExpr neg => -(int)Evaluate(neg.Operand),
        
        // Agrupación
        Grouping group => Evaluate(group.Expression),
        
        // Llamadas a función
        //FunctionCall call => EvaluateFunction(call),

        
        
        _ => Aux(expr) 
            
    };
    
}

    private object Aux(Expr expr)
    {
        CompilingErrors.Add(new CompilingError(expr.StartToken.Line, ErrorCode.Invalid, ErrorStage.Runtime, $"Expresi'on no soportada"));

        return 0;
    }

    private object EvaluateFunction(FunctionCall call)
{
    switch (call.FunctionName)
    {
        case "GetActualX":
            return posX;
            
        case "GetActualY":
            return posY;
            
        case "GetCanvasSize":
            return canvas.Size;
        case "IsBrushColor":
            return EvaluateIsBrushColor(call);
            
        case "IsBrushSize":
            return EvaluateIsBrushSize(call);
        default:
            throw new Exception($"Función desconocida: {call.FunctionName}");
    }
}

private int EvaluateGetColorCount(GetColorCount color)
{
    string colorName = (string)Evaluate(color.ColorExpression);
    int x1 = (int)Evaluate(color.DirX1);
    int y1 = (int)Evaluate(color.DirY1);
    int x2 = (int)Evaluate(color.DirX2);
    int y2 = (int)Evaluate(color.DirY2);

    // Validar coordenadas
    if (x1 < 0 || y1 < 0 || x2 < 0 || y2 < 0 ||
        x1 >= canvas.Size || x2 >= canvas.Size || 
        y1 >= canvas.Size || y2 >= canvas.Size)
    {
        return 0;
    }

    // Ordenar coordenadas
    int minX = Math.Min(x1, x2);
    int maxX = Math.Max(x1, x2);
    int minY = Math.Min(y1, y2);
    int maxY = Math.Max(y1, y2);

    // Contar píxeles del color especificado
    int count = 0;
    Color targetColor = ColorMap[colorName];
    for (int x = minX; x <= maxX; x++)
    {
        for (int y = minY; y <= maxY; y++)
        {
            if (canvas.GetPixel(x, y) == targetColor)
            {
                count++;
            }
        }
    }
    return count;
}

private int EvaluateIsBrushColor(FunctionCall call)
{
    string colorName = (string)Evaluate(call.Arguments[0]);
    return currentColor == colorName ? 1 : 0;
}

private int EvaluateIsBrushSize(FunctionCall call)
{
    int size = (int)Evaluate(call.Arguments[0]);
    return currentSize == size ? 1 : 0;
}

private int EvaluateIsCanvasColor(IsCanvasColor color)
{
    string colorName = (string)Evaluate(color.ColorExpression);
    int vertical = (int)Evaluate(color.DirX);
    int horizontal = (int)Evaluate(color.DirY);

    int targetX = posX + horizontal;
    int targetY = posY + vertical;

    // Verificar límites del canvas
    if (targetX < 0 || targetX >= canvas.Size || 
        targetY < 0 || targetY >= canvas.Size)
    {
        return 0;
    }

    return canvas.GetPixel(targetX, targetY) == ColorMap[colorName] ? 1 : 0;
}
}
