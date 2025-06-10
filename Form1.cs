using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Segundo_Proyecto1._0
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SetupEditor();

            SetDoubleBuffered(lineNumberPanel, true);
            currentFilePath = null;

            InitCanvas(16);
        }

        // Configuración inicial del editor
        private void SetupEditor()
        {
            codeEditor.Font = new Font("Consolas", 10);
            codeEditor.Text = "Spawn(0, 0)\nColor(\"Red\")\nDrawLine(1, 0, 10)";

            // Configuración para scroll horizontal
            codeEditor.WordWrap = false;
            codeEditor.ScrollBars = RichTextBoxScrollBars.ForcedBoth;

            // Evento para scroll horizontal
            codeEditor.HScroll += CodeEditor_HScroll;

            // Evento para zoom con Ctrl+Rueda
            codeEditor.MouseWheel += CodeEditor_MouseWheel;
        }

        // Habilitar doble búfer para reducir parpadeo
        public static void SetDoubleBuffered(Control control, bool enabled)
        {
            var prop = typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            prop.SetValue(control, enabled, null);
        }



        private void LineNumberPanel_Paint(object sender, PaintEventArgs e)
        {
            DrawLineNumbers(e.Graphics);
        }

        private void DrawLineNumbers(Graphics g)
        {
            try
            {
                g.Clear(lineNumberPanel.BackColor);
                Brush brush = Brushes.Black;
                Font font = codeEditor.Font;

                // Obtener el índice del primer carácter visible
                int firstCharIndex = codeEditor.GetCharIndexFromPosition(new Point(0, 0));

                // Obtener el número de la primera línea visible
                int firstVisibleLine = codeEditor.GetLineFromCharIndex(firstCharIndex);

                // Obtener la posición Y del primer carácter visible
                Point firstCharPos = codeEditor.GetPositionFromCharIndex(firstCharIndex);

                // Calcular el número de líneas visibles
                int visibleLineCount = codeEditor.Height / codeEditor.Font.Height + 2;

                // Ajuste para mantener la alineación vertical
                int verticalAdjustment = 0;

                // Dibujar números de línea
                for (int i = 0; i < visibleLineCount; i++)
                {
                    int lineNumber = firstVisibleLine + i;
                    if (lineNumber >= codeEditor.Lines.Length) break;

                    // Obtener el índice del primer carácter de esta línea
                    int lineStartIndex = codeEditor.GetFirstCharIndexFromLine(lineNumber);
                    if (lineStartIndex < 0) continue;

                    // Obtener la posición del primer carácter de esta línea
                    Point linePos = codeEditor.GetPositionFromCharIndex(lineStartIndex);

                    // Calcular posición vertical ajustada
                    float yPos = linePos.Y - firstCharPos.Y + verticalAdjustment;

                    // Solo dibujar si está dentro del área visible
                    if (yPos >= -codeEditor.Font.Height && yPos < codeEditor.Height)
                    {
                        g.DrawString(
                            (lineNumber + 1).ToString(), // +1 porque las líneas empiezan en 0
                            font,
                            brush,
                            new PointF(lineNumberPanel.Width - g.MeasureString((lineNumber + 1).ToString(), font).Width - 5,
                            yPos
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar posibles errores
                Console.WriteLine($"Error dibujando números de línea: {ex.Message}");
            }
        }


        // Evento para scroll horizontal
        private void CodeEditor_HScroll(object sender, EventArgs e)
        {
            lineNumberPanel.Invalidate();
        }

        // Evento para zoom con Ctrl + Rueda del ratón
        private void CodeEditor_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                if (e.Delta > 0)
                    codeEditor.ZoomFactor += 0.1f;
                else
                    codeEditor.ZoomFactor = Math.Max(0.5f, codeEditor.ZoomFactor - 0.1f);

                lineNumberPanel.Invalidate();
            }
        }

        // Método para obtener posición del scroll horizontal (opcional)
        private int GetHorizontalScrollPosition()
        {
            const int EM_GETSCROLLPOS = 0x04DD;
            var point = new NativeMethods.POINT();
            NativeMethods.SendMessage(codeEditor.Handle, EM_GETSCROLLPOS, IntPtr.Zero, ref point);
            return point.X;
        }

        // Clase para métodos nativos de Windows
        internal static class NativeMethods
        {
            public const int SB_HORZ = 0;
            public const int SB_VERT = 1;
            public const int SB_BOTH = 3;

            [DllImport("user32.dll")]
            public static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

            [StructLayout(LayoutKind.Sequential)]
            public struct POINT
            {
                public int X;
                public int Y;
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref POINT lParam);
        }

        private void CodeEditor_VScroll(object sender, EventArgs e)
        {
            lineNumberPanel.Invalidate();
        }

        private void CodeEditor_TextChanged(object sender, EventArgs e)
        {
            lineNumberPanel.Invalidate();
        }

        private void CodeEditor_FontChanged(object sender, EventArgs e)
        {
            lineNumberPanel.Invalidate();
        }

        private void CodeEditor_Resize(object sender, EventArgs e)
        {
            lineNumberPanel.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pixel Wall-E files (*.pw)|*.pw|All files (*.*)|*.*";
            openFileDialog.Title = "Cargar archivo .pw";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileContent = System.IO.File.ReadAllText(openFileDialog.FileName);
                    codeEditor.Text = fileContent;
                    currentFilePath = openFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Pixel Wall-E files (*.pw)|*.pw|All files (*.*)|*.*";
                saveFileDialog.Title = "Guardar archivo .pw";
                saveFileDialog.DefaultExt = "pw";
                saveFileDialog.AddExtension = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        System.IO.File.WriteAllText(saveFileDialog.FileName, codeEditor.Text);
                        currentFilePath = saveFileDialog.FileName; // Guardar la ruta nueva
                        MessageBox.Show("Archivo guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al guardar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                try
                {
                    System.IO.File.WriteAllText(currentFilePath, codeEditor.Text);
                    MessageBox.Show("Archivo guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }


        private void InitCanvas(int size)
        {
            canvasData = new CanvasData(size);
            canvas_Panel.Invalidate(); // Redibuja
        }



        private void canvas_Panel_Paint(object sender, PaintEventArgs e)
        {
            int cellSize = canvas_Panel.Width / canvasData.Size;
            for (int x = 0; x < canvasData.Size; x++)
            {
                for (int y = 0; y < canvasData.Size; y++)
                {
                    Color color = canvasData.GetPixel(x, y);
                    using (Brush b = new SolidBrush(color))
                    {
                        e.Graphics.FillRectangle(b, x * cellSize, y * cellSize, cellSize, cellSize);
                        e.Graphics.DrawRectangle(Pens.Gray, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                }
            }
        }

        private void canvas_Panel_MouseClick(object sender, MouseEventArgs e)
        {
            CanvasForm canvasForm = new CanvasForm(canvasData);
            canvasForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string input = Interaction.InputBox("Introduce el tamaño del canvas (ej: 64):", "Redimensionar canvas", "32");

            if (int.TryParse(input, out int newSize) && newSize > 0)
            {
                InitCanvas(newSize);
            }
            else if (!string.IsNullOrWhiteSpace(input)) // Solo mostrar error si no fue cancelado
            {
                MessageBox.Show("Entrada inválida. Introduce un número entero mayor que cero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string code = codeEditor.Text;
            var lexer = new Lexer(code);
            var tokens1=new TokenStream(lexer.tokens);
        var parser = new PixelParser(tokens1);
        var listaerrores=new List<CompilingError>();
        var program = parser.ParseProgram(listaerrores);
        
      
        var runner = new ProgramRunner(program, canvasData);
        runner.Run();
        
      
        //canvas_Panel.Invalidate();
        
        
        }
    }
}

