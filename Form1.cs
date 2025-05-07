namespace Segundo_Proyecto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            splitContainer1.Panel2.Controls.Add(new Editor());
            splitContainer1.Panel2.Controls[0].Dock=DockStyle.Fill;

        }
    }
}
