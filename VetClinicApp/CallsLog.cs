using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace VetClinicApp
{
    public partial class CallsLog : Form
    {
        private List<string> columns = new List<string>() { "Время", "Телефон", "ФИО" };

        public CallsLog()
        {
            InitializeComponent();
            InitDataGrid();
            LoadLog();
        }

        private void InitDataGrid()
        {
            foreach(string column in columns)
                dataGridView1.Columns.Add(column, column);
        }

        private void LoadLog()
        {
            string[] curr;

            using (StreamReader reader = new StreamReader(StaticData.callsFile))
            {
                while(!reader.EndOfStream)
                {
                    curr = reader.ReadLine().Split(new char[] { '|' });

                    dataGridView1.Rows.Add();

                    for (int i = 0; i < columns.Count; i++)
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[i].Value = curr[i];
                }
            }
        }

        private void CallsLog_Load(object sender, EventArgs e)
        {

        }
    }
}
