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
    public partial class EditService : Form
    {
        private string columnHeader = "Врач";
        private int doctCount;

        public EditService()
        {
            InitializeComponent();

            FillComboBox(comboBox1, StaticData.servicesFolder);
            FillComboBox(comboBox2, StaticData.employeesFolder);
            InitServiceDataGrid();
        }


        public void FillComboBox(ComboBox comboBox, string path)
        {
            string[] list = Directory.GetDirectories(path);

            string[] tmp;

            for (int i = 0; i < list.Length; i++)
            {
                tmp = list[i].Split(new char[] { '\\' });
                list[i] = tmp[tmp.Length - 1];
            }

            comboBox.Items.AddRange(list);
        }

        private void InitServiceDataGrid()
        {
            dataGridView1.Columns.Add(columnHeader, columnHeader);
        }

        private void button3_Click(object sender, EventArgs e)  // Сохранить
        {
            if(dataGridView1.Rows.Count == doctCount)
            {
                return;
            }
            else if(dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show(StaticData.invalidFields);
                return;
            }
            else if(dataGridView1.Rows.Count > doctCount)
            {
                string selected = comboBox1.SelectedItem.ToString();
                string targetDir = string.Empty;
                string currPath = string.Empty;
                string[] servList = Directory.GetDirectories(StaticData.servicesFolder);
                List<string> empls = new List<string>();

                foreach (string dir in servList)
                {
                    if (dir.EndsWith(selected))
                        targetDir = dir;
                }

                currPath = targetDir + "/" + StaticData.serviceEmpls;
                if (!File.Exists(currPath))
                {
                    File.Create(currPath).Close();
                }

                using (StreamWriter writer = new StreamWriter(currPath, true))
                {
                    for (int i = doctCount; i < dataGridView1.Rows.Count; i++)
                    {
                        writer.WriteLine(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    }
                }

                MessageBox.Show(StaticData.saved);
                doctCount = dataGridView1.Rows.Count;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBox1.SelectedItem.ToString();
            string currPath = string.Empty;
            string targetDir = string.Empty;
            string[] servList = Directory.GetDirectories(StaticData.servicesFolder);
            List<string> empls = new List<string>();

            foreach(string dir in servList)
            {
                if (dir.EndsWith(selected))
                    targetDir = dir;
            }

            dataGridView1.Rows.Clear();

            currPath = targetDir + "/" + StaticData.serviceEmpls;
            if (File.Exists(currPath))
            {
                using (StreamReader reader = new StreamReader(currPath))
                {
                    while (!reader.EndOfStream)
                        empls.Add(reader.ReadLine());
                }

                foreach(string empl in empls)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = empl;
                }

                doctCount = empls.Count;
            }
            else
            {

            }

        }

        private void button1_Click(object sender, EventArgs e)  // Добавить врача
        {
            if (comboBox2.SelectedItem != null)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value == comboBox2.SelectedItem)
                    {
                        MessageBox.Show(StaticData.alreadyExist);
                        return;
                    }
                }

                dataGridView1.Rows.Add(comboBox2.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show(StaticData.invalidFields);
                return;
            }
        }
    }
}
