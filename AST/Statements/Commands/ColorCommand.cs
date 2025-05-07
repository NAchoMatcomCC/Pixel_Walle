public class ColorCommand : Stmt
{
    public Expr Color { get; } // Expresión que retorna un color (ej: "Red")
    public ColorCommand(Expr color) => Color = color;
}