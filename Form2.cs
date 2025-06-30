using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Segundo_Proyecto1._0
{
    public partial class CanvasForm : Form
    {
        private Image WALLE;
        //private CanvasData canvasData;
        private Dictionary<int, Image> wallECache = new Dictionary<int, Image>();
        private Bitmap cachedBitmap;
        private Size lastSize = Size.Empty;
        private readonly List<CompilingError> errors;
        private ListBox errorList;
        
        public CanvasForm(CanvasData canvasData, List<CompilingError> errors)
        {
            InitializeComponent();
            this.errors=errors;
            this.canvasData = canvasData;
            this.DoubleBuffered = true;
            this.Paint += CanvasForm_Paint;
            this.Resize += CanvasForm_Resize;
            this.Text = "Canvas ampliado";
            this.WindowState = FormWindowState.Maximized;
            
            // Cargar imagen de Wall-E
            WALLE = Image.FromFile("IMG/WALL-E1.png");

            InitializeErrorList();
        }

        private void CanvasForm_Resize(object sender, EventArgs e)
        {
            // Solo invalidar si el tamaño cambió significativamente
            if (cachedBitmap == null || ClientSize.Width != lastSize.Width || ClientSize.Height != lastSize.Height)
            {
                cachedBitmap?.Dispose();
                cachedBitmap = null;
                lastSize = ClientSize;
                Invalidate();
            }
        }

        private void InitializeErrorList()
        {
            

            errorList = new ListBox
            {
                Dock = DockStyle.Right,
                Width = 750,
                Font = new Font("Consolas", 10),
                ForeColor = Color.Red
            };

            foreach (var err in errors)
            {
                errorList.Items.Add($"[Línea {err.Line}] {err.Stage}: {err.Argument}");
            }

            Controls.Add(errorList);
        }

        private void CanvasForm_Paint(object sender, PaintEventArgs e)
        {
            // Obtener el tamaño del cliente
            int width = ClientSize.Width;
            int height = ClientSize.Height;
            
            // Si no tenemos un bitmap cacheado o el tamaño cambió, crear uno nuevo
            if (cachedBitmap == null || cachedBitmap.Width != width || cachedBitmap.Height != height)
            {
                cachedBitmap?.Dispose();
                cachedBitmap = new Bitmap(width, height);
                
                using (Graphics g = Graphics.FromImage(cachedBitmap))
                {
                    // Calcular tamaño de celda
                    int cellSize = Math.Min(width, height) / canvasData.Size;
                    
                    // Dibujar todas las celdas
                    for (int x = 0; x < canvasData.Size; x++)
                    {
                        for (int y = 0; y < canvasData.Size; y++)
                        {
                            using (Brush b = new SolidBrush(canvasData.GetPixel(x, y)))
                            {
                                g.FillRectangle(b, x * cellSize, y * cellSize, cellSize, cellSize);
                                g.DrawRectangle(Pens.Black, x * cellSize, y * cellSize, cellSize, cellSize);
                            }
                        }
                    }
                }
            }
            
            // Dibujar el bitmap cacheado
            e.Graphics.DrawImage(cachedBitmap, 0, 0);
            
            // Dibujar Wall-E encima si está presente
            if (canvasData.WallE_X != null && canvasData.WallE_Y != null)
            {
                int cellSize = Math.Min(ClientSize.Width, ClientSize.Height) / canvasData.Size;
                int x = canvasData.WallE_X;
                int y = canvasData.WallE_Y;
                
                Image wallEImage = GetWallEImage(cellSize);
                e.Graphics.DrawImage(
                    wallEImage,
                    x * cellSize,
                    y * cellSize
                );
            }
        }

        private Image GetWallEImage(int cellSize)
        {
            // Usar imagen cacheada si está disponible
            if (wallECache.TryGetValue(cellSize, out Image cachedImage))
            {
                return cachedImage;
            }
            
            // Crear nueva imagen redimensionada
            double scale = Math.Min(1.0, cellSize / (double)WALLE.Width);
            int newWidth = (int)(WALLE.Width * scale);
            int newHeight = (int)(WALLE.Height * scale);
            
            Image resizedImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(WALLE, 0, 0, newWidth, newHeight);
            }
            
            // Guardar en cache
            wallECache[cellSize] = resizedImage;
            return resizedImage;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            
            // Limpiar recursos
            cachedBitmap?.Dispose();
            
            foreach (var image in wallECache.Values)
            {
                image.Dispose();
            }
            wallECache.Clear();
        }
    }
}