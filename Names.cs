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
    public partial class Names : UserControl
    {
        private Functions F;
        public Names(Functions fun)
        {
            InitializeComponent();
            F = fun;
        }

        public string[] Ret()
        {
            string[] v = { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text };
            return v;
        }
    }
}
