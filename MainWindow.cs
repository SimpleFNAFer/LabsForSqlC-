using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlLabCS
{
    public partial class MainWindow : Form
    {
        private Functions F = new Functions();
        private Connecting C = null;
        private Main M = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            C = new Connecting(F);
            panel1.Size = C.Size;
            panel1.Controls.Clear();
            panel1.Controls.Add(C);
            C.Podkluchilos += ChangeControl;
        }

        private void ChangeControl()
        {
            F.Connect = C.Path;
            M = new Main(F);
            panel1.Size = M.Size;
            panel1.Controls.Clear();
            panel1.Controls.Add(M);
        }
    }
}
