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
    public partial class Main : UserControl
    {
        private Functions F = new Functions();

        private Names N;
        private Subjects S;
        private Living L;

        public Main(Functions fun)
        {
            InitializeComponent();
            F = fun;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            N = new Names(F);
            S = new Subjects(F);
            L = new Living(F);
        }

        private void namesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F.WhereOperate = "GroupNames";
            F.Refresh(dataGridView1, F.WhereOperate);
            panel1.Controls.Clear();
            panel1.Controls.Add(N);
        }

        private void subjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F.WhereOperate = "GroupStudying";
            F.Refresh(dataGridView1, F.WhereOperate);
            panel1.Controls.Clear();
            panel1.Controls.Add(S);
        }

        private void livingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F.WhereOperate = "GroupLiving";
            F.Refresh(dataGridView1, F.WhereOperate);
            panel1.Controls.Clear();
            panel1.Controls.Add(L);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (panel1.Contains(N))
            {
                F.Values = N.Ret();
            }

            if (panel1.Contains(S))
            {
                F.Values = S.Ret();
            }

            if (panel1.Contains(L))
            {
                F.Values = L.Ret();
            }
            F.Add(F.WhereOperate, F.Values);
            F.Refresh(dataGridView1, F.WhereOperate);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (panel1.Contains(N))
            {
                F.Values = N.Ret();
            }

            if (panel1.Contains(S))
            {
                F.Values = S.Ret();
            }

            if (panel1.Contains(L))
            {
                F.Values = L.Ret();
            }
            F.Delete(F.WhereOperate, F.Values);
            F.Refresh(dataGridView1, F.WhereOperate);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (panel1.Contains(N))
            {
                F.Values = N.Ret();
            }

            if (panel1.Contains(S))
            {
                F.Values = S.Ret();
            }

            if (panel1.Contains(L))
            {
                F.Values = L.Ret();
            }
            F.Update(F.WhereOperate, F.Values);
            F.Refresh(dataGridView1, F.WhereOperate);
        }
    }
}
