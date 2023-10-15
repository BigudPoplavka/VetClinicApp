using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VetClinicApp
{
    public partial class OperatorMenu : Form
    {
        Form1 parent;
        string formRole = "Оператор";
        string defTextTimes = "График";
        string emptyPhone;

        private List<ComboBox> comboBoxes = new List<ComboBox>();
        public OperatorMenu(Form1 form)
        {
            InitializeComponent();

            parent = form;
            emptyPhone = maskedTextBox1.Text;
            label12.Text = defTextTimes;

            comboBoxes.Add(comboBox1);
            comboBoxes.Add(comboBox2);
            comboBoxes.Add(comboBox3);

            InitTreeView();
            InitComboBoxLists();
        }

        private void OperatorMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            parent.Show();
        }

        private void InitComboBoxLists()
        {
            foreach (TreeNode node in treeView1.Nodes)
                comboBox1.Items.Add(node.Text);
        }

        private string GetServNameFromDirName(string path)
        {
            var tmp = path.Split(new char[] { '\\' });
            return tmp[tmp.Length-1];
        }

        private void InitTreeView()
        {
            string[] servDirs = Directory.GetDirectories(StaticData.servicesFolder);
            string[] servOptions;
            string[] currLine = new string[2];

            foreach(string dir in servDirs)
            {
                servOptions = Directory.GetFiles(dir);

                treeView1.Nodes.Add(GetServNameFromDirName(dir));

                foreach(string option in servOptions)
                {
                    if (GetServNameFromDirName(option) == StaticData.serviceEmpls)
                        continue;

                    using (StreamReader reader = new StreamReader(option))
                    {
                        while(!reader.EndOfStream)
                        {
                            currLine = reader.ReadLine().Split(new char[] { ':' });
                        }
                    }

                    treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Add(currLine[0] + " | " + currLine[1]);
                }
            }
        }

        private void OperatorMenu_Load(object sender, EventArgs e)
        {
            label2.Text = formRole;
            label3.Text = DateTime.Now.ToString();
        }

        private bool ValidatePhoneCallFields()
        {
            if (maskedTextBox1.Text == emptyPhone || String.IsNullOrWhiteSpace(textBox1.Text))
                return false;
            return true;
        }

        private void button1_Click(object sender, EventArgs e)  // Звонок
        {
            if(ValidatePhoneCallFields())
            {
                using (StreamWriter writer = new StreamWriter(StaticData.callsFile, true))
                {
                    writer.WriteLine(DateTime.Now.ToString() + "|" + maskedTextBox1.Text + "|" + textBox1.Text);
                }

                MessageBox.Show(StaticData.saved);
            }
            else
            {
                MessageBox.Show(StaticData.invalidFields);
                return;
            }
        }

        private Service GetSelectedService(string service, string selectedOption, List<string> doctors)
        {
            string[] dirs = Directory.GetDirectories(StaticData.servicesFolder);
            string targetDir = string.Empty;
            (string, string) option = ("", "");

            foreach (string dir in dirs)
                if (dir.EndsWith(service))
                    targetDir = dir;

            List<(string, string)> options = new List<(string, string)>();
            string[] tmp = Directory.GetFiles(targetDir);
            string[] curr = new string[2];

            foreach(string optionPath in tmp)
            {
                if (optionPath.EndsWith(StaticData.serviceEmpls))
                    continue;

                using(StreamReader reader = new StreamReader(optionPath))
                {
                    while(!reader.EndOfStream)
                    {
                        curr = reader.ReadLine().Split(new char[] { ':' });
                    }
                }

                options.Add((GetServNameFromDirName(optionPath), curr[1]));
            }

            Service selectedService = new Service(service, options, doctors);

            return selectedService;
        }

        private Client GetClient()
        {
            string fio = textBox1.Text;
            string targetClient = string.Empty;
            string[] clients = Directory.GetFiles(StaticData.clientsFolder);
            List<string> fields = new List<string>();
            string[] curr = new string[2];

            foreach (string record in clients)
            {
                if (GetServNameFromDirName(record) == fio.Replace(' ', '_'))
                {
                    targetClient = record;
                    break;
                }
            }

            if(targetClient == string.Empty)
            {
                return null;
            }

            using(StreamReader reader = new StreamReader(targetClient))
            {
                while(!reader.EndOfStream)
                {
                    curr = reader.ReadLine().Split(new char[] { ':' });
                    fields.Add(curr[1]);
                }
            }

            Client client = new Client(fields);

            return client;
        }

        private bool ValidateAppointmentFields()
        {
            foreach (ComboBox comboBox in comboBoxes)
                if (comboBox.SelectedItem == null)
                    return false;
            if (String.IsNullOrEmpty(textBox2.Text) || String.IsNullOrWhiteSpace(textBox2.Text))
                return false;
            return true;
        }

        private void button2_Click(object sender, EventArgs e)  // Заявка
        {
            if (ValidateAppointmentFields())
            {
                string type = comboBox1.SelectedItem.ToString();
                string option = comboBox2.SelectedItem.ToString();
                string doctor = comboBox3.SelectedItem.ToString();

                List<string> doctors = new List<string>();

                foreach (string elem in comboBox3.Items)
                    doctors.Add(elem);

                Service service = GetSelectedService(type, option, doctors);
                Client client = GetClient();

                if (client == null)
                {
                    MessageBox.Show(StaticData.noClientSaved);
                    return;
                }

                Appointment appointment = new Appointment(client, service, option, textBox2.Text);
                appointment.Serialize();

                AppointmentsList appointmentsList = new AppointmentsList();
                appointmentsList.ShowDialog();
            }
            else
            {
                MessageBox.Show(StaticData.invalidFields);
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)  // Клиент
        {
            if (ValidatePhoneCallFields())
            {
                AddClient addClient = new AddClient(textBox1.Text, maskedTextBox1.Text);
                addClient.ShowDialog();
            }
            else
            {
                MessageBox.Show(StaticData.invalidFields);
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)  // Лог
        {
            CallsLog log = new CallsLog();
            log.Show();
        }

        private List<string> GetDoctors(string service)
        {
            string[] dirs = Directory.GetDirectories(StaticData.servicesFolder);
            string targetDir = string.Empty;

            List<string> empls = new List<string>();

            foreach(string dir in dirs)
                if (dir.EndsWith(service))
                    targetDir = dir;

            if (!File.Exists(targetDir + "/" + StaticData.serviceEmpls))
                return null;

            using (StreamReader reader = new StreamReader(targetDir + "/" + StaticData.serviceEmpls))
            {
                while (!reader.EndOfStream)
                    empls.Add(reader.ReadLine());
            }

            return empls;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)  // Тип услуги
        {
            string selected = comboBox1.SelectedItem.ToString();
            var selectedTypeInTree = treeView1.Nodes.Find(selected, false);

            comboBox2.Items.Clear();
            comboBox3.Items.Clear();

            foreach (TreeNode node in treeView1.Nodes)
            {
                if(node.Text == selected)
                    foreach (TreeNode subnode in node.Nodes)
                        comboBox2.Items.Add(subnode.Text.Replace("|","-"));               
            }

            List<string> doctors = GetDoctors(selected);

            if(doctors != null)
                comboBox3.Items.AddRange(doctors.ToArray());
            else
            {
                MessageBox.Show(StaticData.noDoctAdded);
                return;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)  // Врач
        {
            string[] empls = Directory.GetDirectories(StaticData.employeesFolder);
            string targetDir = string.Empty;
            string res = string.Empty;

            foreach (string dir in empls)
                if (dir.EndsWith(comboBox3.SelectedItem.ToString()))
                    targetDir = dir;

            using(StreamReader reader = new StreamReader(targetDir + "/" + StaticData.workTimesFile))
            {
                while (!reader.EndOfStream)
                {
                    res += reader.ReadLine();
                    res += "\n";
                }
            }

            label12.Text = res;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            AppointmentsList appointmentsList = new AppointmentsList();
            appointmentsList.ShowDialog();
        }
    }
}
