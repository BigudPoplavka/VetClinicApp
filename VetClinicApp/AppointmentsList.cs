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
    public partial class AppointmentsList : Form
    {
        private string columnHeader = "Заявка";
        public AppointmentsList()
        {
            InitializeComponent();
            InitAppointmentsDataGrid();
            FillGridView();
        }

        private void InitAppointmentsDataGrid()
        {
            dataGridView1.Columns.Add(columnHeader, columnHeader);
        }

        private void FillGridView()
        {
            string[] records = Directory.GetFiles(StaticData.appointmentFolder);

            foreach(string record in records)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = new FileInfo(record).Name;
            }
        }

        private void AppointmentsList_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            string deletedRecord = e.Row.Cells[0].Value.ToString();
            string[] dirs = Directory.GetFiles(StaticData.appointmentFolder);

            foreach (string dir in dirs)
            {
                if (dir.EndsWith(deletedRecord))
                {
                    File.Delete(dir);
                    MessageBox.Show(StaticData.deleted);
                    break;
                }
            }
        }
    }
}
