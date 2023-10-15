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
    public partial class AdminFirstStart : Form
    {
        private string errorEquPasswords = "Пароли не должны совпадать!";
        private string rolesFile;

        public delegate void OnNoConfiguredClosing();
        public OnNoConfiguredClosing noConfigClosing;

        public delegate void OnConfiguredClosing();
        public OnConfiguredClosing configClosing;

        public AdminFirstStart(string path)
        {
            InitializeComponent();

            rolesFile = path;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(maskedTextBox1.Text) || String.IsNullOrEmpty(maskedTextBox2.Text))
            {
                MessageBox.Show(StaticData.invalidFields);
                return;
            }

            if(maskedTextBox1.Text == maskedTextBox2.Text)
            {
                MessageBox.Show(errorEquPasswords);
                return;
            }

                File.Create(rolesFile).Close();

            using (StreamWriter writer = new StreamWriter(rolesFile))
            {
                writer.WriteLine(label2.Text + ":" + maskedTextBox1.Text);
                writer.WriteLine(label3.Text + ":" + maskedTextBox2.Text);
            }

            Close();
        }

        private void AdminFirstStart_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!File.Exists(rolesFile))
                noConfigClosing?.Invoke();
            else
                configClosing?.Invoke();
        }

        private void AdminFirstStart_Load(object sender, EventArgs e)
        {

        }
    }
}
