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
    public partial class AdminMenu : Form
    {
        Form1 parent;

        private List<Control> addEmplForm;
        private List<Control> addServiceForm;

        public AdminMenu(Form1 form)
        {
            InitializeComponent();

            parent = form;

            addEmplForm = new List<Control>()
            {
                textBox1, textBox2, textBox3, maskedTextBox1, maskedTextBox2, textBox7, textBox4, maskedTextBox3
            };

            addServiceForm = new List<Control>()
            {
                textBox5
            };

            FillEmplList();
            InitServiceDataGrid();
        }

        private void InitServiceDataGrid()
        {
            dataGridView1.Columns.Add("Услуга", "Услуга");
            dataGridView1.Columns.Add("Цена", "Цена");
            dataGridView2.Columns.Add("Врач", "Врач");
        }

        public void FillEmplList()
        {
            string[] emplList = Directory.GetDirectories(StaticData.employeesFolder);

            string[] tmp;

            for (int i = 0; i < emplList.Length; i++)
            {
                tmp = emplList[i].Split(new char[] { '\\' });
                emplList[i] = tmp[tmp.Length - 1];
            }

            comboBox1.Items.AddRange(emplList);
        }

        public (List<string>, string) GetEmplInputsList()
        {
            List<string> data = new List<string>();

            foreach (Control control in addEmplForm)
                data.Add(control.Text);
            return (data, textBox6.Text);
        }

        private bool ValidateEmplInputs()
        {
            foreach (Control control in addEmplForm)
                if (String.IsNullOrEmpty(control.Text) || String.IsNullOrWhiteSpace(control.Text))
                    return false;
            if (int.Parse(maskedTextBox1.Text) < 18 || int.Parse(maskedTextBox2.Text) > DateTime.Now.Year)
                return false;
            if (String.IsNullOrEmpty(textBox6.Text) || String.IsNullOrWhiteSpace(textBox6.Text))
                return false;
            return true;
        }

        private bool ValidateServInputs()
        {
            if (String.IsNullOrEmpty(textBox5.Text) || String.IsNullOrWhiteSpace(textBox5.Text))
                return false;
            if (dataGridView1.Rows.Count == 1)
                return false;
            if (dataGridView2.Rows.Count == 0)
                return false;

            return true;
        }

        private void ClearEmplInputs()
        {
            foreach (Control control in addEmplForm)
                control.Text = string.Empty;
            textBox6.Text = string.Empty;

            MessageBox.Show(StaticData.saved);
        }

        private void ClearServInputs()
        {
            textBox5.Text = string.Empty;
            dataGridView1.Rows.Clear();
            
            MessageBox.Show(StaticData.saved);
        }

        private void AdminMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            parent.Show();
        }

        private void AdminMenu_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)  // Новый сотрудник
        {
            if(ValidateEmplInputs())
            {
                try
                {
                    var data = GetEmplInputsList();
                    Employee employee = new Employee(data.Item1, data.Item2);
                    employee.Serialize();

                    comboBox1.Items.Clear();
                    FillEmplList();
                    ClearEmplInputs();
                }
                catch(Exception)
                {
                    MessageBox.Show(StaticData.recordCreationError);
                    return;
                }
            }
            else
            {
                MessageBox.Show(StaticData.invalidFields);
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)  // Новая услуга
        {
            if (ValidateServInputs())
            {
                try
                {
                    int tmp = 0;
                    List<(string, string)> serviseOptions = new List<(string, string)>();
                    List<string> serviceDoctors = new List<string>();
                    (string, string) line = ("", "");

                    for(int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        serviceDoctors.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                    }

                    for(int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        if(dataGridView1.Rows[i].Cells[0].Value == null || dataGridView1.Rows[i].Cells[1].Value == null)
                        {
                            MessageBox.Show(StaticData.notFillFields);
                            return;
                        }
                        else
                        {
                            line.Item1 = dataGridView1.Rows[i].Cells[0].Value.ToString();
                            line.Item2 = dataGridView1.Rows[i].Cells[1].Value.ToString();

                            serviseOptions.Add(line);
                        }
                    }

                    Service service = new Service(textBox5.Text, serviseOptions, serviceDoctors);
                    service.Serialize();

                    ClearServInputs();
                }
                catch (Exception)
                {
                    MessageBox.Show(StaticData.recordCreationError);
                    return;
                }
            }
            else
            {
                MessageBox.Show(StaticData.invalidFields);
                return;
            }


        }

        private void button3_Click(object sender, EventArgs e)  // Добавить врача в услугу
        {
            if(comboBox1.SelectedItem != null)
            {
                foreach(DataGridViewRow row in dataGridView2.Rows)
                {
                    if(row.Cells[0].Value == comboBox1.SelectedItem)
                    {
                        MessageBox.Show(StaticData.alreadyExist);
                        return;
                    }
                }

                dataGridView2.Rows.Add(comboBox1.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show(StaticData.invalidFields);
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)  // Сотрудники
        {
            EmplList emplList = new EmplList(this);
            emplList.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)  //  Редактировать
        {
            EditService editService = new EditService();
            editService.ShowDialog();
        }
    }
}
