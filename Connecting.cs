using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlLabCS
{
    public partial class Connecting : UserControl
    {
        private Functions F = new Functions();
        public Connecting(Functions fun)
        {
            InitializeComponent();
            F = fun;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (F.TryConnect(textBox1.Text))
            {
                this.Path = textBox1.Text;
                Podkluchilos.Invoke();
            }
                    
        }
    }
}
