using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Segundo_Proyecto1._0
{
    public partial class CanvasForm : Form
    {
        private Image WALLE;
        public CanvasForm(CanvasData canvasData)
        {
            InitializeComponent();

            this.canvasData = canvasData;
            this.DoubleBuffered = true;
            this.Paint += CanvasForm_Paint;
            this.Resize += (s, e) => Invalidate();
            this.Text = "Canvas ampliado";
            this.WindowState = FormWindowState.Maximized;
            WALLE=Image.FromFile("IMG/WALL-E1.png");
        }

        private void CanvasForm_Paint(object sender, PaintEventArgs e)
        {
            int cellSize = Math.Min(ClientSize.Width, ClientSize.Height) / canvasData.Size;
            for (int x = 0; x < canvasData.Size; x++)
            {
                for (int y = 0; y < canvasData.Size; y++)
                {
                    using (Brush b = new SolidBrush(canvasData.GetPixel(x, y)))
                    {
                        e.Graphics.FillRectangle(b, x * cellSize, y * cellSize, cellSize, cellSize);
                        e.Graphics.DrawRectangle(Pens.Black, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                }
            }

            if(canvasData.WallE_X!=null){
            int drawX = canvasData.WallE_X * cellSize;
        int drawY = canvasData.WallE_Y * cellSize;
        
        // Dibujar la imagen ocupando toda la celda
        e.Graphics.DrawImage(
            WALLE,
            drawX,
            drawY,
            cellSize,
            cellSize
        );
        }
        }


    }
}
