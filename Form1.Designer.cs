namespace Segundo_Proyecto1._0
{
    partial class MainForm
    {
        private CanvasData canvasData;

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel lineNumberPanel;
        private System.Windows.Forms.RichTextBox codeEditor;

        //private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCanvas;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.SplitContainer editorSplitContainer;
        //private System.Windows.Forms.Panel lineNumberPanel;
        //private System.Windows.Forms.RichTextBox codeEditor;

        private string currentFilePath;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                splitContainer?.Dispose();
                lineNumberPanel?.Dispose();
                canvas_Panel?.Dispose();
                codeEditor?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            splitContainer = new SplitContainer();
            lineNumberPanel = new Panel();
            canvas_Panel = new Panel();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            codeEditor = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            lineNumberPanel.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 0);
            splitContainer.Margin = new Padding(5, 6, 5, 6);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(lineNumberPanel);
            splitContainer.Panel1.Padding = new Padding(0, 0, 8, 0);
            splitContainer.Panel1MinSize = 30;
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(codeEditor);
            splitContainer.Size = new Size(1333, 865);
            splitContainer.SplitterDistance = 444;
            splitContainer.SplitterWidth = 7;
            splitContainer.TabIndex = 0;
            // 
            // lineNumberPanel
            // 
            lineNumberPanel.BackColor = SystemColors.ControlLight;
            lineNumberPanel.Controls.Add(canvas_Panel);
            lineNumberPanel.Controls.Add(button4);
            lineNumberPanel.Controls.Add(button3);
            lineNumberPanel.Controls.Add(button2);
            lineNumberPanel.Controls.Add(button1);
            lineNumberPanel.Dock = DockStyle.Fill;
            lineNumberPanel.Location = new Point(0, 0);
            lineNumberPanel.Margin = new Padding(5, 6, 5, 6);
            lineNumberPanel.Name = "lineNumberPanel";
            lineNumberPanel.Size = new Size(436, 865);
            lineNumberPanel.TabIndex = 0;
            lineNumberPanel.Paint += LineNumberPanel_Paint;
            // 
            // canvas_Panel
            // 
            canvas_Panel.Location = new Point(73, 475);
            canvas_Panel.Name = "canvas_Panel";
            canvas_Panel.Size = new Size(318, 319);
            canvas_Panel.TabIndex = 4;
            canvas_Panel.Paint += canvas_Panel_Paint;
            canvas_Panel.MouseClick += canvas_Panel_MouseClick;
            // 
            // button4
            // 
            button4.Location = new Point(73, 309);
            button4.Name = "button4";
            button4.Size = new Size(112, 34);
            button4.TabIndex = 3;
            button4.Text = "CANVAS";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.Location = new Point(73, 233);
            button3.Name = "button3";
            button3.Size = new Size(112, 34);
            button3.TabIndex = 2;
            button3.Text = "EJECUTAR";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.Location = new Point(73, 162);
            button2.Name = "button2";
            button2.Size = new Size(112, 34);
            button2.TabIndex = 1;
            button2.Text = "CARGAR";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(73, 93);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 0;
            button1.Text = "GUARDAR";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // codeEditor
            // 
            codeEditor.BorderStyle = BorderStyle.None;
            codeEditor.Dock = DockStyle.Fill;
            codeEditor.Font = new Font("Consolas", 10F);
            codeEditor.Location = new Point(0, 0);
            codeEditor.Margin = new Padding(5, 6, 5, 6);
            codeEditor.Name = "codeEditor";
            codeEditor.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            codeEditor.Size = new Size(882, 865);
            codeEditor.TabIndex = 0;
            codeEditor.Text = "";
            codeEditor.WordWrap = false;
            codeEditor.HScroll += CodeEditor_HScroll;
            codeEditor.VScroll += CodeEditor_VScroll;
            codeEditor.FontChanged += CodeEditor_FontChanged;
            codeEditor.TextChanged += CodeEditor_TextChanged;
            codeEditor.MouseWheel += CodeEditor_MouseWheel;
            codeEditor.Resize += CodeEditor_Resize;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1333, 865);
            Controls.Add(splitContainer);
            Margin = new Padding(5, 6, 5, 6);
            Name = "MainForm";
            Text = "Pixel Wall-E IDE";
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            lineNumberPanel.ResumeLayout(false);
            ResumeLayout(false);

            this.button3.Click += new System.EventHandler(this.button3_Click);
        }

        private Button button1;
        private Button button3;
        private Button button2;
        private Button button4;
        private Panel canvas_Panel;
    }
}