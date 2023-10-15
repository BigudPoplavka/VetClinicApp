using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace VetClinicApp
{
    public partial class EmplList : Form
    {
        AdminMenu parent;

        public List<string> columns = new List<string>() {
            "Имя", "Фамилия", "Отчество", "Возраст",
            "В профессии с", "Должность", "Эл.Почта", "Тел."
        };

        public EmplList(AdminMenu form)
        {
            parent = form;

            InitializeComponent();
            InitDataGrid();
            LoadLog();
        }

        private void InitDataGrid()
        {
            foreach (string column in columns)
                dataGridView1.Columns.Add(column, column);
        }

        private void LoadLog()
        {
            string[] curr;
            string[] dirs = Directory.GetDirectories(StaticData.employeesFolder);

            List<string> fields = new List<string>();

            foreach(string dir in dirs)
            {
                using (StreamReader reader = new StreamReader(dir + "/" + StaticData.passportfile))
                {
                    while (!reader.EndOfStream)
                    {
                        curr = reader.ReadLine().Split(new char[] { ':' });
                        fields.Add(curr[1]);
                    }
                }

                dataGridView1.Rows.Add();

                    for (int i = 0; i < fields.Count; i++)
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[i].Value = fields[i];
                
                fields.Clear();
            }
        }

        private void EmplList_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            string deletedEmpl = e.Row.Cells[0].Value.ToString() + "_" + e.Row.Cells[1].Value.ToString() + "_" + e.Row.Cells[2].Value.ToString();

            string[] dirs = Directory.GetDirectories(StaticData.employeesFolder);

            foreach(string dir in dirs)
            {
                if(dir.EndsWith(deletedEmpl))
                {
                    Directory.Delete(dir, true);
                    MessageBox.Show(StaticData.deleted);
                }

                break;
            }

            parent.FillEmplList();
        }
    }
}
