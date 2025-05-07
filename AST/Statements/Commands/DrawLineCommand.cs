public class DrawLineCommand : Stmt
{
    public Expr DirX { get; }
    public Expr DirY { get; }
    public Expr Distance { get; }
    public DrawLineCommand(Expr dirX, Expr dirY, Expr distance)
    {
        DirX = dirX;
        DirY = dirY;
        Distance = distance;
    }
}