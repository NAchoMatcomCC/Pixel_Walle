public class SpawnCommand : Stmt
{
    public Expr X { get; } // Coordenada X (ej: 0)
    public Expr Y { get; } // Coordenada Y (ej: 0)
    public SpawnCommand(Expr x, Expr y)
    {
        X = x;
        Y = y;
    }
}