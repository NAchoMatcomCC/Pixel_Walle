using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Segundo_Proyecto.Properties;
using System.Drawing;
using FastColoredTextBoxNS;

namespace Segundo_Proyecto
{
    public partial class Editor : UserControl
    {
        public Editor()
        {
            InitializeComponent();

            tabControl1.TabPages[tabControl1.TabCount - 1].Text = "";
            tabControl1.Padding = new Point(12, 4);
            tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;

            tabControl1.DrawItem += tabControl1_DrawItem;
            tabControl1.MouseDown += tabControl1_MouseDown;
            tabControl1.Selecting += tabControl1_Selecting;
            tabControl1.HandleCreated += tabControl1_HandleCreated;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        private const int TCM_SETMINTABWIDTH = 0x1300 + 49;

        private void tabControl1_HandleCreated(object sender, EventArgs e)
        {
            SendMessage(tabControl1.Handle, TCM_SETMINTABWIDTH, IntPtr.Zero, (IntPtr)16);
        }
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == tabControl1.TabCount - 1)
                e.Cancel = true;
        }
        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            var lastIndex = tabControl1.TabCount - 1;
            if (tabControl1.GetTabRect(lastIndex).Contains(e.Location))
            {
                FastColoredTextBox texteEditor= new FastColoredTextBox();
                texteEditor.Dock=DockStyle.Fill;
                texteEditor.BringToFront();

                tabControl1.TabPages.Insert(lastIndex, "Sin título");
                tabControl1.SelectedIndex = lastIndex;
                tabControl1.TabPages[lastIndex].UseVisualStyleBackColor = true;

                tabControl1.TabPages[lastIndex].Controls.Add(texteEditor);
            }
            else
            {
                for (var i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    var tabRect = tabControl1.GetTabRect(i);
                    tabRect.Inflate(-2, -2);
                    var closeImage = Resource1.Close;
                    var imageRect = new Rectangle(
                        (tabRect.Right - closeImage.Width),
                        tabRect.Top + (tabRect.Height - closeImage.Height) / 2,
                        closeImage.Width,
                        closeImage.Height);
                    if (imageRect.Contains(e.Location))
                    {
                        tabControl1.TabPages.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tabPage = tabControl1.TabPages[e.Index];
            var tabRect = tabControl1.GetTabRect(e.Index);
            tabRect.Inflate(-2, -2);
            if (e.Index == tabControl1.TabCount - 1)
            {
                var addImage = Resource1.Add;
                e.Graphics.DrawImage(addImage,
                    tabRect.Left + (tabRect.Width - addImage.Width) / 2,
                    tabRect.Top + (tabRect.Height - addImage.Height) / 2);
            }
            else
            {
                var closeImage = Resource1.Close;
                e.Graphics.DrawImage(closeImage,
                    (tabRect.Right - closeImage.Width),
                    tabRect.Top + (tabRect.Height - closeImage.Height) / 2);
                TextRenderer.DrawText(e.Graphics, tabPage.Text, tabPage.Font,
                    tabRect, tabPage.ForeColor, TextFormatFlags.Left);
            }
        }


    }
}
