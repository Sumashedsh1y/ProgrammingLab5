using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_5_Semestr_3
{
    public partial class StatisticsForm : Form
    {
        public StatisticsForm(IDictionary<string, int> stats)
        {
            InitializeComponent();
            foreach (KeyValuePair<string, int> p in stats)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                row.Cells[0].Value = p.Key;
                row.Cells[1].Value = p.Value;
                dataGridView1.Rows.Add(row);
            }
        }
    }
}
