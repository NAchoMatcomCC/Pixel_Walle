namespace Segundo_Proyecto1._0
{
    public class CanvasData
{
    public int WallE_X { get; set; }
    public int WallE_Y { get; set; }
    public int Size { get; private set; }
    public Color[,] Colors { get; private set; }

    public CanvasData(int size)
    {
        Size = size;
        Colors = new Color[size, size];
        Clear(Color.White);
        WallE_X=0;
        WallE_Y=0;
    }

    public void Clear(Color color)
    {
        for (int x = 0; x < Size; x++)
            for (int y = 0; y < Size; y++)
                Colors[x, y] = color;
    }

    public void SetPixel(int x, int y, Color color)
    {
        if (x >= 0 && x < Size && y >= 0 && y < Size)
            Colors[x, y] = color;
    }

    public Color GetPixel(int x, int y)
    {
        if (x >= 0 && x < Size && y >= 0 && y < Size)
            return Colors[x, y];
        return Color.Transparent;
    }
}
   
}