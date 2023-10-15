using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace VetClinicApp
{
    public partial class Form1 : Form
    {
        private string invalidPassword = "Неверный пароль!";
        private string rolesFile = Directory.GetCurrentDirectory() + "/roles";
        private Dictionary<string, string> rolePass = new Dictionary<string, string>();
        private Dictionary<string, Form> roleForm = new Dictionary<string, Form>();

        private List<string> dirsToCreate = new List<string>() { StaticData.employeesFolder, StaticData.servicesFolder, StaticData.appointmentFolder, StaticData.clientsFolder };
        private List<string> filessToCreate = new List<string>() { StaticData.callsFile };
        public Form1()
        {
            InitializeComponent();
            
            label4.Text = DateTime.UtcNow.ToString();

            CheckWorkFolders();
        }

        private void ReadRoles()
        {
            using (StreamReader reader = new StreamReader(rolesFile))
            {
                string[] curr = new string[2];

                while (!reader.EndOfStream)
                {
                    curr = reader.ReadLine().Split(new char[] { ':' });
                    rolePass.Add(curr[0], curr[1]);
                }
            }

            roleForm.Add("Администратор", new AdminMenu(this));
            roleForm.Add("Оператор", new OperatorMenu(this));

            comboBox1.Items.AddRange(rolePass.Keys.ToArray());
        }

        private void FillRoles()
        {
            if(!File.Exists(rolesFile))
            {
                AdminFirstStart adminForm = new AdminFirstStart(rolesFile);
                adminForm.noConfigClosing += ClosingWithoutConfigs;
                adminForm.configClosing += GetCreatedRoles;
                adminForm.ShowDialog();
            }
            else
            {
                ReadRoles();
            }
        }

        private void CheckRolePassword()
        {
            string selectedRole = string.Empty;

            foreach (KeyValuePair<string, string> pair in rolePass)
                if (pair.Key == comboBox1.SelectedItem.ToString())
                    selectedRole = pair.Key;

            if(maskedTextBox1.Text == rolePass[selectedRole])
            {
                Form form;

                if (selectedRole == "Администратор")
                    form = new AdminMenu(this);
                else
                    form = new OperatorMenu(this);

                form.Show();
                Hide();
            }
            else
            {
                MessageBox.Show(invalidPassword);
                return;
            }
        }

        private void CheckWorkFolders()
        {
            foreach(string dir in dirsToCreate)
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

            foreach(string file in filessToCreate)
                if (!File.Exists(file))
                    File.Create(file).Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(maskedTextBox1.Text) && comboBox1.SelectedItem != null)
                CheckRolePassword();
            else
            {
                MessageBox.Show(StaticData.invalidFields);
                return;
            }
        }

        public void ClosingWithoutConfigs()
        {
            Close();
        }

        public void GetCreatedRoles()
        {
            ReadRoles();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            FillRoles();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
