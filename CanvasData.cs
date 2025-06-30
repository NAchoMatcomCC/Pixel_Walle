namespace Segundo_Proyecto1._0
{
    public class CanvasData
{
    public int WallE_X { get; set; }
    public int WallE_Y { get; set; }
    public int Size { get; private set; }
    public Color[,] Colors { get; private set; }
    private Bitmap cachedBitmap;
    private bool needsRedraw;

    public CanvasData(int size)
    {
        Size = size;
        Colors = new Color[size, size];
        Clear(Color.White);
        WallE_X=0;
        WallE_Y=0;
        needsRedraw=true;
    }

    public void MarkDirty()
    {
        needsRedraw = true;
    }
    
    public Bitmap GetBitmap(int width, int height)
    {
        if (cachedBitmap == null || cachedBitmap.Width != width || cachedBitmap.Height != height)
        {
            cachedBitmap?.Dispose();
            cachedBitmap = new Bitmap(width, height);
            needsRedraw = true;
        }
        
        if (needsRedraw)
        {
            using (Graphics g = Graphics.FromImage(cachedBitmap))
            {
                int cellWidth = width / Size;
                int cellHeight = height / Size;
                
                for (int x = 0; x < Size; x++)
                {
                    for (int y = 0; y < Size; y++)
                    {
                        using (Brush b = new SolidBrush(Colors[x, y]))
                        {
                            g.FillRectangle(b, x * cellWidth, y * cellHeight, cellWidth, cellHeight);
                            g.DrawRectangle(Pens.Black, x * cellWidth, y * cellHeight, cellWidth, cellHeight);
                        }
                    }
                }
            }
            needsRedraw = false;
        }
        
        return cachedBitmap;
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
        {
            Colors[x, y] = color;
            MarkDirty();
        }

    }

    public Color GetPixel(int x, int y)
    {
        if (x >= 0 && x < Size && y >= 0 && y < Size)
            return Colors[x, y];
        return Color.Transparent;
    }
}
   
}